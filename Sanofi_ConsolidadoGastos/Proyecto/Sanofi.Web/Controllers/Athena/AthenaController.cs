using Newtonsoft.Json;
using Sanofi.BM.Athena;
using Sanofi.DT.Athena;
using Sanofi.DT.Excel;
using Sanofi.DT.General;
using Sanofi.DT.Mensajes;
using Sanofi.Soporte;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Sanofi.Web.Controllers.Athena
{
    public class AthenaController : Controller
    {
        // GET: InscripcionesYMatriculas
        public ActionResult IndexAthena()
        {
            return View();
        }

        public ActionResult ConsultaAthena()
        {
            return View();
        }
        public ActionResult ReporteAthena()
        {
            return View();
        }

        #region Descargar Plantilla de Excel Athena
        public ActionResult DescargarPlantilla()
        {
            string RutaPlantilla = ConfigurationManager.AppSettings.Get("PlantillaAthena");
            DTResultadoOperacionModel<string> _DTResultadoModel = new DTResultadoOperacionModel<string>();
            if (System.IO.File.Exists(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + RutaPlantilla))
            {
                string SubRutaActual = Request.RawUrl;
                string RutaCompleta = Request.Url.OriginalString.ToString();
                var RutaFilal = RutaCompleta.Replace(SubRutaActual, "");


                _DTResultadoModel.Resultado = RutaPlantilla.Replace("\\", "//");
                _DTResultadoModel.Respuesta = true;
            }
            else
            {
                _DTResultadoModel.Mensaje = DTMensaje.ObtenerObjetoMensaje(DTCodigoMensajes.MENSAJE013);
            }
            return Json(_DTResultadoModel, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Obtener documento de excel
        public string ValidarArchivo(string Plantilla, string NombreArchivo, int? Conf = 0)
        {
            string RutaArchivo = string.Empty;
            DTResultadoOperacionModel<DTAthena> DTResultadoModel = new DTResultadoOperacionModel<DTAthena>();
            DTResultadoOperacionModel<DTErroresExcel> DTResultadoModelCarga = new DTResultadoOperacionModel<DTErroresExcel>();
            int ConEliminacion = 0;

            try
            {
                DTExcel _Plantilla = JsonConvert.DeserializeObject<DTExcel>(Plantilla);
                List<DTFilasExcel> filas = _Plantilla.sheets.First().rows;
                if (filas.Count > 1)
                {

                    //Validar el encabezado
                    List<DTColumnasExcel> columnas = filas.First().cells;

                    if (columnas[0].value.ToString() == DTEstructuraAthena.Item
                       && columnas[1].value.ToString() == DTEstructuraAthena.PurchasingDocument
                       && columnas[2].value.ToString() == DTEstructuraAthena.DocumentDate
                       && columnas[3].value.ToString() == DTEstructuraAthena.Material
                       && columnas[4].value.ToString() == DTEstructuraAthena.ShortText
                       && columnas[5].value.ToString() == DTEstructuraAthena.OrderQuantity
                       && columnas[6].value.ToString() == DTEstructuraAthena.StillDelivered
                       && columnas[7].value.ToString() == DTEstructuraAthena.OrderUnit
                       && columnas[8].value.ToString() == DTEstructuraAthena.Netprice
                       && columnas[9].value.ToString() == DTEstructuraAthena.NetOrderValue
                       && columnas[10].value.ToString() == DTEstructuraAthena.VendorSupplyingPlant
                       && columnas[11].value.ToString() == DTEstructuraAthena.Currency
                       && columnas[12].value.ToString() == DTEstructuraAthena.ReleaseState
                       && columnas[13].value.ToString() == DTEstructuraAthena.DeletionIndicator
                       && columnas[14].value.ToString() == DTEstructuraAthena.POHistory)
                    {
                        List<DTFilasExcel> ConInformacion = (from dt in filas
                                                             where dt.cells.Count(x => x.value == null) != dt.cells.Count()
                                                             && dt.index != 0
                                                             select dt).ToList();
                        if (ConInformacion.Count > 0)
                        {

                            foreach (DTFilasExcel Fila in ConInformacion)
                            {

                                if (Fila.cells[Fila.cells.Count - 2].index == 13 && Fila.cells[Fila.cells.Count - 2].value != null)
                                {
                                    if (Fila.cells[Fila.cells.Count - 2].value.ToString() != "")
                                        ConEliminacion++;  //Si finalmente es mayor que cero entonces significa que hay al menos una eliminación.
                                }

                            }

                            if (ConEliminacion > 0 && Conf == 0)
                            {

                                DTErroresExcel Eliminar = new DTErroresExcel();
                                Eliminar.Fila = 1;

                                //Preguntar al usuario si realmente quiere reemplazar
                                DTResultadoModelCarga.Respuesta = false;
                                DTResultadoModelCarga.Resultado = Eliminar;
                                DTResultadoModelCarga.Mensaje = DTMensaje.ObtenerObjetoMensaje(DTCodigoMensajes.MENSAJE007);
                                //Realmente desea reemplazar usuarios?
                            }
                            else
                            {
                                BMAthena _BMIM = new BMAthena();
                                DTResultadoModelCarga = _BMIM.LeerArchivo(ConInformacion, NombreArchivo);
                            }


                        }
                        else
                        {
                            DTResultadoModelCarga.Respuesta = false;
                            DTResultadoModelCarga.Mensaje = DTMensaje.ObtenerObjetoMensaje(DTCodigoMensajes.MENSAJE004);
                        }
                    }
                    else
                    {
                        DTResultadoModelCarga.Respuesta = false;
                        DTResultadoModelCarga.Mensaje = DTMensaje.ObtenerObjetoMensaje(DTCodigoMensajes.MENSAJE002);
                    }
                }
                else
                {

                    DTResultadoModelCarga.Respuesta = false;
                    DTResultadoModelCarga.Mensaje = DTMensaje.ObtenerObjetoMensaje(DTCodigoMensajes.MENSAJE004);
                }
            }
            catch (Exception ex)
            {
                DTResultadoModelCarga.Respuesta = false;
                DTResultadoModelCarga.Mensaje = DTMensaje.ObtenerObjetoMensaje(DTCodigoMensajes.MENSAJE001);
                GestorLog.RegistrarLogExcepcion(ex);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(DTResultadoModelCarga);
        }
        #endregion

        #region Consultar Registros Exitosos
        public string ConsultarregistrosExitosos()
        {
            DTResultadoOperacionList<DTAthena> ResultadoList = new DTResultadoOperacionList<DTAthena>();
            List<DTAthena> Resultado = new List<DTAthena>();

            try
            {
                Resultado = new BMAthena().ConsultarRegistrosEx();
                ResultadoList.Resultado = true;
                ResultadoList.Datos = Resultado;
            }
            catch (Exception ex)
            {

                GestorLog.RegistrarLogExcepcion(ex);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 500000000;
            return serializer.Serialize(ResultadoList);
        }
        #endregion

        #region Consultar registros del excel , con inconvenientes
        public string ConsultarErroresExcel()
        {
            DTResultadoOperacionList<DTErroresExcel> ResultadoList = new DTResultadoOperacionList<DTErroresExcel>();
            List<DTErroresExcel> Resultado = new List<DTErroresExcel>();

            try
            {
                Resultado = new BMAthena().ConsultarErroresExcel();
                ResultadoList.Resultado = true;
                ResultadoList.Datos = Resultado;
            }
            catch (Exception ex)
            {

                GestorLog.RegistrarLogExcepcion(ex);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 500000000;
            return serializer.Serialize(ResultadoList);
        }
        #endregion

        #region Consultar Athena Filtros
        public string ConsultarAthenaFiltros(DTFiltros Dt)
        {
            DTResultadoOperacionList<DTAthena> ResultadoList = new DTResultadoOperacionList<DTAthena>();
            List<DTAthena> Resultado = new List<DTAthena>();

            try
            {
                Resultado = new BMAthena().ConsultarAthenaFiltros(Dt);
                ResultadoList.Resultado = true;
                ResultadoList.Datos = Resultado;
            }
            catch (Exception ex)
            {
                GestorLog.RegistrarLogExcepcion(ex);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 500000000;
            return serializer.Serialize(ResultadoList);
        }
        #endregion

    }
}