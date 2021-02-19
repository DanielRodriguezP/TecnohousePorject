using Sanofi.DM.Athena;
using Sanofi.DT.Athena;
using Sanofi.DT.Excel;
using Sanofi.DT.General;
using Sanofi.DT.Mensajes;
using Sanofi.Soporte;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sanofi.BM.Athena
{
    public class BMAthena
    {
        #region Obtener Valores de las celdas de Excel
        private string ObtenerValorExcel(int index, DTFilasExcel filas)
        {
            string Valor = string.Empty;
            Valor = (from dt in filas.cells
                     where dt.index == index
                     select dt.value != null ? dt.value.ToString() : string.Empty).FirstOrDefault();
            return string.IsNullOrEmpty(Valor) ? string.Empty : Valor;
        }
        #endregion

        #region Leer archivo y realizar validaciones 
        public DTResultadoOperacionModel<DTErroresExcel> LeerArchivo(List<DTFilasExcel> fileFactura, string nombreArchivo)
        {
            DTResultadoOperacionModel<DTAthena> _DTResultadoModel = new DTResultadoOperacionModel<DTAthena>();
            DTResultadoOperacionModel<DTErroresExcel> resultado = new DTResultadoOperacionModel<DTErroresExcel>();

            Workbook _Workbook = new Workbook();
            List<DTAthena> ListaCargaAthena = new List<DTAthena>();
            List<DTErroresExcel> ListaCargaIncompleta = new List<DTErroresExcel>();
            try
            {
                List<DTFilasExcel> informacion = (from dt in fileFactura
                                                  where dt.index != 0
                                                  select dt).ToList();

                foreach (var filas in informacion)
                {
                    DTAthena _MMExcel = new DTAthena();
                    string Item = ObtenerValorExcel(0, filas).Replace(Constantes.ValidacionEncabezados, string.Empty).Trim().ToString();
                    string Document = ObtenerValorExcel(1, filas).Replace(Constantes.ValidacionEncabezados, string.Empty).Trim().ToString();
                    string Date = ObtenerValorExcel(2, filas).Replace(Constantes.ValidacionEncabezados, string.Empty).Trim().ToString();
                    string Material = ObtenerValorExcel(3, filas).Replace(Constantes.ValidacionEncabezados, string.Empty).Trim().ToString();
                    string ShortText = ObtenerValorExcel(4, filas).Replace(Constantes.ValidacionEncabezados, string.Empty).Trim().ToString();
                    string OrderQty = ObtenerValorExcel(5, filas).Replace(Constantes.ValidacionEncabezados, string.Empty).Trim().ToString();
                    string DeliveryQty = ObtenerValorExcel(6, filas).Replace(Constantes.ValidacionEncabezados, string.Empty).Trim().ToString();
                    string OrderUnit = ObtenerValorExcel(7, filas).Replace(Constantes.ValidacionEncabezados, string.Empty).Trim().ToString();
                    string NetPrice = ObtenerValorExcel(8, filas).Replace(Constantes.ValidacionEncabezados, string.Empty).Trim().ToString();
                    string NetOrderValue = ObtenerValorExcel(9, filas).Replace(Constantes.ValidacionEncabezados, string.Empty).Trim().ToString();
                    string VendorSuplyingPlant = ObtenerValorExcel(10, filas).Replace(Constantes.ValidacionEncabezados, string.Empty).Trim().ToString();
                    string Currency = ObtenerValorExcel(11, filas).Replace(Constantes.ValidacionEncabezados, string.Empty).Trim().ToString();
                    string ReleaseState = ObtenerValorExcel(12, filas).Replace(Constantes.ValidacionEncabezados, string.Empty).Trim().ToString();
                    string Delete = ObtenerValorExcel(13, filas).Replace(Constantes.ValidacionEncabezados, string.Empty).Trim().ToString();
                    string POHistory = ObtenerValorExcel(14, filas).Replace(Constantes.ValidacionEncabezados, string.Empty).Trim().ToString();

                    Regex Numeros = new Regex(Constantes.ExpRegSoloNumeros);
                    Regex ValidandoEspacios = new Regex(Constantes.ExpRegEspaciosBlanco);
                    Regex NumyLetras = new Regex(Constantes.ExpRegAlfaNumericos);
                    Regex rex = new Regex(Constantes.ExpRegAlfaNumericos);
                    Regex rexNumeroPunto = new Regex(Constantes.ExpRegNumeroPunto);
                    Regex Letras = new Regex(Constantes.ExpRegSoloLetras);

                    //Validación casillas de totales
                    if (string.IsNullOrEmpty(Item) && string.IsNullOrEmpty(Document) && string.IsNullOrEmpty(Date) && string.IsNullOrEmpty(ReleaseState))
                    {
                        continue;
                    }
                    //Validación Item
                    if ( !string.IsNullOrEmpty(Item))
                    {
                        if (!Numeros.IsMatch(Item))
                        {
                            DTErroresExcel _Excel = new DTErroresExcel();
                            _Excel.Mensaje = "El campo no debe llevar caracteres especiales.";
                            _Excel.Fila = filas.index + 1;
                            _Excel.Campo = "Item";
                            _Excel.Valor = Item;
                            ListaCargaIncompleta.Add(_Excel);
                        }
                        else
                        {
                            _MMExcel.Item = Int32.Parse(Item);
                        }
                    }
                    else
                    {
                        DTErroresExcel _Excel = new DTErroresExcel();
                        _Excel.Mensaje = "El campo se encuentra vacío.";
                        _Excel.Fila = filas.index + 1;
                        _Excel.Campo = "Item";
                        _Excel.Valor = Item;
                        ListaCargaIncompleta.Add(_Excel);
                    }

                    //Validación Document
                    if (!string.IsNullOrEmpty(Document))
                    {
                        if (!Numeros.IsMatch(Document))
                        {
                            DTErroresExcel _Excel = new DTErroresExcel();
                            _Excel.Mensaje = "El campo no debe llevar caracteres especiales.";
                            _Excel.Fila = filas.index + 1;
                            _Excel.Campo = "Document";
                            _Excel.Valor = Document;
                            ListaCargaIncompleta.Add(_Excel);
                        }
                        else
                        {
                            _MMExcel.Document = Document;
                        }
                    }
                    else
                    {
                        DTErroresExcel _Excel = new DTErroresExcel();
                        _Excel.Mensaje = "El campo se encuentra vacío.";
                        _Excel.Fila = filas.index + 1;
                        _Excel.Campo = "Document";
                        _Excel.Valor = Document;
                        ListaCargaIncompleta.Add(_Excel);
                    }

                    //Validación Date
                    if (!string.IsNullOrEmpty(Date))
                    {
                        DateTime fecha = new DateTime();
                        try
                        {
                            fecha = DateTime.FromOADate(Convert.ToDouble(Date));

                            _MMExcel.Date = Convert.ToDateTime(fecha);

                        }
                        catch (Exception)
                        {
                            DTErroresExcel _Excel = new DTErroresExcel();
                            _Excel.Mensaje = "El campo no tiene el formato correcto.";
                            _Excel.Fila = filas.index + 1;
                            _Excel.Campo = "Date";
                            _Excel.Valor = Date;
                            ListaCargaIncompleta.Add(_Excel);
                        }
                    }
                    else
                    {
                        DTErroresExcel _Excel = new DTErroresExcel();
                        _Excel.Mensaje = "El campo se encuentra vacío.";
                        _Excel.Fila = filas.index + 1;
                        _Excel.Campo = "Date";
                        _Excel.Valor = Date;
                        ListaCargaIncompleta.Add(_Excel);
                    }

                    //Validación Material
                    if (!string.IsNullOrEmpty(Material))
                    {
                        if (!NumyLetras.IsMatch(Material))
                        {
                            DTErroresExcel _Excel = new DTErroresExcel();
                            _Excel.Mensaje = "El campo no debe llevar caracteres especiales.";
                            _Excel.Fila = filas.index + 1;
                            _Excel.Campo = "Material";
                            _Excel.Valor = Material;
                            ListaCargaIncompleta.Add(_Excel);
                        }
                        else
                        {
                            _MMExcel.Material = Material;
                        }
                    }
                    else
                    {
                        DTErroresExcel _Excel = new DTErroresExcel();
                        _Excel.Mensaje = "El campo se encuentra vacío.";
                        _Excel.Fila = filas.index + 1;
                        _Excel.Campo = "Material";
                        _Excel.Valor = Material;
                        ListaCargaIncompleta.Add(_Excel);
                    }

                    //Validación ShortText
                    if (!string.IsNullOrEmpty(ShortText))
                    {
                        _MMExcel.ShortText = ShortText;
                    }
                    else
                    {
                        DTErroresExcel _Excel = new DTErroresExcel();
                        _Excel.Mensaje = "El campo se encuentra vacío.";
                        _Excel.Fila = filas.index + 1;
                        _Excel.Campo = "Short Text";
                        _Excel.Valor = ShortText;
                        ListaCargaIncompleta.Add(_Excel);
                    }

                    //Validación OrderQty
                    if (!string.IsNullOrEmpty(OrderQty))
                    {
                        if (!rexNumeroPunto.IsMatch(OrderQty))
                        {
                            DTErroresExcel _Excel = new DTErroresExcel();
                            _Excel.Mensaje = "El campo no debe llevar caracteres especiales.";
                            _Excel.Fila = filas.index + 1;
                            _Excel.Campo = "Order Quantity";
                            _Excel.Valor = OrderQty;
                            ListaCargaIncompleta.Add(_Excel);
                        }
                        else
                        {
                            _MMExcel.OrderQty = Decimal.Parse(OrderQty);
                        }
                    }
                    else
                    {
                        DTErroresExcel _Excel = new DTErroresExcel();
                        _Excel.Mensaje = "El campo se encuentra vacío.";
                        _Excel.Fila = filas.index + 1;
                        _Excel.Campo = "Order Quantity";
                        _Excel.Valor = OrderQty;
                        ListaCargaIncompleta.Add(_Excel);
                    }

                    //Validación DeliveryQty
                    if (!string.IsNullOrEmpty(DeliveryQty))
                    {
                        if (!rexNumeroPunto.IsMatch(DeliveryQty))
                        {
                            DTErroresExcel _Excel = new DTErroresExcel();
                            _Excel.Mensaje = "El campo no debe llevar caracteres especiales.";
                            _Excel.Fila = filas.index + 1;
                            _Excel.Campo = "Still to be delivered (qty)";
                            _Excel.Valor = DeliveryQty;
                            ListaCargaIncompleta.Add(_Excel);
                        }
                        else
                        {
                            _MMExcel.DeliveryQty = decimal.Parse(DeliveryQty);
                        }
                    }
                    else
                    {
                        DTErroresExcel _Excel = new DTErroresExcel();
                        _Excel.Mensaje = "El campo se encuentra vacío.";
                        _Excel.Fila = filas.index + 1;
                        _Excel.Campo = "Still to be delivered (qty)";
                        _Excel.Valor = DeliveryQty;
                        ListaCargaIncompleta.Add(_Excel);
                    }

                    //Validación OrderUnit
                    if (!string.IsNullOrEmpty(OrderUnit))
                    {
                        if (!Letras.IsMatch(OrderUnit))
                        {
                            DTErroresExcel _Excel = new DTErroresExcel();
                            _Excel.Mensaje = "El campo no debe llevar caracteres especiales.";
                            _Excel.Fila = filas.index + 1;
                            _Excel.Campo = "Order Unit";
                            _Excel.Valor = OrderUnit;
                            ListaCargaIncompleta.Add(_Excel);
                        }
                        else
                        {
                            _MMExcel.OrderUnit = OrderUnit;
                        }
                    }
                    else
                    {
                        DTErroresExcel _Excel = new DTErroresExcel();
                        _Excel.Mensaje = "El campo se encuentra vacío.";
                        _Excel.Fila = filas.index + 1;
                        _Excel.Campo = "Order Unit";
                        _Excel.Valor = OrderUnit;
                        ListaCargaIncompleta.Add(_Excel);
                    }

                    //Validación NetPrice
                    if (!string.IsNullOrEmpty(NetPrice))
                    {
                        if (!rexNumeroPunto.IsMatch(NetPrice))
                        {
                            DTErroresExcel _Excel = new DTErroresExcel();
                            _Excel.Mensaje = "El campo no debe llevar caracteres especiales.";
                            _Excel.Fila = filas.index + 1;
                            _Excel.Campo = "Net Price";
                            _Excel.Valor = NetPrice;
                            ListaCargaIncompleta.Add(_Excel);
                        }
                        else
                        {
                            _MMExcel.NetPrice = decimal.Parse(NetPrice);
                        }
                    }
                    else
                    {
                        DTErroresExcel _Excel = new DTErroresExcel();
                        _Excel.Mensaje = "El campo se encuentra vacío.";
                        _Excel.Fila = filas.index + 1;
                        _Excel.Campo = "Net Price";
                        _Excel.Valor = NetPrice;
                        ListaCargaIncompleta.Add(_Excel);
                    }

                    //Validación NetOrderValue
                    if (!string.IsNullOrEmpty(NetOrderValue))
                    {
                        if (!rexNumeroPunto.IsMatch(NetOrderValue))
                        {
                            DTErroresExcel _Excel = new DTErroresExcel();
                            _Excel.Mensaje = "El campo no debe llevar caracteres especiales.";
                            _Excel.Fila = filas.index + 1;
                            _Excel.Campo = "Net Order Value";
                            _Excel.Valor = NetOrderValue;
                            ListaCargaIncompleta.Add(_Excel);
                        }
                        else
                        {
                            _MMExcel.NetOrderValue = decimal.Parse(NetOrderValue);
                        }
                    }
                    else
                    {
                        DTErroresExcel _Excel = new DTErroresExcel();
                        _Excel.Mensaje = "El campo se encuentra vacío.";
                        _Excel.Fila = filas.index + 1;
                        _Excel.Campo = "Net Order Value";
                        _Excel.Valor = NetOrderValue;
                        ListaCargaIncompleta.Add(_Excel);
                    }

                    //Validación VendorSuplyingPlant
                    if (!string.IsNullOrEmpty(VendorSuplyingPlant))
                    {
                        _MMExcel.Vendor = VendorSuplyingPlant;
                    }
                    else
                    {
                        DTErroresExcel _Excel = new DTErroresExcel();
                        _Excel.Mensaje = "El campo se encuentra vacío.";
                        _Excel.Fila = filas.index + 1;
                        _Excel.Campo = "Vendor/supplying plant";
                        _Excel.Valor = VendorSuplyingPlant;
                        ListaCargaIncompleta.Add(_Excel);
                    }

                    //Validación Currency
                    if (!string.IsNullOrEmpty(Currency))
                    {
                        if (!Letras.IsMatch(Currency))
                        {
                            DTErroresExcel _Excel = new DTErroresExcel();
                            _Excel.Mensaje = "El campo no debe llevar caracteres especiales.";
                            _Excel.Fila = filas.index + 1;
                            _Excel.Campo = "Currency";
                            _Excel.Valor = Currency;
                            ListaCargaIncompleta.Add(_Excel);
                        }
                        else
                        {
                            _MMExcel.Currency = Currency;
                        }
                    }
                    else
                    {
                        DTErroresExcel _Excel = new DTErroresExcel();
                        _Excel.Mensaje = "El campo se encuentra vacío.";
                        _Excel.Fila = filas.index + 1;
                        _Excel.Campo = "Currency";
                        _Excel.Valor = Currency;
                        ListaCargaIncompleta.Add(_Excel);
                    }

                    //Validación ReleaseState
                    if (!string.IsNullOrEmpty(ReleaseState))
                    {
                        //if (!Numeros.IsMatch(ReleaseState))
                        //{
                        //    DTErroresExcel _Excel = new DTErroresExcel();
                        //    _Excel.Mensaje = "El campo no debe llevar caracteres especiales.";
                        //    _Excel.Fila = filas.index + 1;
                        //    _Excel.Campo = "Item";
                        //    _Excel.Valor = Item;
                        //    ListaCargaIncompleta.Add(_Excel);
                        //}
                        //else
                        //{
                            _MMExcel.ReleaseState = ReleaseState;
                        //}
                    }
                    else
                    {
                        DTErroresExcel _Excel = new DTErroresExcel();
                        _Excel.Mensaje = "El campo se encuentra vacío.";
                        _Excel.Fila = filas.index + 1;
                        _Excel.Campo = "Release State";
                        _Excel.Valor = ReleaseState;
                        ListaCargaIncompleta.Add(_Excel);
                    }

                    //Validación Delete
                    if (!string.IsNullOrEmpty(Delete))
                    {
                        if (Delete == "1" || Delete == "0")
                        {
                            _MMExcel.Delete = Convert.ToBoolean(Convert.ToInt32(Delete));
                        }
                        else
                        {
                            DTErroresExcel _Excel1 = new DTErroresExcel();
                            _Excel1.Mensaje = "El dato no coincide con los parámetros establecidos.";
                            _Excel1.Fila = filas.index + 1;
                            _Excel1.Campo = "Delete";
                            _Excel1.Valor = Delete;
                            ListaCargaIncompleta.Add(_Excel1);
                        }
                    }
                    else
                    {
                        _MMExcel.Delete = false;

                    }

                    //Validación POHistory
                    _MMExcel.POHistory = POHistory;

                    _MMExcel.Fila = filas.index + 1;
                    //Registros Duplicados

                    List<DTAthena> ExisteRegistroDuplicados = (from dt in ListaCargaAthena
                                                                                 where dt.Document == _MMExcel.Document && dt.Date == _MMExcel.Date && dt.Item == _MMExcel.Item
                                                                                 select dt).ToList();

                    //Validación Registros Duplicados
                    if (ExisteRegistroDuplicados.Count > 0)
                    {
                        DTErroresExcel _Excel1 = new DTErroresExcel();
                        _Excel1.Mensaje = "Se repiten registros.";
                        _Excel1.Fila = filas.index + 1;
                        _Excel1.Campo = "Item,Purchasing Document, Document Date";
                        _Excel1.Valor = _MMExcel.Item.ToString()+","+ _MMExcel.Document.ToString() + ", " + _MMExcel.Date.ToShortDateString();
                        ListaCargaIncompleta.Add(_Excel1);

                        foreach (var item in ExisteRegistroDuplicados)
                        {
                            DTErroresExcel _Excel = new DTErroresExcel();
                            _Excel.Mensaje = "Se repiten registros.";
                            _Excel.Fila = item.Fila;
                            _Excel.Campo = "Item, Purchasing Document, Document Date";
                            _Excel.Valor = _MMExcel.Item.ToString() + "," + _MMExcel.Document.ToString() + ", " + _MMExcel.Date.ToShortDateString();
                            ListaCargaIncompleta.Add(_Excel);
                            ListaCargaAthena.RemoveAll(c => c.Fila == item.Fila);
                        }
                    }


                    // consultar si la fila no tiene inconvenientes
                    List<DTErroresExcel> ConInformacion = (from dt in ListaCargaIncompleta
                                                           where dt.Fila == (filas.index + 1)
                                                           select dt).ToList();

                    // si no prensentar inconveniente se agrega a la lista de carga bd.
                    if (ConInformacion.Count > 0)
                    {

                    }
                    else
                    {
                        ListaCargaAthena.Add(_MMExcel);
                    }
                }

                // inserta los exitoso o setea en 0 los estados
                ConvertToDataTable(ListaCargaAthena);
                

                if (ListaCargaIncompleta.Count > 0)
                {
                    ConvertToXML(ListaCargaIncompleta);
                }
                _DTResultadoModel.Respuesta = true;
                resultado.Respuesta = _DTResultadoModel.Respuesta;
            }
            catch (Exception ex)
            {
                _DTResultadoModel.Mensaje = DTMensaje.ObtenerObjetoMensaje(DTCodigoMensajes.MENSAJE008);
                _DTResultadoModel.Respuesta = false;
                GestorLog.RegistrarLogExcepcion(ex);

                resultado.Respuesta = _DTResultadoModel.Respuesta;
                resultado.Mensaje = _DTResultadoModel.Mensaje;
            }
            return resultado;
        }
        #endregion

        #region Convertir información .xlsx a xml
        public DTResultadoOperacionModel<DTErroresExcel> ConvertToDataTable(List<DTAthena> ListaCargaAthena)
        {
            DTResultadoOperacionModel<DTErroresExcel> resultado = new DTResultadoOperacionModel<DTErroresExcel>();
            DTResultadoOperacionModel<DTAthena> _resultado = new DTResultadoOperacionModel<DTAthena>();
            string Xml = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Item");
                dt.Columns.Add("Document");
                dt.Columns.Add("Date");
                dt.Columns.Add("Material");
                dt.Columns.Add("ShortText");
                dt.Columns.Add("OrderQty");
                dt.Columns.Add("DeliveryQty");
                dt.Columns.Add("OrderUnit");
                dt.Columns.Add("NetPrice");
                dt.Columns.Add("NetOrderValue");
                dt.Columns.Add("Vendor");
                dt.Columns.Add("Currency");
                dt.Columns.Add("ReleaseState");
                dt.Columns.Add("Delete");
                dt.Columns.Add("POHistory");
                foreach (var user in ListaCargaAthena)
                {
                    var newRow = dt.NewRow();
                    newRow["Item"] = user.Item;
                    newRow["Document"] = user.Document;
                    newRow["Date"] = user.Date.ToString("dd-MM-yyyy");
                    newRow["Material"] = user.Material;
                    newRow["ShortText"] = user.ShortText;
                    newRow["OrderQty"] = user.OrderQty;
                    newRow["DeliveryQty"] = user.DeliveryQty;
                    newRow["OrderUnit"] = user.OrderUnit;
                    newRow["NetPrice"] = user.NetPrice;
                    newRow["NetOrderValue"] = user.NetOrderValue;
                    newRow["Vendor"] = user.Vendor;
                    newRow["Currency"] = user.Currency;
                    newRow["ReleaseState"] = user.ReleaseState;
                    newRow["Delete"] = user.Delete;
                    newRow["POHistory"] = user.POHistory;
                    dt.Rows.Add(newRow);
                }

                DataSet dsBuildSQL = new DataSet();
                StringBuilder sbSQL;
                StringWriter swSQL;

                sbSQL = new StringBuilder();
                swSQL = new StringWriter(sbSQL);
                string XMLformat;

                dsBuildSQL.Merge(dt, true, MissingSchemaAction.AddWithKey);
                dsBuildSQL.Tables[0].TableName = "ArchivoXML";

                foreach (DataColumn col in dsBuildSQL.Tables[0].Columns)
                {
                    col.ColumnMapping = MappingType.Attribute;
                }

                dsBuildSQL.WriteXml(swSQL, XmlWriteMode.WriteSchema);
                XMLformat = sbSQL.ToString();

                _resultado = new DMAthena().InsertarAthena(XMLformat);
            }
            catch (Exception ex)
            {
                throw;
            }
            return resultado;
        }
        #endregion

        #region Convertir Errores Excel a XML
        public DTResultadoOperacionModel<DTErroresExcel> ConvertToXML(List<DTErroresExcel> ListaCargaErrores)
        {
            DTResultadoOperacionModel<DTErroresExcel> resultado = new DTResultadoOperacionModel<DTErroresExcel>();

            string Xml = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Mensaje");
                dt.Columns.Add("Campo");
                dt.Columns.Add("Fila");
                dt.Columns.Add("IdModalidad");
                dt.Columns.Add("Valor");
                foreach (var Error in ListaCargaErrores)
                {
                    var newRow = dt.NewRow();
                    newRow["Mensaje"] = Error.Mensaje;
                    newRow["Campo"] = Error.Campo;
                    newRow["Fila"] = Error.Fila;
                    newRow["IdModalidad"] = Constantes.IdModAthena;
                    newRow["Valor"] = Error.Valor;
                    dt.Rows.Add(newRow);
                }

                DataSet dsBuildSQL = new DataSet();
                StringBuilder sbSQL;
                StringWriter swSQL;

                sbSQL = new StringBuilder();
                swSQL = new StringWriter(sbSQL);
                string XMLformat;

                dsBuildSQL.Merge(dt, true, MissingSchemaAction.AddWithKey);
                dsBuildSQL.Tables[0].TableName = "ArchivoXML";

                foreach (DataColumn col in dsBuildSQL.Tables[0].Columns)
                {
                    col.ColumnMapping = MappingType.Attribute;
                }

                dsBuildSQL.WriteXml(swSQL, XmlWriteMode.WriteSchema);
                XMLformat = sbSQL.ToString();

                resultado = new DMAthena().InsertarErroresExcel(XMLformat);
            }
            catch (Exception ex)
            {

                throw;
            }
            return resultado;
        }

        #endregion

        #region Consultar Registros Exitosos
        public List<DTAthena> ConsultarRegistrosEx()
        {
            try
            {
                return new DMAthena().ConsultaRegistroExitoso();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Consulta Errores Excel
        public List<DTErroresExcel> ConsultarErroresExcel()
        {
            try
            {
                return new DMAthena().ConsultarErroresExcel();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        #endregion

        #region Consulta Filtros fechas eventos
        //public List<DTAthena> ConsultarEventosFiltros(DTAthena Dt)
        //{
        //    try
        //    {
        //        return new DMInscripcionesYMatriculas().ConsultarEventosIyM(Dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        GestorLog.RegistrarLogExcepcion(ex);
        //        throw ex;

        //    }
        //}
        #endregion

        #region Consulta Athena Filtros
        public List<DTAthena> ConsultarAthenaFiltros(DTFiltros Dt)
        {
            try
            {
                return new DMAthena().ConsultarAthenaFiltros(Dt);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        #endregion

        #region Consulta Reporte
        public List<DTAthena> ConsultarReporte()
        {
            try
            {
                return new DMAthena().ConsultarReporte();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        #endregion
    }
}
