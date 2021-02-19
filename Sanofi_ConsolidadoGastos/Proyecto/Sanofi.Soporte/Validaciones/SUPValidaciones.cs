namespace Sanofi.Soporte.Validaciones
{
    using Sanofi.DT.Excel;
    using Sanofi.DT.General;
    using Sanofi.DT.Solped;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class SUPValidaciones
    {
        #region Obtener Valores de las celdas de Excel
        public static string ObtenerValorExcel(int index, DTFilasExcel filas)
        {
            string Valor = string.Empty;
            Valor = (from dt in filas.cells
                     where dt.index == index
                     select dt.value != null ? dt.value.ToString() : string.Empty).FirstOrDefault();
            return string.IsNullOrEmpty(Valor) ? string.Empty : Valor.Replace(Constantes.ValidacionEncabezados, string.Empty).Trim().ToString();
        }
        #endregion

        public static DTErroresExcel Validacion(DTFilaValidaciones Valores, int NFila)
        {
            Regex Numeros = new Regex(Constantes.ExpRegSoloNumeros);
            Regex rexNumeroPunto = new Regex(Constantes.ExpRegNumeroPunto);

            DTErroresExcel _Excel = new DTErroresExcel();
            try
            {
                
                switch (Valores.Validaciones)
                {
                    case 0:
                        _Excel.Mensaje = string.Empty;
                        break;
                    case 1:
                        if (!string.IsNullOrEmpty(Valores.Valor))
                        {
                            if (!Numeros.IsMatch(Valores.Valor))
                            {
                                _Excel.Mensaje = "El campo no debe llevar caracteres especiales.";
                                _Excel.Fila = NFila + 1;
                                _Excel.Campo = Valores.Nombre;
                                _Excel.Valor = Valores.Valor;
                            }
                            else
                            {
                                _Excel.Mensaje = string.Empty;
                            }
                        }
                        else
                        {
                            _Excel.Mensaje = "El campo se encuentra vacío.";
                            _Excel.Fila = NFila + 1;
                            _Excel.Campo = Valores.Nombre;
                            _Excel.Valor = Valores.Valor;
                        }
                        break;
                    case 2:
                        if (!string.IsNullOrEmpty(Valores.Valor))
                        {
                            try
                            {
                                DateTime temp = DateTime.ParseExact(Valores.Valor, "dd/MM/yy", CultureInfo.InvariantCulture);
                                _Excel.Fecha = DateTime.ParseExact(temp.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                _Excel.Mensaje = string.Empty;
                            }
                            catch (Exception e)
                            {
                                try
                                {
                                    DateTime fecha_inicio_anio_actual = DateTime.FromOADate(Convert.ToInt32(Valores.Valor));
                                    _Excel.Fecha = fecha_inicio_anio_actual;
                                    _Excel.Mensaje = string.Empty;
                                }
                                catch (Exception)
                                {
                                    _Excel.Mensaje = "El campo no tiene el formato correcto.";
                                    _Excel.Fila = NFila + 1;
                                    _Excel.Campo = Valores.Nombre;
                                    _Excel.Valor = Valores.Valor;
                                }
                                
                            }

                        }
                        else
                        {
                            _Excel.Mensaje = "El campo se encuentra vacío.";
                            _Excel.Fila = NFila + 1;
                            _Excel.Campo = Valores.Nombre;
                            _Excel.Valor = Valores.Valor;
                        }
                        break;
                    case 3:
                        if (!string.IsNullOrEmpty(Valores.Valor))
                        {
                            if (!rexNumeroPunto.IsMatch(Valores.Valor))
                            {
                                _Excel.Mensaje = "El campo no debe llevar caracteres especiales.";
                                _Excel.Fila = NFila + 1;
                                _Excel.Campo = Valores.Nombre;
                                _Excel.Valor = Valores.Valor;
                            }
                            else
                            {
                                _Excel.Mensaje = string.Empty;
                            }
                        }
                        else
                        {
                            _Excel.Mensaje = "El campo se encuentra vacío.";
                            _Excel.Fila = NFila + 1;
                            _Excel.Campo = Valores.Nombre;
                            _Excel.Valor = Valores.Valor;
                        }
                        break;
                    case 4:
                        if (!string.IsNullOrEmpty(Valores.Valor))
                        {
                            try
                            {
                                DateTime temp = DateTime.ParseExact(Valores.Valor, "dd/MM/yy", CultureInfo.InvariantCulture);
                                _Excel.Fecha = DateTime.ParseExact(temp.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                _Excel.Mensaje = string.Empty;
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    DateTime fecha_inicio_anio_actual = DateTime.FromOADate(Convert.ToInt32(Valores.Valor));
                                    _Excel.Fecha = fecha_inicio_anio_actual;
                                    _Excel.Mensaje = string.Empty;
                                }
                                catch (Exception)
                                {
                                    _Excel.Mensaje = "El campo no tiene el formato correcto.";
                                    _Excel.Fila = NFila + 1;
                                    _Excel.Campo = Valores.Nombre;
                                    _Excel.Valor = Valores.Valor;
                                }
                            }
                        }
                        else
                        {
                            _Excel.Fecha = default;
                            _Excel.Mensaje = string.Empty;
                        }
                        break;
                    case 5:
                        if (!string.IsNullOrEmpty(Valores.Valor))
                        {
                            if (!Numeros.IsMatch(Valores.Valor))
                            {
                                _Excel.Mensaje = "El campo no debe llevar caracteres especiales.";
                                _Excel.Fila = NFila + 1;
                                _Excel.Campo = Valores.Nombre;
                                _Excel.Valor = Valores.Valor;
                            }
                            else
                            {
                                _Excel.Mensaje = string.Empty;
                            }
                        }
                        else
                        {
                            _Excel.Mensaje = string.Empty;
                        }
                        break;
                    case 6:
                        if (!string.IsNullOrEmpty(Valores.Valor))
                        {
                            _Excel.Mensaje = string.Empty;
                        }
                        else
                        {
                            _Excel.Mensaje = "El campo se encuentra vacío.";
                            _Excel.Fila = NFila + 1;
                            _Excel.Campo = Valores.Nombre;
                            _Excel.Valor = Valores.Valor;
                        }
                        break;
                    case 7:
                        if (!string.IsNullOrEmpty(Valores.Valor))
                        {
                            if (!rexNumeroPunto.IsMatch(Valores.Valor))
                            {
                                _Excel.Mensaje = "El campo no debe llevar caracteres especiales.";
                                _Excel.Fila = NFila + 1;
                                _Excel.Campo = Valores.Nombre;
                                _Excel.Valor = Valores.Valor;
                            }
                            else
                            {
                                _Excel.Mensaje = string.Empty;
                            }
                        }
                        else
                        {
                            _Excel.Mensaje = string.Empty;
                        }
                        break;
                }
            }catch(Exception ex)
            {
                throw ex;
            }
            return _Excel;
        }

    }
}
