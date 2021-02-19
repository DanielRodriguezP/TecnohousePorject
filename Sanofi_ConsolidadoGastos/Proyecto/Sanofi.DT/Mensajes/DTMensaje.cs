using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sanofi.DT.Mensajes
{
    internal class JsonMensajes
    {
        public List<DTMensaje> Mensajes { get; set; }
    }

    public enum TipoMensaje
    {
        Advertencia = 1,
        Ok = 2,
        Error = 3,
        Informacion = 4,
        Confirmacion = 5,
        Progreso = 7,
        ConfirmacionSN = 8
    }

    public class DTMensaje
    {
        #region Constantes
        private const string NOMBRE_ARCHIVO_MENSAJES = "Mensajes.json";
        private const string MENSAJE_ARCHIVO = "No se encontro el archivo: ";
        #endregion

        #region Constructores
        public DTMensaje()
        {

        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene o establece el código del mensaje
        /// </summary>
        public int Codigo { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de mensaje
        /// </summary>
        public TipoMensaje Tipo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Texto { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string[] Parametros { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Imagen { get; set; }
        #endregion

        public static string ObtenerMensaje(int codigo)
        {
            return ObtenerMensaje(codigo, null).Texto;
        }

        /// <summary>
        /// Obtiene la ruta del archivo de mensajes<
        /// </summary>
        /// <returns></returns>
        private static string ObtenerRutaArchivoMensajes()
        {
            string rutaArchivoConfiguracion = string.Empty;
            rutaArchivoConfiguracion = $"{ AppDomain.CurrentDomain.RelativeSearchPath}\\Mensajes\\";
            return rutaArchivoConfiguracion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static IConfigurationBuilder ObtenerArchivoMensajes()
        {
            string rutaArchivo = ObtenerRutaArchivoMensajes();
            if (!File.Exists($"{rutaArchivo}\\{NOMBRE_ARCHIVO_MENSAJES}"))
            {
                try
                {
                    string mensaje = string.Format(MENSAJE_ARCHIVO, rutaArchivo);
                }
                catch (FileNotFoundException fileEx)
                {
                    throw new FileNotFoundException(fileEx.ToString());
                }
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(rutaArchivo)
                .AddJsonFile(NOMBRE_ARCHIVO_MENSAJES);

            return builder;
        }

        public static DTMensaje ObtenerObjetoMensaje(int codigo)
        {
            return ObtenerMensaje(codigo, null);
        }

        /// <summary>
        /// Obtiene las propiedades de un mensaje
        /// </summary>
        /// <param name="codigo">Codigo unico del mensaje a consultar</param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public static DTMensaje ObtenerMensaje(int codigo, params string[] parametros)
        {
            var jsonMensajes = new JsonMensajes();
            DTMensaje mensaje = new DTMensaje();
            var config = ObtenerArchivoMensajes();

            IConfigurationRoot Configuracion = config.Build();
            Configuracion.Bind(jsonMensajes);
            mensaje = jsonMensajes.Mensajes.Where(m => m.Codigo == codigo).FirstOrDefault();
            if (parametros != null)
            {
                mensaje.Texto = string.Format(mensaje.Texto, parametros);
            }
            return mensaje;
        }
    }

}
