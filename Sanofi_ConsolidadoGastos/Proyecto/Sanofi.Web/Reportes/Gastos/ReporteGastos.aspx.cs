using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sanofi.Web.Reportes.Gastos
{
    public partial class ReporteGastos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string Cuenta = Request.QueryString["Cuentas"];
                //string Tipos = Request.QueryString["Tipos"];
                //string Concatenar = Request.QueryString["Concatenar"];
                DisableUnwantedExportFormat(Viewer, "PDF");
                //Reporte();
            }
        }

        public void carga (string Cuentas, string Tipos, string Concatenar)
        {
            string Cuenta = Cuentas;
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