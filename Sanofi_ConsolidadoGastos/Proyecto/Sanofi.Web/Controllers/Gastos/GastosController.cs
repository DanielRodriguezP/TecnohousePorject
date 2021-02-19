namespace Sanofi.Web.Controllers.Gastos
{
    using Newtonsoft.Json;
    using Sanofi.BM.Gastos;
    using Sanofi.BM.Solped;
    using Sanofi.DT.Excel;
    using Sanofi.DT.Gastos;
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
    public class GastosController : Controller
    {
        // GET: InscripcionesYMatriculas
        public ActionResult IndexGastos()
        {
            return View();
        }

        public ActionResult ConsultaGastos()
        {
            return View();
        }

        public ActionResult ReporteGastos()
        {
            return View();
        }

        #region Descargar Plantilla de Excel Gastos
        public ActionResult DescargarPlantilla()
        {
            string RutaPlantilla = ConfigurationManager.AppSettings.Get("PlantillaGastos");
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
            string RutaArchivo = string.Empty;
            DTResultadoOperacionModel<DTSolped> DTResultadoModel = new DTResultadoOperacionModel<DTSolped>();
            DTResultadoOperacionModel<DTErroresExcel> DTResultadoModelCarga = new DTResultadoOperacionModel<DTErroresExcel>();
            //int ConEliminacion = 0;

            try
            {
                DTExcel _Plantilla = JsonConvert.DeserializeObject<DTExcel>(Plantilla);
                List<DTFilasExcel> filas = _Plantilla.sheets.First().rows;
                string MesGastos = _Plantilla.sheets.First().name;
                bool FechaGasto = ValidarFechaGasto(MesGastos);

                if (filas.Count > 1)
                {

                    //Validar el encabezado
                    List<DTColumnasExcel> columnas = filas.First().cells;

                    if (columnas[0].value.ToString().Trim() ==   DTEstructuraGastos.NomCuenta
                       && columnas[1].value.ToString().Trim() == DTEstructuraGastos.NomCentroC
                       && columnas[2].value.ToString().Trim() == DTEstructuraGastos.NroCuenta
                       && columnas[3].value.ToString().Trim() == DTEstructuraGastos.CentroCoste
                       && columnas[4].value.ToString().Trim() == DTEstructuraGastos.Concatenar
                       && columnas[5].value.ToString().Trim() == DTEstructuraGastos.Tipo
                       && columnas[6].value.ToString().Trim() == DTEstructuraGastos.Mes
                       && columnas[7].value.ToString().Trim() == DTEstructuraGastos.Ajustes
                       && columnas[8].value.ToString().Trim() == DTEstructuraGastos.TotalMes
                       )
                    {
                        List<DTFilasExcel> ConInformacion = (from dt in filas
                                                             where dt.cells.Count(x => x.value == null) != dt.cells.Count()
                                                             && dt.index != 0
                                                             select dt).ToList();
                        if (ConInformacion.Count > 0)
                        {
                            if (FechaGasto)
                            {
                                string MesGastoReal = "01-" + MesGastos;
                                BMGastos _BMGastos = new BMGastos();
                                DTResultadoModelCarga = _BMGastos.LeerArchivo(ConInformacion, MesGastoReal);
                            }
                            else
                            {
                                DTResultadoModelCarga.Respuesta = false;
                                DTResultadoModelCarga.Mensaje = DTMensaje.ObtenerObjetoMensaje(DTCodigoMensajes.MENSAJE022);
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
            DTResultadoOperacionList<DTGastos> ResultadoList = new DTResultadoOperacionList<DTGastos>();
            List<DTGastos> Resultado = new List<DTGastos>();

            try
            {
                Resultado = new BMGastos().ConsultarRegistrosEx();
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
                Resultado = new BMGastos().ConsultarErroresExcel();
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


        #region Consultar Gastos Filtros
        public string ConsultarGastosFiltros(DTFiltros Dt)
        {
            DTResultadoOperacionList<DTGastos> ResultadoList = new DTResultadoOperacionList<DTGastos>();
            List<DTGastos> Resultado = new List<DTGastos>();

            try
            {
                Resultado = new BMGastos().ConsultarGastosFiltros(Dt);
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

        #region Consultar Cuentas
        public string ConsultarCuentas(DTFiltros Dt)
        {
            DTResultadoOperacionList<DTObjetosFiltros> ResultadoList = new DTResultadoOperacionList<DTObjetosFiltros>();
            List<DTObjetosFiltros> Resultado = new List<DTObjetosFiltros>();

            try
            {
                Resultado = new BMGastos().ConsultarCuentas(Dt);
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

        #region Consultar Tipos
        public string ConsultarTipos(DTFiltros Dt)
        {
            DTResultadoOperacionList<DTObjetosFiltros> ResultadoList = new DTResultadoOperacionList<DTObjetosFiltros>();
            List<DTObjetosFiltros> Resultado = new List<DTObjetosFiltros>();

            try
            {
                Resultado = new BMGastos().ConsultarTipos(Dt);
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

        #region Consultar Concatenar
        public string ConsultarConcatenar(DTFiltros Dt)
        {
            DTResultadoOperacionList<DTObjetosFiltros> ResultadoList = new DTResultadoOperacionList<DTObjetosFiltros>();
            List<DTObjetosFiltros> Resultado = new List<DTObjetosFiltros>();

            try
            {
                Resultado = new BMGastos().ConsultarConcatenar(Dt);
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

        bool ValidarFechaGasto(string fecha)
        {
            bool Respuesta = false;
            try
            {
                string[] mesAnio = fecha.Split('-');
                int mes = Convert.ToInt32(mesAnio.First());
                int anio = Convert.ToInt32(mesAnio[1]);
                if (mes > 0 && mes < 13)
                {
                    Respuesta = true;
                }
                return Respuesta;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

    }
}