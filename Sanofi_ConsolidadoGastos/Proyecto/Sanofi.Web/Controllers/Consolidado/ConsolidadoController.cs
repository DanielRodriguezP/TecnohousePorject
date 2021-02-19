using Sanofi.BM.Consolidado;
using Sanofi.DT.Consolidado;
using Sanofi.DT.General;
using Sanofi.DT.Mensajes;
using Sanofi.Soporte;
using Sanofi.Soporte.Seguridad;
using Sanofi.Soporte.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Sanofi.Web.Controllers
{
    public class ConsolidadoController : Controller
    {
        // GET: Consolidado
        public ActionResult IndexConsolidado()
        {
            return View();
        }

        public ActionResult ExportToExcel(DTFiltros Dtos)
        {

            //DTResultadoOperacionModel<DTConsolidadoExcel> resultado = new DTResultadoOperacionModel<DTConsolidadoExcel>();
            DTResultadoOperacionModel<string> respuesta = new DTResultadoOperacionModel<string>();
            List<DTConsolidadoExcel> list = new List<DTConsolidadoExcel>();
            BMConsolidado bMConsolidado = new BMConsolidado();
            list = bMConsolidado.ConsultarConsolidadoExcel(Dtos);
            if (list.Count > 0)
            {
                DataTable dataTable = ConvertData.ConvertToDataTable<DTConsolidadoExcel>(list);
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(dataTable);

                string numbreArchivo = ConfigurationManager.AppSettings.Get("ReporteExcelConsolidado");
                string rutaArchivo = string.Empty;
                try
                {
                    string RutaServidor = (string)new AppSettingsReader().GetValue("RutaServidor", typeof(string));
                    string SubRutaActual = Request.RawUrl;
                    string RutaPlantilla = Request.Url.OriginalString.ToString();
                    var RutaFinal = RutaPlantilla.Replace(SubRutaActual, "");

                    rutaArchivo = RutaFinal + @"/" + RutaServidor + numbreArchivo + ".xlsx";
                    string rutaVirtual = Server.MapPath("~/Excel/");
                    string rutaCompleta = rutaVirtual + numbreArchivo + ".xlsx";

                    WindowsImpersonationContext logon = null;

                    Boolean Suplantacion = (Boolean)new AppSettingsReader().GetValue("Impersonate", typeof(Boolean));
                    if (Suplantacion)
                    {
                        string DominioSuplantacion = (string)new AppSettingsReader().GetValue("DominioSuplantacion", typeof(string));
                        string UsuarioSuplantacion = (string)new AppSettingsReader().GetValue("UsuarioSuplantacion", typeof(string));
                        string ClaveSuplantacion = (string)new AppSettingsReader().GetValue("ClaveSuplantacion", typeof(string));
                        using (new Impersonation(DominioSuplantacion, UsuarioSuplantacion, ClaveSuplantacion))
                        {
                            ConvertData.ToExcelFile(dataTable, rutaCompleta);
                        }
                    }
                    else
                    {
                        ConvertData.ToExcelFile(dataTable, rutaCompleta);
                    }
                    respuesta.Respuesta = true;
                    respuesta.Resultado = rutaArchivo;
                }
                catch (Exception ex)
                {
                    respuesta.Mensaje = DTMensaje.ObtenerObjetoMensaje(DTCodigoMensajes.MENSAJE023);
                    respuesta.Resultado = null;
                    respuesta.Respuesta = false;
                    GestorLog.RegistrarLogExcepcion(ex);
                }
            }
            else
            {
                respuesta.Mensaje = DTMensaje.ObtenerObjetoMensaje(DTCodigoMensajes.MENSAJE024);
                respuesta.Resultado = null;
                respuesta.Respuesta = false;
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
    }
}