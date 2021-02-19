using Sanofi.DM.DataAccesObjects;
using Sanofi.DM.Resources;
using Sanofi.DT.Excel;
using Sanofi.DT.General;
using Sanofi.DT.Athena;
using Sanofi.DT.Mensajes;
using Sanofi.Soporte;
using Sanofi.Soporte.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Sanofi.DM.Athena
{
    public class DMAthena
    {
        private ManagerDM DMAccesoDatos;
        private List<Parameter> Parametros;
        private string _Usuario = string.Empty;
        public DMAthena()
        {
            DMAccesoDatos = new ManagerDM();
            Parametros = new List<Parameter>();
            _Usuario = Convert.ToString(HttpContext.Current.Session["UsuarioLogin"]);
        }


        #region Registrar informacion .xlsx
        public DTResultadoOperacionModel<DTAthena> InsertarAthena(string Xml)
        {
            DTAthena _DTIM = new DTAthena();
            DTResultadoOperacionModel<DTAthena> Resultado = new DTResultadoOperacionModel<DTAthena>();
            try
            {

                Parametros.Add(new Parameter("@IN_Xml", Xml));
                int respuesta = DMAccesoDatos.EjecutarNonQuery(ProcedimientosAlmacenados.Sp_InsertarAthena, Parametros);

                if (respuesta > 0)
                {
                    Resultado.Respuesta = true;
                }
                else
                {
                    Resultado.Mensaje = DTMensaje.ObtenerObjetoMensaje(DTCodigoMensajes.MENSAJE001);
                    Resultado.Respuesta = false;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return Resultado;
        }
        #endregion

        #region Registrar informacion de errores del .xlsx
        public DTResultadoOperacionModel<DTErroresExcel> InsertarErroresExcel(string Xml)
        {
            DTResultadoOperacionModel<DTErroresExcel> Resultado = new DTResultadoOperacionModel<DTErroresExcel>();
            try
            {
                Parametros.Add(new Parameter("@IN_Xml", Xml));
                Parametros.Add(new Parameter("@IdModalidad", Constantes.IdModAthena));
                int respuesta =  DMAccesoDatos.EjecutarNonQuery(ProcedimientosAlmacenados.Sp_InsertarErrores, Parametros);

                if (respuesta > 0)
                {
                    Resultado.Respuesta = true;
                }
                else
                {
                    Resultado.Mensaje = DTMensaje.ObtenerObjetoMensaje(DTCodigoMensajes.MENSAJE001);
                    Resultado.Respuesta = false;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return Resultado;
        }
        #endregion

        #region Listar Athena
        public List<DTAthena> ConsultaRegistroExitoso()
        {
            List<DTAthena> List = new List<DTAthena>();
            try
            {
                DataTable Athena = DMAccesoDatos.EjecutarDataSetWithDataTable(ProcedimientosAlmacenados.Sp_ConsultaRegistrosExitososAthena, Parametros);
                List = ConvertData.ConvertirDtoToList<DTAthena>(Athena);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return List;
        }
        #endregion

        #region Consultar datos Incorrectos Athena
        public List<DTErroresExcel> ConsultarErroresExcel()
        {
            List<DTErroresExcel> List = new List<DTErroresExcel>();
            try
            {
                Parametros.Add(new Parameter("@IdModalidad", Constantes.IdModAthena));
                DataTable Athena = DMAccesoDatos.EjecutarDataSetWithDataTable(ProcedimientosAlmacenados.Sp_ConsultaErroresExcel, Parametros);
                List = ConvertData.ConvertirDtoToList<DTErroresExcel>(Athena);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return List;
        }
        #endregion

        #region Consultar Athena segun los filtros
        public List<DTAthena> ConsultarAthenaFiltros(DTFiltros Dtos)
        {
            List<DTAthena> List = new List<DTAthena>();
            try
            {
                Parametros.Add(new Parameter("@FechaInicial", Dtos.FechaInicial));
                Parametros.Add(new Parameter("@FechaFinal", Dtos.FechaFinal));
                DataTable dt = DMAccesoDatos.EjecutarDataSetWithDataTable(ProcedimientosAlmacenados.Sp_ConsultaAthenaFiltros, Parametros);
                List = ConvertData.ConvertirDtoToList<DTAthena>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return List;
        }
        #endregion

        #region Consultar Athena segun los filtros
        public List<DTAthena> ConsultarReporte()
        {
            List<DTAthena> List = new List<DTAthena>();
            try
            {
                DataTable dt = DMAccesoDatos.EjecutarDataSetWithDataTable(ProcedimientosAlmacenados.Sp_ReportesAthena);
                List = ConvertData.ConvertirDtoToList<DTAthena>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return List;
        }
        #endregion

    }
}
