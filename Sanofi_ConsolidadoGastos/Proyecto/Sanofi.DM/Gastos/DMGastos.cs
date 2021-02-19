namespace Sanofi.DM.Gastos
{
    using Sanofi.DM.DataAccesObjects;
    using Sanofi.DM.Resources;
    using Sanofi.DT.Excel;
    using Sanofi.DT.Gastos;
    using Sanofi.DT.General;
    using Sanofi.DT.Solped;
    using Sanofi.Soporte.Utils;
    using System;
    using System.Collections.Generic;
    using System.Data;
    public class DMGastos
    {
        private ManagerDM DMAccesoDatos;
        private List<Parameter> Parametros;
        //private string _Usuario = string.Empty;
        public DMGastos()
        {
            DMAccesoDatos = new ManagerDM();
            Parametros = new List<Parameter>();
            //_Usuario = Convert.ToString(HttpContext.Current.Session["UsuarioLogin"]);
        }


        #region Registrar informacion .xlsx
        public void InsertarGastos(string Xml, string mesGasto)
        {
            try
            {
                Parametros.Add(new Parameter("@IN_Xml", Xml));
                Parametros.Add(new Parameter("@FechaGasto", mesGasto));
                DMAccesoDatos.EjecutarNonQuery(ProcedimientosAlmacenados.Sp_InsertarGastos, Parametros);
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
                Parametros.Add(new Parameter("@IdModalidad", Constantes.IdModGastos));
                DMAccesoDatos.EjecutarNonQuery(ProcedimientosAlmacenados.Sp_InsertarErrores, Parametros);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Listar Solped
        public List<DTGastos> ConsultaRegistroExitoso()
        {
            List<DTGastos> List = new List<DTGastos>();
            try
            {
                DataTable Gastos = DMAccesoDatos.EjecutarDataSetWithDataTable(ProcedimientosAlmacenados.Sp_ConsultaRegistrosExitososGastos, Parametros);
                List = ConvertData.ConvertirDtoToList<DTGastos>(Gastos);
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
                Parametros.Add(new Parameter("@IdModalidad", Constantes.IdModGastos));
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
        public List<DTGastos> ConsultarGastosFiltros(DTFiltros Dtos)
        {
            List<DTGastos> List = new List<DTGastos>();
            try
            {
                string fecha = "01/" + Dtos.FechaInicial + "/" + Dtos.FechaFinal;
                Parametros.Add(new Parameter("@Fecha", fecha));
                DataTable dt = DMAccesoDatos.EjecutarDataSetWithDataTable(ProcedimientosAlmacenados.Sp_ConsultaGastosFiltros, Parametros);
                List = ConvertData.ConvertirDtoToList<DTGastos>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return List;
        }
        #endregion

        #region Consultar Cuentas
        public List<DTObjetosFiltros> ConsultarCuentas(DTFiltros Dtos)
        {
            List<DTObjetosFiltros> List = new List<DTObjetosFiltros>();
            try
            {
                string fecha = "01/" + Dtos.FechaInicial + "/" + Dtos.FechaFinal;
                Parametros.Add(new Parameter("@Fecha", fecha));
                DataTable dt = DMAccesoDatos.EjecutarDataSetWithDataTable(ProcedimientosAlmacenados.Sp_ListarGastosCuentas, Parametros);
                List = ConvertData.ConvertirDtoToList<DTObjetosFiltros>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return List;
        }
        #endregion
        #region Consultar Tipos
        public List<DTObjetosFiltros> ConsultarTipos(DTFiltros Dtos)
        {
            List<DTObjetosFiltros> List = new List<DTObjetosFiltros>();
            try
            {
                string fecha = "01/" + Dtos.FechaInicial + "/" + Dtos.FechaFinal;
                Parametros.Add(new Parameter("@Fecha", fecha));
                DataTable dt = DMAccesoDatos.EjecutarDataSetWithDataTable(ProcedimientosAlmacenados.Sp_ListarGastosTipos, Parametros);
                List = ConvertData.ConvertirDtoToList<DTObjetosFiltros>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return List;
        }
        #endregion
        #region Consultar Concatenar
        public List<DTObjetosFiltros> ConsultarConcatenar(DTFiltros Dtos)
        {
            List<DTObjetosFiltros> List = new List<DTObjetosFiltros>();
            try
            {
                string fecha = "01/" + Dtos.FechaInicial + "/" + Dtos.FechaFinal;
                Parametros.Add(new Parameter("@Fecha", fecha));
                DataTable dt = DMAccesoDatos.EjecutarDataSetWithDataTable(ProcedimientosAlmacenados.Sp_ListarGastosConcatenar, Parametros);
                List = ConvertData.ConvertirDtoToList<DTObjetosFiltros>(dt);
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
