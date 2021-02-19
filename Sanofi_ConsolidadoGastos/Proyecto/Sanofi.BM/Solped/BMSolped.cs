namespace Sanofi.BM.Solped
{
    using Sanofi.DM.Solped;
    using Sanofi.DT.Excel;
    using Sanofi.DT.General;
    using Sanofi.DT.Mensajes;
    using Sanofi.DT.Solped;
    using Sanofi.Soporte;
    using Sanofi.Soporte.Validaciones;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class BMSolped
    {
        
        #region Leer archivo y realizar validaciones 
        public DTResultadoOperacionModel<DTErroresExcel> LeerArchivo(List<DTFilasExcel> fileFactura)
        {
            DTResultadoOperacionModel<DTSolped> _DTResultadoModel = new DTResultadoOperacionModel<DTSolped>();
            DTResultadoOperacionModel<DTErroresExcel> resultado = new DTResultadoOperacionModel<DTErroresExcel>();

            List<DTSolped> ListaCargaSolped = new List<DTSolped>();
            List<DTErroresExcel> ListaCargaIncompleta = new List<DTErroresExcel>();
            try
            {
                List<DTFilasExcel> informacion = (from dt in fileFactura
                                                  where dt.index != 0
                                                  select dt).ToList();

                foreach (var filas in informacion)
                {
                    DTSolped _MMExcel = new DTSolped();
                    DTFilaValidaciones[] Fila = new DTFilaValidaciones[26];
                    Fila[0] = new DTFilaValidaciones() { Nombre = DTEstructuraSolped.SolNro, Valor = SUPValidaciones.ObtenerValorExcel(0, filas), Validaciones = 1};
                    Fila[1] = new DTFilaValidaciones() { Nombre = DTEstructuraSolped.NroOC, Valor = SUPValidaciones.ObtenerValorExcel(1, filas), Validaciones = 0 };
                    Fila[2] = new DTFilaValidaciones() { Nombre = DTEstructuraSolped.LineaPedido, Valor = SUPValidaciones.ObtenerValorExcel(2, filas), Validaciones = 5 };
                    Fila[3] = new DTFilaValidaciones() { Nombre = DTEstructuraSolped.NroArticulos, Valor = SUPValidaciones.ObtenerValorExcel(3, filas), Validaciones = 1 };
                    Fila[4] = new DTFilaValidaciones() { Nombre = DTEstructuraSolped.NroSPE, Valor = SUPValidaciones.ObtenerValorExcel(4, filas), Validaciones = 0 };
                    Fila[5] = new DTFilaValidaciones() { Nombre = DTEstructuraSolped.HACAT, Valor = SUPValidaciones.ObtenerValorExcel(5, filas), Validaciones = 6 };
                    Fila[6] = new DTFilaValidaciones() { Nombre = DTEstructuraSolped.PersonaSolicitud, Valor = SUPValidaciones.ObtenerValorExcel(6, filas), Validaciones = 6 };
                    Fila[7] = new DTFilaValidaciones() { Nombre = DTEstructuraSolped.FechaEnvio, Valor = SUPValidaciones.ObtenerValorExcel(7, filas), Validaciones = 4 };
                    Fila[8] = new DTFilaValidaciones() { Nombre = DTEstructuraSolped.Estado, Valor = SUPValidaciones.ObtenerValorExcel(8, filas), Validaciones = 6 };
                    Fila[9] = new DTFilaValidaciones() { Nombre = DTEstructuraSolped.Tipo, Valor = SUPValidaciones.ObtenerValorExcel(9, filas), Validaciones = 6 };
                    Fila[10] = new DTFilaValidaciones (){ Nombre = DTEstructuraSolped.Articulo, Valor = SUPValidaciones.ObtenerValorExcel(10, filas), Validaciones = 0 };
                    Fila[11] = new DTFilaValidaciones (){ Nombre = DTEstructuraSolped.Cantidad, Valor = SUPValidaciones.ObtenerValorExcel(11, filas), Validaciones = 3 };
                    Fila[12] = new DTFilaValidaciones (){ Nombre = DTEstructuraSolped.TotalLinea, Valor = SUPValidaciones.ObtenerValorExcel(12, filas), Validaciones = 3 };
                    Fila[13] = new DTFilaValidaciones (){ Nombre = DTEstructuraSolped.CodigoIVA, Valor = SUPValidaciones.ObtenerValorExcel(13, filas), Validaciones = 0 };
                    Fila[14] = new DTFilaValidaciones (){ Nombre = DTEstructuraSolped.CuadroCuentas, Valor = SUPValidaciones.ObtenerValorExcel(14, filas), Validaciones = 6 };
                    Fila[15] = new DTFilaValidaciones (){ Nombre = DTEstructuraSolped.PersonaCreador, Valor = SUPValidaciones.ObtenerValorExcel(15, filas), Validaciones = 6 };
                    Fila[16] = new DTFilaValidaciones (){ Nombre = DTEstructuraSolped.FechaPedido, Valor = SUPValidaciones.ObtenerValorExcel(16, filas), Validaciones = 4 };
                    Fila[17] = new DTFilaValidaciones (){ Nombre = DTEstructuraSolped.FechaCreacion, Valor = SUPValidaciones.ObtenerValorExcel(17, filas), Validaciones = 2 };
                    Fila[18] = new DTFilaValidaciones (){ Nombre = DTEstructuraSolped.FechaEntrega, Valor = SUPValidaciones.ObtenerValorExcel(18, filas), Validaciones = 4 };
                    Fila[19] = new DTFilaValidaciones (){ Nombre = DTEstructuraSolped.FechaCaducidad, Valor = SUPValidaciones.ObtenerValorExcel(19, filas), Validaciones = 4 };
                    Fila[20] = new DTFilaValidaciones (){ Nombre = DTEstructuraSolped.Proveedor, Valor = SUPValidaciones.ObtenerValorExcel(20, filas), Validaciones = 6 };
                    Fila[21] = new DTFilaValidaciones (){ Nombre = DTEstructuraSolped.Cuenta, Valor = SUPValidaciones.ObtenerValorExcel(21, filas), Validaciones = 0 };
                    Fila[22] = new DTFilaValidaciones (){ Nombre = DTEstructuraSolped.IdOC, Valor = SUPValidaciones.ObtenerValorExcel(22, filas), Validaciones = 5 };
                    Fila[23] = new DTFilaValidaciones (){ Nombre = DTEstructuraSolped.LeandingCostC, Valor = SUPValidaciones.ObtenerValorExcel(23, filas), Validaciones = 6 };
                    Fila[24] = new DTFilaValidaciones (){ Nombre = DTEstructuraSolped.EstadoLinea, Valor = SUPValidaciones.ObtenerValorExcel(24, filas), Validaciones = 0 };
                    Fila[25] = new DTFilaValidaciones (){ Nombre = DTEstructuraSolped.Divisa, Valor = SUPValidaciones.ObtenerValorExcel(25, filas), Validaciones = 6 };

                    foreach(DTFilaValidaciones valores in Fila)
                    {
                        DTErroresExcel Errores = SUPValidaciones.Validacion(valores,filas.index);
                        if(Errores.Mensaje != string.Empty)
                        {
                            ListaCargaIncompleta.Add(Errores);
                        }
                        else
                        {
                            switch (valores.Nombre)
                            {
                                case DTEstructuraSolped.SolNro:
                                    _MMExcel.SolNro = Int32.Parse(valores.Valor);
                                    break;
                                case DTEstructuraSolped.NroOC:
                                        _MMExcel.NroOC = valores.Valor;
                                    break;
                                case DTEstructuraSolped.LineaPedido:
                                    if (!string.IsNullOrEmpty(valores.Valor))
                                    {
                                        _MMExcel.LineaPedido = Int32.Parse(valores.Valor);
                                    }
                                    else
                                    {
                                        _MMExcel.NroOC = null;
                                    }
                                    break;
                                case DTEstructuraSolped.NroArticulos:
                                    _MMExcel.NroArticulos = Int32.Parse(valores.Valor);
                                    break;
                                case DTEstructuraSolped.NroSPE:
                                    _MMExcel.NroSPE = valores.Valor;
                                    break;
                                case DTEstructuraSolped.HACAT:
                                    _MMExcel.HACAT = valores.Valor;
                                    break;
                                case DTEstructuraSolped.PersonaSolicitud:
                                    _MMExcel.PersonaSolicitud = valores.Valor;
                                    break;
                                case DTEstructuraSolped.FechaEnvio:
                                    _MMExcel.FechaEnvio = Errores.Fecha;
                                    break;
                                case DTEstructuraSolped.Estado:
                                    _MMExcel.Estado = valores.Valor;
                                    break;
                                case DTEstructuraSolped.Tipo:
                                    _MMExcel.Tipo = valores.Valor;
                                    break;
                                case DTEstructuraSolped.Articulo:
                                    _MMExcel.Articulo = valores.Valor;
                                    break;
                                case DTEstructuraSolped.Cantidad:
                                    _MMExcel.Cantidad = decimal.Parse(valores.Valor);
                                    break;
                                case DTEstructuraSolped.TotalLinea:
                                    string valor = valores.Valor.Replace('.', ',');
                                    _MMExcel.TotalLinea = decimal.Parse(valor);
                                    break;
                                case DTEstructuraSolped.CodigoIVA:
                                    _MMExcel.CodigoIVA = valores.Valor;
                                    break;
                                case DTEstructuraSolped.CuadroCuentas:
                                    _MMExcel.CuadroCuentas = valores.Valor;
                                    break;
                                case DTEstructuraSolped.PersonaCreador:
                                    _MMExcel.PersonaCreador = valores.Valor;
                                    break;
                                case DTEstructuraSolped.FechaPedido:
                                    _MMExcel.FechaPedido = Errores.Fecha;
                                    break;
                                case DTEstructuraSolped.FechaCreacion:
                                    _MMExcel.FechaCreacion = (DateTime)Errores.Fecha;
                                    break;
                                case DTEstructuraSolped.FechaEntrega:
                                    _MMExcel.FechaEntrega = Errores.Fecha;
                                    break;
                                case DTEstructuraSolped.FechaCaducidad:
                                    _MMExcel.FechaCaducidad = Errores.Fecha;
                                    break;
                                case DTEstructuraSolped.Proveedor:
                                    _MMExcel.Proveedor = valores.Valor;
                                    break;
                                case DTEstructuraSolped.Cuenta:
                                    _MMExcel.Cuenta = valores.Valor;
                                    break;
                                case DTEstructuraSolped.IdOC:
                                    if (!string.IsNullOrEmpty(valores.Valor))
                                    {
                                        _MMExcel.IdOC = Int32.Parse(valores.Valor);
                                    }
                                    else
                                    {
                                        _MMExcel.NroOC = null;
                                    }
                                    break;
                                case DTEstructuraSolped.LeandingCostC:
                                    _MMExcel.LeandingCostC = valores.Valor;
                                    break;
                                case DTEstructuraSolped.EstadoLinea:
                                    _MMExcel.EstadoLinea = valores.Valor;
                                    break;
                                case DTEstructuraSolped.Divisa:
                                    _MMExcel.Divisa = valores.Valor;
                                    break;
                            }
                        }
                    }

                    _MMExcel.Fila = filas.index + 1;

                    //Registros Duplicados
                    List<DTSolped> ExisteRegistroDuplicados = (from dt in ListaCargaSolped
                                                               where dt.SolNro == _MMExcel.SolNro && dt.NroArticulos == _MMExcel.NroArticulos
                                                               select dt).ToList();

                    //Validación Registros Duplicados
                    if (ExisteRegistroDuplicados.Count > 0)
                    {
                        DTErroresExcel _Excel1 = new DTErroresExcel();
                        _Excel1.Mensaje = "Se repiten registros.";
                        _Excel1.Fila = filas.index + 1;
                        _Excel1.Campo = DTEstructuraSolped.SolNro + ", " + DTEstructuraSolped.NroArticulos;
                        _Excel1.Valor = _MMExcel.SolNro.ToString() + ", " + _MMExcel.NroArticulos.ToString();
                        ListaCargaIncompleta.Add(_Excel1);

                        foreach (var item in ExisteRegistroDuplicados)
                        {
                            DTErroresExcel _Excel = new DTErroresExcel();
                            _Excel.Mensaje = "Se repiten registros.";
                            _Excel.Fila = item.Fila;
                            _Excel.Campo = DTEstructuraSolped.SolNro+", "+ DTEstructuraSolped.NroArticulos;
                            _Excel.Valor = _MMExcel.SolNro.ToString() + ", " + _MMExcel.NroArticulos.ToString();
                            ListaCargaIncompleta.Add(_Excel);
                            ListaCargaSolped.RemoveAll(c => c.Fila == item.Fila);
                        }
                    }


                    // consultar si la fila no tiene inconvenientes
                    List<DTErroresExcel> ConInformacion = (from dt in ListaCargaIncompleta
                                                           where dt.Fila == (filas.index + 1)
                                                           select dt).ToList();

                    // si no prensentar inconveniente se agrega a la lista de carga bd.
                    if (ConInformacion.Count == 0)
                    {
                        ListaCargaSolped.Add(_MMExcel);
                    }
                }

                //// inserta los exitoso o setea en 0 los estados
                ConvertToDataTable(ListaCargaSolped);

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
        public void ConvertToDataTable(List<DTSolped> ListaCarga)
        {
            string Xml = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("SolNro");
                dt.Columns.Add("NroOC");
                dt.Columns.Add("LineaPedido");
                dt.Columns.Add("NroArticulos");
                dt.Columns.Add("NroSPE");
                dt.Columns.Add("HACAT");
                dt.Columns.Add("PersonaSolicitud");
                dt.Columns.Add("FechaEnvio");
                dt.Columns.Add("Estado");
                dt.Columns.Add("Tipo");
                dt.Columns.Add("Articulo");
                dt.Columns.Add("Cantidad");
                dt.Columns.Add("TotalLinea");
                dt.Columns.Add("CodigoIVA");
                dt.Columns.Add("CuadroCuentas");
                dt.Columns.Add("PersonaCreador");
                dt.Columns.Add("FechaPedido");
                dt.Columns.Add("FechaCreacion");
                dt.Columns.Add("FechaEntrega");
                dt.Columns.Add("FechaCaducidad");
                dt.Columns.Add("Proveedor");
                dt.Columns.Add("Cuenta");
                dt.Columns.Add("IdOC");
                dt.Columns.Add("LeandingCostC");
                dt.Columns.Add("EstadoLinea");
                dt.Columns.Add("Divisa");
                foreach (var user in ListaCarga)
                {
                    var newRow = dt.NewRow();
                    newRow["SolNro"] = user.SolNro;
                    newRow["NroOC"] = user.NroOC;
                    newRow["LineaPedido"] = user.LineaPedido;
                    newRow["NroArticulos"] = user.NroArticulos;
                    newRow["NroSPE"] = user.NroSPE;
                    newRow["HACAT"] = user.HACAT;
                    newRow["PersonaSolicitud"] = user.PersonaSolicitud;
                    newRow["FechaEnvio"] = user.FechaEnvio.ToString("dd-MM-yyyy").Replace("1/01/0001 12:00:00 a. m.",string.Empty);
                    newRow["Estado"] = user.Estado;
                    newRow["Tipo"] = user.Tipo;
                    newRow["Articulo"] = user.Articulo;
                    newRow["Cantidad"] = user.Cantidad;
                    newRow["TotalLinea"] = user.TotalLinea;
                    newRow["CodigoIVA"] = user.CodigoIVA;
                    newRow["CuadroCuentas"] = user.CuadroCuentas;
                    newRow["PersonaCreador"] = user.PersonaCreador;
                    newRow["FechaPedido"] = user.FechaPedido.ToString("dd-MM-yyyy").Replace("1/01/0001 12:00:00 a. m.", string.Empty); ;
                    newRow["FechaCreacion"] = user.FechaCreacion.ToString("dd-MM-yyyy");
                    newRow["FechaEntrega"] = user.FechaEntrega.ToString("dd-MM-yyyy").Replace("1/01/0001 12:00:00 a. m.", string.Empty); ;
                    newRow["FechaCaducidad"] = user.FechaCaducidad.ToString("dd-MM-yyyy").Replace("1/01/0001 12:00:00 a. m.", string.Empty); ;
                    newRow["Proveedor"] = user.Proveedor;
                    newRow["Cuenta"] = user.Cuenta;
                    newRow["IdOC"] = user.IdOC;
                    newRow["LeandingCostC"] = user.LeandingCostC;
                    newRow["EstadoLinea"] = user.EstadoLinea;
                    newRow["Divisa"] = user.Divisa;
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

                new DMSolped().InsertarSolped(XMLformat);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Convertir Errores Excel a XML
        public void ConvertToXML(List<DTErroresExcel> ListaCargaErrores)
        {
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
                    newRow["IdModalidad"] = Constantes.IdModSolped;
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

                new DMSolped().InsertarErroresExcel(XMLformat);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region Consultar Registros Exitosos
        public List<DTSolped> ConsultarRegistrosEx()
        {
            try
            {
                return new DMSolped().ConsultaRegistroExitoso();
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
                return new DMSolped().ConsultarErroresExcel();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        #endregion

        #region Consulta Solped Filtros
        public List<DTSolped> ConsultarSolpedFiltros(DTFiltros Dt)
        {
            try
            {
                return new DMSolped().ConsultarSolpedFiltros(Dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
