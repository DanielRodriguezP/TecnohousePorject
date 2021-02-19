using Sanofi.DM.DataAccesObjects;
using Sanofi.DM.Resources;
using Sanofi.DT.General;
using Sanofi.DT.Mensajes;
using Sanofi.DT.Usuarios;
using Sanofi.Soporte;
using Sanofi.Soporte.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanofi.DM.Layout
{
    public class DMLayout
    {
        private ManagerDM DMAccesoDatos;
        private List<Parameter> Parametros;
        public DMLayout()
        {
            DMAccesoDatos = new ManagerDM();
            Parametros = new List<Parameter>();
        }

        #region Consulta los roles y las modalidades a las que tiene permiso el usuario
        public DTResultadoOperacionModel<List<DTMaestroUsuario>> ConsultarRolModalidadUsuario(string Usuario)
        {
            DTMaestroUsuario _DTMU = new DTMaestroUsuario();
            DTResultadoOperacionModel<List<DTMaestroUsuario>> Resultado = new DTResultadoOperacionModel<List<DTMaestroUsuario>>();
            List<DTMaestroUsuario> _DTUsuario = new List<DTMaestroUsuario>();
            try
            {

                Parametros.Add(new Parameter("@IN_Usuario", Usuario));
                DataTable DT_Usuarios = null;//DMAccesoDatos.EjecutarDataSetWithDataTable(ProcedimientosAlmacenados.Sp_ListarRolesModalidades, Parametros);
                _DTUsuario = ConvertData.ConvertirDtoToList<DTMaestroUsuario>(DT_Usuarios);

                if (_DTUsuario != null)
                {
                    Resultado.Respuesta = true;
                    Resultado.Resultado = _DTUsuario;
                }
                else
                {
                    Resultado.Mensaje = DTMensaje.ObtenerObjetoMensaje(DTCodigoMensajes.MENSAJE001);
                    Resultado.Respuesta = false;
                }

            }
            catch (Exception ex)
            {
                GestorLog.RegistrarLogExcepcion(ex);
            }
            return Resultado;
        }
        #endregion

        #region Consulta el usuario
        public DTResultadoOperacionModel<DTMaestroUsuario> ConsultarUsuario(string Usuario)
        {
            DTMaestroUsuario _DTMU = new DTMaestroUsuario();
            DTResultadoOperacionModel<DTMaestroUsuario> Resultado = new DTResultadoOperacionModel<DTMaestroUsuario>();
            DTMaestroUsuario _DTUsuario = new DTMaestroUsuario();
            try
            {

                Parametros.Add(new Parameter("@IN_Usuario", Usuario));
                DataTable DT_Usuarios = null;//DMAccesoDatos.EjecutarDataSetWithDataTable(ProcedimientosAlmacenados.Sp_ValidarUsuario, Parametros);
                _DTUsuario = ConvertData.ConvertirDtoToList<DTMaestroUsuario>(DT_Usuarios).FirstOrDefault();
                Resultado.Resultado = _DTUsuario;
            }
            catch (Exception ex)
            {
                GestorLog.RegistrarLogExcepcion(ex);
            }
            return Resultado;
        }
        #endregion
    }
}
