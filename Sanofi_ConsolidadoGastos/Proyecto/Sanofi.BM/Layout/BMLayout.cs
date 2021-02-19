using Sanofi.DM.Layout;
using Sanofi.DT.General;
using Sanofi.DT.Mensajes;
using Sanofi.DT.Usuarios;
using Sanofi.Soporte;
using System;
using System.Collections.Generic;
using System.Linq;
//using Novo.TransferenciaDeValor.DM.Layout;

namespace Sanofi.BM.Layout
{
    public class BMLayout
    {
        #region consultar rol y modalidades a las que tiene permiso el usuario
        public DTResultadoOperacionModel<List<DTMaestroUsuario>> ConsultarRolModalidadUsuario(string Usuario)
        {
            DTResultadoOperacionModel<List<DTMaestroUsuario>> resultado = new DTResultadoOperacionModel<List<DTMaestroUsuario>>();
            try
            {
                resultado = new DMLayout().ConsultarRolModalidadUsuario(Usuario);
            }
            catch (Exception ex)
            {
                GestorLog.RegistrarLogExcepcion(ex);
                throw ex;
            }
            return resultado;
        }
        #endregion

        #region consultar usuario
        public DTResultadoOperacionModel<DTMaestroUsuario> ConsultarUsuario(string _UsuarioWindows)
        {
            DTResultadoOperacionModel<DTMaestroUsuario> resultado = new DTResultadoOperacionModel<DTMaestroUsuario>();
            try
            {
             
                DTMaestroUsuario _Usuario = new DTMaestroUsuario();
               
                _Usuario.Usuario = _UsuarioWindows.Split('\\').Last();
                resultado = new DMLayout().ConsultarUsuario(_Usuario.Usuario);
                if (resultado != null)
                {
                    if (resultado.Resultado.IdUsuario != 0)
                    {
                        resultado.Respuesta = true;
                    }
                    else
                    {
                        resultado.Respuesta = false;
                        resultado.Resultado = _Usuario;
                        resultado.Mensaje = DTMensaje.ObtenerObjetoMensaje(DTCodigoMensajes.MENSAJE021);
                    }
                }

            }
            catch (Exception ex)
            {
                GestorLog.RegistrarLogExcepcion(ex);
                resultado.Mensaje = DTMensaje.ObtenerObjetoMensaje(DTCodigoMensajes.MENSAJE001);
                throw ex;
            }
            return resultado;
        }
        #endregion
    }
}
