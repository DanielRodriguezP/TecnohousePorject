namespace Sanofi.DM.Solped
{
    using Sanofi.DM.DataAccesObjects;
    using Sanofi.DM.Resources;
    using Sanofi.DT.Excel;
    using Sanofi.DT.General;
    using Sanofi.DT.Solped;
    using Sanofi.Soporte.Utils;
    using System;
    using System.Collections.Generic;
    using System.Data;
    public class DMSolped
    {
        private ManagerDM DMAccesoDatos;
        private List<Parameter> Parametros;
        //private string _Usuario = string.Empty;
        public DMSolped()
        {
            DMAccesoDatos = new ManagerDM();
            Parametros = new List<Parameter>();
            //_Usuario = Convert.ToString(HttpContext.Current.Session["UsuarioLogin"]);
        }


        #region Registrar informacion .xlsx
        public void InsertarSolped(string Xml)
        {
            try
            {
                Parametros.Add(new Parameter("@IN_Xml", Xml));
                DMAccesoDatos.EjecutarNonQuery(ProcedimientosAlmacenados.Sp_InsertarSolped, Parametros);
                            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Registrar informacion de errores del .xlsx
        public void InsertarErroresExcel(string Xml)
        {
            try
            {
                Parametros.Add(new Parameter("@IN_Xml", Xml));
                Parametros.Add(new Parameter("@IdModalidad", Constantes.IdModSolped));
                DMAccesoDatos.EjecutarNonQuery(ProcedimientosAlmacenados.Sp_InsertarErrores, Parametros);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Listar Solped
        public List<DTSolped> ConsultaRegistroExitoso()
        {
            List<DTSolped> List = new List<DTSolped>();
            try
            {
                DataTable Solped = DMAccesoDatos.EjecutarDataSetWithDataTable(ProcedimientosAlmacenados.Sp_ConsultaRegistrosExitososSolped, Parametros);
                List = ConvertData.ConvertirDtoToList<DTSolped>(Solped);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return List;
        }
        #endregion

        #region Consultar datos Incorrectos
        public List<DTErroresExcel> ConsultarErroresExcel()
        {
            List<DTErroresExcel> List = new List<DTErroresExcel>();
            try
            {
                Parametros.Add(new Parameter("@IdModalidad", Constantes.IdModSolped));
                DataTable Solped = DMAccesoDatos.EjecutarDataSetWithDataTable(ProcedimientosAlmacenados.Sp_ConsultaErroresExcel, Parametros);
                List = ConvertData.ConvertirDtoToList<DTErroresExcel>(Solped);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return List;
        }
        #endregion

        #region Consultar segun los filtros
        public List<DTSolped> ConsultarSolpedFiltros(DTFiltros Dtos)
        {
            List<DTSolped> List = new List<DTSolped>();
            try
            {
                Parametros.Add(new Parameter("@FechaInicial", Dtos.FechaInicial));
                Parametros.Add(new Parameter("@FechaFinal", Dtos.FechaFinal));
                DataTable dt = DMAccesoDatos.EjecutarDataSetWithDataTable(ProcedimientosAlmacenados.Sp_ConsultaSolpedFiltros, Parametros);
                List = ConvertData.ConvertirDtoToList<DTSolped>(dt);
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
