using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Sanofi.DT.General;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Sanofi.Soporte
{
    public class GestorLog
    {
        #region Constantes

        /// <summary>
        /// Constante para manejar el nombre de la categoria General de excepciones.
        /// </summary>
        private const string CategoriaLogExcepciones = "LogExcepciones";

        /// <summary>
        /// Constante para manejar el nombre de la categoria de excepciones de negocio.
        /// </summary>
        private const string CategoriaLogExcepcionesNegocio = "LogExcepcionesNegocio";
        /// <summary>
        /// The categoria log integraciones envio
        /// </summary>
        private const string CategoriaLogEventos = "LogEventos";

        #endregion

        #region Metodos

        #region RegistrarLogExcepcion

        /// <summary>
        /// Registra un log de excepción.
        /// </summary>
        /// <param name="parametros">Excepción a registrar.</param>
        /// <author>"Alexander Gonzalez Valencia"</author>
        public static void RegistrarLogExcepcion(Exception parametros)
        {
            DTNegocioException ExcepcionNegocio = new DTNegocioException();
            if (parametros.GetType() != ExcepcionNegocio.GetType())
            {
                RegistrarInformacionBasica(parametros, CategoriaLogExcepciones);
            }
        }

        #endregion

        #region RegistrarLogNegocio

        /// <summary>
        /// Registra un log de excepción.
        /// </summary>
        /// <param name="parametros">Excepción a registrar.</param>
        /// <author>
        /// "Alexander Gonzalez Valencia"
        /// </author>
        public static void RegistrarLogNegocio(Exception parametros)
        {
            DTNegocioException ExcepcionNegocio = new DTNegocioException();
            if (parametros.GetType() != ExcepcionNegocio.GetType())
            {
                RegistrarInformacionBasica(parametros, CategoriaLogExcepcionesNegocio);
            }
        }

        #endregion

        #region RegistrarInformacionBasica

        /// <summary>
        /// Metodo privado para registrar la excepcion en el log 
        /// </summary>
        /// <param name="parametros">Objeto Excepcion </param>
        /// <param name="CategoriaLog">Tipo de categoria a Registrar.</param>
        private static void RegistrarInformacionBasica(Exception parametros, string CategoriaLog)
        {
            LogEntry log = new LogEntry();
            LogEntry logEntry = new LogEntry();
            log.Categories.Add(CategoriaLog);
            log.ExtendedProperties.Add("Fecha y hora de la operación:", DateTime.Now);
            log.ExtendedProperties.Add("\r\nTipo de excepción:", parametros.GetBaseException().GetType().ToString());
            log.ExtendedProperties.Add("\r\nMensaje técnico:", ObtenerMensajeExcepcion(parametros));
            if (!string.IsNullOrEmpty(parametros.StackTrace))
            {
                log.ExtendedProperties.Add("\r\nPila del error:", parametros.StackTrace);
            }
            //Logger.Write(log);
            IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();
            LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);
            Logger.SetLogWriter(logWriterFactory.Create(), false);

            Logger.Write(log);
        }

        #endregion

        #region RegistrarLogExcepcion

        /// <summary>
        /// Registra un log de excepción.
        /// </summary>
        /// <param name="parametros">Excepción a registrar.</param>
        /// <author>"Alexander Gonzalez Valencia"</author>
        public static void RegistrarLogExcepcion(Exception parametros, object Obj)
        {
            try
            {
                //Todo: Falta cambiar el idioma parametrizable en los encabezados del log 
                LogEntry log = new LogEntry();
                log.Categories.Add(CategoriaLogExcepcionesNegocio);
                log.ExtendedProperties.Add("Fecha y hora de la operación:", DateTime.Now);
                log.ExtendedProperties.Add("\r\nTipo de excepción:", parametros.GetBaseException().GetType().ToString());
                log.ExtendedProperties.Add("\r\nMensaje técnico:", ObtenerMensajeExcepcion(parametros));
                if (!string.IsNullOrEmpty(parametros.StackTrace))
                {
                    log.ExtendedProperties.Add("\r\nPila del error:", parametros.StackTrace);
                }
                List<object> ReferenceData = new List<object>();
                string complemento = string.Empty;
                if (Obj.GetType() == ReferenceData.GetType())
                {
                    foreach (object elemento in (List<object>)Obj)
                    {
                        foreach (MemberInfo mi in elemento.GetType().GetMembers())
                        {
                            if (mi.MemberType == MemberTypes.Property)
                            {

                                PropertyInfo pi = mi as PropertyInfo;
                                if (pi != null)
                                {
                                    if (pi.GetValue(elemento, null) != null)
                                    {
                                        complemento = complemento + pi.Name + ": " + pi.GetValue(elemento, null).ToString() + "\r\n";
                                    }
                                    else
                                    {
                                        complemento = complemento + pi.Name + ": null \r\n";
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (MemberInfo mi in Obj.GetType().GetMembers())
                    {
                        if (mi.MemberType == MemberTypes.Property)
                        {
                            PropertyInfo pi = mi as PropertyInfo;
                            if (pi != null)
                            {
                                if (pi.GetValue(Obj, null) != null)
                                {
                                    complemento = complemento + pi.Name + ": " + pi.GetValue(Obj, null).ToString() + "\r\n";
                                }
                                else
                                {
                                    complemento = complemento + pi.Name + ": null \r\n";
                                }
                            }
                        }
                    }
                }
                log.ExtendedProperties.Add("\r\nInformacion Adicional: ", complemento);

                Logger.Write(log);
            }
            catch
            {
                RegistrarInformacionBasica(parametros, CategoriaLogExcepcionesNegocio);
            }
        }

        #endregion

        #region RegistrarEvento

        /// <summary>
        /// Registrars the evento.
        /// </summary>
        /// <param name="mensaje">The mensaje.</param>
        public static void RegistrarEvento(string mensaje)
        {
            LogEntry log = new LogEntry();
            log.Categories.Add(CategoriaLogEventos);
            log.ExtendedProperties.Add("Fecha y hora de la operación:", DateTime.Now);
            log.ExtendedProperties.Add("\r\nTipo de excepción:", "Mensaje de información");
            log.ExtendedProperties.Add("\r\nMensaje:", mensaje);
            Logger.Write(log);
        }

        #endregion

        #region ObtenerMensajeExcepcion

        /// <summary>
        /// Permite obtener la cadena completa del mensaje de la excepcion
        /// buscando las excepciones internas que tenga el mensaje.
        /// </summary>
        /// <param name="excepcion">Excepcion generada.</param>
        /// <returns>Cadena con el mensaje de la excepcion tanto externa como interna(s).</returns>
        internal static string ObtenerMensajeExcepcion(Exception excepcion)
        {
            StringBuilder cadenaMensajeExcepcion = new StringBuilder(excepcion.Message);
            Exception excepcionInterna = excepcion.InnerException;
            while (excepcionInterna != null)
            {
                cadenaMensajeExcepcion.AppendFormat(CultureInfo.InvariantCulture, "{0}Message: {1}{2}Source: {3}{4}StackTrace:{5}{6}TargetSite:{7}{8}Body{9}", Environment.NewLine, excepcionInterna.Message, Environment.NewLine, excepcionInterna.Source, Environment.NewLine, excepcionInterna.StackTrace, Environment.NewLine, excepcionInterna.TargetSite, Environment.NewLine, excepcionInterna.TargetSite == null ? "" : excepcionInterna.TargetSite.GetMethodBody().ToString());
                excepcionInterna = excepcionInterna.InnerException;
            }
            return cadenaMensajeExcepcion.ToString();
        }


        #endregion

        #endregion
    }
}
