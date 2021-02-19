using Newtonsoft.Json;
using Sanofi.BM.Solped;
using Sanofi.DT.Excel;
using Sanofi.DT.General;
using Sanofi.DT.Mensajes;
using Sanofi.DT.Solped;
using Sanofi.Soporte;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Sanofi.Web.Controllers.Solped
{
    public class SolpedController : Controller
    {
        // GET: InscripcionesYMatriculas
        public ActionResult IndexSolped()
        {
            return View();
        }

        public ActionResult ConsultaSolped()
        {
            return View();
        }

        #region Descargar Plantilla de Excel Solped
        public ActionResult DescargarPlantilla()
        {
            string RutaPlantilla = ConfigurationManager.AppSettings.Get("PlantillaSolped");
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
        public string ValidarArchivo(string Plantilla)
        {
            DTResultadoOperacionModel<DTSolped> DTResultadoModel = new DTResultadoOperacionModel<DTSolped>();
            DTResultadoOperacionModel<DTErroresExcel> DTResultadoModelCarga = new DTResultadoOperacionModel<DTErroresExcel>();
            //int ConEliminacion = 0;

            try
            {
                DTExcel _Plantilla = JsonConvert.DeserializeObject<DTExcel>(Plantilla);
                List<DTFilasExcel> filas = _Plantilla.sheets.First().rows;
                if (filas.Count > 1)
                {

                    //Validar el encabezado
                    List<DTColumnasExcel> columnas = filas.First().cells;

                    if (columnas[0].value.ToString().Trim() == DTEstructuraSolped.SolNro
                       && columnas[1].value.ToString().Trim() == DTEstructuraSolped.NroOC
                       && columnas[2].value.ToString().Trim() == DTEstructuraSolped.LineaPedido
                       && columnas[3].value.ToString().Trim() == DTEstructuraSolped.NroArticulos
                       && columnas[4].value.ToString().Trim() == DTEstructuraSolped.NroSPE
                       && columnas[5].value.ToString().Trim() == DTEstructuraSolped.HACAT
                       && columnas[6].value.ToString().Trim() == DTEstructuraSolped.PersonaSolicitud
                       && columnas[7].value.ToString().Trim() == DTEstructuraSolped.FechaEnvio
                       && columnas[8].value.ToString().Trim() == DTEstructuraSolped.Estado
                       && columnas[9].value.ToString().Trim() == DTEstructuraSolped.Tipo
                       && columnas[10].value.ToString().Trim() == DTEstructuraSolped.Articulo
                       && columnas[11].value.ToString().Trim() == DTEstructuraSolped.Cantidad
                       && columnas[12].value.ToString().Trim() == DTEstructuraSolped.TotalLinea
                       && columnas[13].value.ToString().Trim() == DTEstructuraSolped.CodigoIVA
                       && columnas[14].value.ToString().Trim() == DTEstructuraSolped.CuadroCuentas
                       && columnas[15].value.ToString().Trim() == DTEstructuraSolped.PersonaCreador
                       && columnas[16].value.ToString().Trim() == DTEstructuraSolped.FechaPedido
                       && columnas[17].value.ToString().Trim() == DTEstructuraSolped.FechaCreacion
                       && columnas[18].value.ToString().Trim() == DTEstructuraSolped.FechaEntrega
                       && columnas[19].value.ToString().Trim() == DTEstructuraSolped.FechaCaducidad
                       && columnas[20].value.ToString().Trim() == DTEstructuraSolped.Proveedor
                       && columnas[21].value.ToString().Trim() == DTEstructuraSolped.Cuenta
                       && columnas[22].value.ToString().Trim() == DTEstructuraSolped.IdOC
                       && columnas[23].value.ToString().Trim() == DTEstructuraSolped.LeandingCostC
                       && columnas[24].value.ToString().Trim() == DTEstructuraSolped.EstadoLinea
                       && columnas[25].value.ToString().Trim() == DTEstructuraSolped.Divisa
                       )
                    {
                        List<DTFilasExcel> ConInformacion = (from dt in filas
                                                             where dt.cells.Count(x => x.value == null) != dt.cells.Count()
                                                             && dt.index != 0
                                                             select dt).ToList();
                        if (ConInformacion.Count > 0)
                        {
                            BMSolped _BMSolped = new BMSolped();
                            DTResultadoModelCarga = _BMSolped.LeerArchivo(ConInformacion);
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
            DTResultadoOperacionList<DTSolped> ResultadoList = new DTResultadoOperacionList<DTSolped>();
            List<DTSolped> Resultado = new List<DTSolped>();

            try
            {
                Resultado = new BMSolped().ConsultarRegistrosEx();
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
                Resultado = new BMSolped().ConsultarErroresExcel();
                ResultadoList.Resultado = true;
                ResultadoList.Datos = Resultado;
            }
            catch (Exception ex)
            {

                GestorLog.RegistrarLogExcepcion(ex);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //serializer.MaxJsonLength = 500000000;
            return serializer.Serialize(ResultadoList);
        }
        #endregion


        #region Consultar Solped Filtros
        public string ConsultarSolpedFiltros(DTFiltros Dt)
        {
            DTResultadoOperacionList<DTSolped> ResultadoList = new DTResultadoOperacionList<DTSolped>();
            List<DTSolped> Resultado = new List<DTSolped>();

            try
            {
                Resultado = new BMSolped().ConsultarSolpedFiltros(Dt);
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