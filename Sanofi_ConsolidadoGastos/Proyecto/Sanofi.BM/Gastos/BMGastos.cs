namespace Sanofi.BM.Gastos
{
    using Sanofi.DM.Gastos;
    using Sanofi.DT.Excel;
    using Sanofi.DT.Gastos;
    using Sanofi.DT.General;
    using Sanofi.DT.Mensajes;
    using Sanofi.Soporte;
    using Sanofi.Soporte.Validaciones;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class BMGastos
    {
        
        #region Leer archivo y realizar validaciones 
        public DTResultadoOperacionModel<DTErroresExcel> LeerArchivo(List<DTFilasExcel> fileFactura,string MesGasto)
        {
            DTResultadoOperacionModel<DTGastos> _DTResultadoModel = new DTResultadoOperacionModel<DTGastos>();
            DTResultadoOperacionModel<DTErroresExcel> resultado = new DTResultadoOperacionModel<DTErroresExcel>();

            List<DTGastos> ListaCarga = new List<DTGastos>();
            List<DTErroresExcel> ListaCargaIncompleta = new List<DTErroresExcel>();
            try
            {
                List<DTFilasExcel> informacion = (from dt in fileFactura
                                                  where dt.index != 0
                                                  select dt).ToList();

                foreach (var filas in informacion)
                {
                    DTGastos _MMExcel = new DTGastos();
                    DTFilaValidaciones[] Fila = new DTFilaValidaciones[8];
                    Fila[0] = new DTFilaValidaciones() { Nombre = DTEstructuraGastos.NomCuenta, Valor = SUPValidaciones.ObtenerValorExcel(0, filas), Validaciones = 6, Id=1};
                    Fila[1] = new DTFilaValidaciones() { Nombre = DTEstructuraGastos.NomCentroC, Valor = SUPValidaciones.ObtenerValorExcel(1, filas), Validaciones = 6 };
                    Fila[2] = new DTFilaValidaciones() { Nombre = DTEstructuraGastos.NroCuenta, Valor = SUPValidaciones.ObtenerValorExcel(2, filas), Validaciones = 6, Id = 2 };
                    Fila[3] = new DTFilaValidaciones() { Nombre = DTEstructuraGastos.CentroCoste, Valor = SUPValidaciones.ObtenerValorExcel(3, filas), Validaciones = 5 };
                    //Fila[4] = new DTFilaValidaciones() { Nombre = DTEstructuraGastos.Concatenar, Valor = SUPValidaciones.ObtenerValorExcel(4, filas), Validaciones = 6 };
                    Fila[4] = new DTFilaValidaciones() { Nombre = DTEstructuraGastos.Tipo, Valor = SUPValidaciones.ObtenerValorExcel(5, filas), Validaciones = 6 };
                    Fila[5] = new DTFilaValidaciones() { Nombre = DTEstructuraGastos.Mes, Valor = SUPValidaciones.ObtenerValorExcel(6, filas), Validaciones = 7 };
                    //Fila[6] = new DTFilaValidaciones() { Nombre = DTEstructuraGastos.Cambios, Valor = SUPValidaciones.ObtenerValorExcel(7, filas), Validaciones = 7 };
                    Fila[6] = new DTFilaValidaciones() { Nombre = DTEstructuraGastos.Ajustes, Valor = SUPValidaciones.ObtenerValorExcel(7, filas), Validaciones = 7 };
                    Fila[7] = new DTFilaValidaciones() { Nombre = DTEstructuraGastos.TotalMes, Valor = SUPValidaciones.ObtenerValorExcel(8, filas), Validaciones = 7 };

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
                                case DTEstructuraGastos.NomCuenta:
                                    if(valores.Id == 1)
                                    {
                                        _MMExcel.NomCuenta = valores.Valor;
                                    }else if(valores.Id == 2)
                                    {
                                        _MMExcel.NroCuenta = valores.Valor;
                                    }
                                    break;
                                case DTEstructuraGastos.NomCentroC:
                                        _MMExcel.NomCentroC = valores.Valor;
                                    break;
                                case DTEstructuraGastos.CentroCoste:
                                    if (!string.IsNullOrEmpty(valores.Valor))
                                    {
                                        _MMExcel.CentroCoste = double.Parse(valores.Valor);
                                    }
                                    else
                                    {
                                        _MMExcel.CentroCoste = default;
                                    }
                                        break;
                                //case DTEstructuraGastos.Concatenar:
                                //    _MMExcel.Concatenar = valores.Valor;
                                //    break;
                                case DTEstructuraGastos.Tipo:
                                    _MMExcel.Tipo = valores.Valor;
                                    break;
                                case DTEstructuraGastos.Mes:
                                    if (!string.IsNullOrEmpty(valores.Valor))
                                    {
                                        _MMExcel.Mes = decimal.Parse(valores.Valor);
                                    }
                                    else
                                    {
                                        _MMExcel.Mes = default;
                                    }
                                    break;
                                case DTEstructuraGastos.Ajustes:
                                    if (!string.IsNullOrEmpty(valores.Valor))
                                    {
                                        _MMExcel.Ajustes = decimal.Parse(valores.Valor);
                                    }
                                    else
                                    {
                                        _MMExcel.Ajustes = default;
                                    }
                                    break;
                                case DTEstructuraGastos.TotalMes:
                                    if (!string.IsNullOrEmpty(valores.Valor))
                                    {
                                        _MMExcel.TotalMes = decimal.Parse(valores.Valor);
                                    }
                                    else
                                    {
                                        _MMExcel.TotalMes = default;
                                    }
                                    break;
                            }
                        }
                    }

                    _MMExcel.Fila = filas.index + 1;

                    //Registros Duplicados
                    List<DTGastos> ExisteRegistroDuplicados = (from dt in ListaCarga
                                                               where dt.NroCuenta == _MMExcel.NroCuenta && dt.CentroCoste == _MMExcel.CentroCoste
                                                               select dt).ToList();

                    //Validación Registros Duplicados
                    if (ExisteRegistroDuplicados.Count > 0)
                    {
                        DTErroresExcel _Excel1 = new DTErroresExcel();
                        _Excel1.Mensaje = "Se repiten registros.";
                        _Excel1.Fila = filas.index + 1;
                        _Excel1.Campo = DTEstructuraGastos.NroCuenta + ", " + DTEstructuraGastos.CentroCoste;
                        _Excel1.Valor = _MMExcel.NroCuenta.ToString() + ", " + _MMExcel.CentroCoste.ToString();
                        ListaCargaIncompleta.Add(_Excel1);

                        foreach (var item in ExisteRegistroDuplicados)
                        {
                            DTErroresExcel _Excel = new DTErroresExcel();
                            _Excel.Mensaje = "Se repiten registros.";
                            _Excel.Fila = item.Fila;
                            _Excel.Campo = DTEstructuraGastos.NroCuenta + ", " + DTEstructuraGastos.CentroCoste;
                            _Excel.Valor = _MMExcel.NroCuenta.ToString() + ", " + _MMExcel.CentroCoste.ToString();
                            ListaCargaIncompleta.Add(_Excel);
                            ListaCarga.RemoveAll(c => c.Fila == item.Fila);
                        }
                    }


                    // consultar si la fila no tiene inconvenientes
                    List<DTErroresExcel> ConInformacion = (from dt in ListaCargaIncompleta
                                                           where dt.Fila == (filas.index + 1)
                                                           select dt).ToList();

                    // si no prensentar inconveniente se agrega a la lista de carga bd.
                    if (ConInformacion.Count == 0)
                    {
                        ListaCarga.Add(_MMExcel);
                    }
                }

                //// inserta los exitoso o setea en 0 los estados
                ConvertToDataTable(ListaCarga,MesGasto);

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
        public void ConvertToDataTable(List<DTGastos> ListaCarga,string MesGasto)
        {
            string Xml = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("NomCuenta");
                dt.Columns.Add("NomCentroC");
                dt.Columns.Add("NroCuenta");
                dt.Columns.Add("CentroCoste");
                //dt.Columns.Add("Concatenar");
                dt.Columns.Add("Tipo");
                dt.Columns.Add("Mes");
                //dt.Columns.Add("Cambios");
                dt.Columns.Add("Ajustes");
                dt.Columns.Add("TotalMes");
                foreach (var user in ListaCarga)
                {
                    var newRow = dt.NewRow();
                    newRow["NomCuenta"] = user.NomCuenta;
                    newRow["NomCentroC"] = user.NomCentroC;
                    newRow["NroCuenta"] = user.NroCuenta;
                    newRow["CentroCoste"] = user.CentroCoste;
                    //newRow["Concatenar"] = user.Concatenar;
                    newRow["Tipo"] = user.Tipo;
                    newRow["Mes"] = user.Mes;
                    //newRow["Cambios"] = user.Cambios;
                    newRow["Ajustes"] = user.Ajustes;
                    newRow["TotalMes"] = user.TotalMes;
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

                new DMGastos().InsertarGastos(XMLformat,MesGasto);
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
                    newRow["IdModalidad"] = Constantes.IdModGastos;
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

                new DMGastos().InsertarErroresExcel(XMLformat);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region Consultar Registros Exitosos
        public List<DTGastos> ConsultarRegistrosEx()
        {
            try
            {
                return new DMGastos().ConsultaRegistroExitoso();
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
                return new DMGastos().ConsultarErroresExcel();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        #endregion

        #region Consulta Gastos Filtros
        public List<DTGastos> ConsultarGastosFiltros(DTFiltros Dt)
        {
            try
            {
                return new DMGastos().ConsultarGastosFiltros(Dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Consulta cuentas
        public List<DTObjetosFiltros> ConsultarCuentas(DTFiltros Dt)
        {
            try
            {
                return new DMGastos().ConsultarCuentas(Dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Consulta tipos
        public List<DTObjetosFiltros> ConsultarTipos(DTFiltros Dt)
        {
            try
            {
                return new DMGastos().ConsultarTipos(Dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Consulta concatenar
        public List<DTObjetosFiltros> ConsultarConcatenar(DTFiltros Dt)
        {
            try
            {
                return new DMGastos().ConsultarConcatenar(Dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
