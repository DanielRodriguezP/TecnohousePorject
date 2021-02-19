namespace Sanofi.Web.Reportes
{
    using Microsoft.Reporting.WebForms;
    using Sanofi.BM.Athena;
    using Sanofi.Soporte;
    using System;
    using System.Reflection;
    using Sanofi.Web.Dataset;
    using Sanofi.DT.Athena;
    using System.Collections.Generic;
    using System.Data;

    public partial class ReporteAthena : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DisableUnwantedExportFormat(Viewer, "PDF");
                Reporte();
            }
        }

        public void Reporte()
        {

            DSReportes ds = new DSReportes();
            try
            {
                List<DTAthena> RSAthena = new List<DTAthena>();
                RSAthena = new BMAthena().ConsultarReporte();
                foreach (var data in RSAthena)
                {
                    DataRow dr = ds.Athena.Rows.Add();
                    dr.SetField("PurchasinDoc", data.Document);
                    dr.SetField("Material", data.Material);
                    dr.SetField("ShortTxt", data.ShortText);
                    dr.SetField("OrderQty", data.OrderQty);
                    dr.SetField("NetOrderValue", data.NetOrderValue);
                    dr.SetField("Vendor", data.Vendor);
                    dr.SetField("Currency", data.Currency);
                }



                this.Viewer.PageCountMode = PageCountMode.Actual;
                this.Viewer.ProcessingMode = ProcessingMode.Local;
                this.Viewer.ZoomMode = ZoomMode.Percent;
                this.Viewer.ZoomPercent = 100;
                this.Viewer.LocalReport.ReportPath = Server.MapPath("../Athena/Athena.rdlc");


                this.Viewer.LocalReport.DataSources.Clear();
                this.Viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet", ds.Tables[0]));
                this.Viewer.LocalReport.Refresh();

            }
            catch (Exception ex)
            {
                Viewer.Visible = false;
                GestorLog.RegistrarLogExcepcion(ex);
            }
        }
        public void DisableUnwantedExportFormat(ReportViewer ReportViewerID, string strFormatName)
        {
            FieldInfo info;

            foreach (RenderingExtension extension in ReportViewerID.LocalReport.ListRenderingExtensions())
            {
                if (extension.Name == strFormatName)
                {
                    info = extension.GetType().GetField("m_isVisible", BindingFlags.Instance | BindingFlags.NonPublic);
                    info.SetValue(extension, false);
                }
            }
        }
    }
}