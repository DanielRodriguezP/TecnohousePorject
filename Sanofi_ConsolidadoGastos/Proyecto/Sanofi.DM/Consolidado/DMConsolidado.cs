using Sanofi.DM.DataAccesObjects;
using Sanofi.DM.Resources;
using Sanofi.DT.Consolidado;
using Sanofi.DT.General;
using Sanofi.Soporte.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Sanofi.DM.Consolidado
{
    public class DMConsolidado
    {
        private ManagerDM DMAccesoDatos;
        private List<Parameter> Parametros;
        private string _Usuario = string.Empty;
        public DMConsolidado()
        {
            DMAccesoDatos = new ManagerDM();
            Parametros = new List<Parameter>();
            //_Usuario = Convert.ToString(HttpContext.Current.Session["UsuarioLogin"]);
        }
        #region Consultar reporte para excel
        public List<DTConsolidadoExcel> ConsultarConsolidadoExcel(DTFiltros Dtos) 
        {
            List<DTConsolidadoExcel> List = new List<DTConsolidadoExcel>();
            try
            {
                Parametros.Add(new Parameter("@FechaInicio", Dtos.FechaInicial));
                Parametros.Add(new Parameter("@FechaFin", Dtos.FechaFinal));
                DataTable dt = DMAccesoDatos.EjecutarDataSetWithDataTable(ProcedimientosAlmacenados.Sp_ReporteConsolidado, Parametros);
                List = ConvertData.ConvertirDtoToList<DTConsolidadoExcel>(dt);
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
