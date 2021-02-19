using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Sanofi.Soporte.Utils
{
    public class ConvertData
    {
        /// <summary>
        /// Convierte una lista a un DataTable
        /// </summary>
        /// <typeparam name="T">Tipo Generico</typeparam>
        /// <param name="datos">Lista de datos</param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        /// <summary>
        /// Convierte un datatabel eun una collection de datos especifico
        /// </summary>
        /// <typeparam name="T">Tipo Generico</typeparam>
        /// <param name="datos">Datatable de datos</param>
        /// <returns></returns>
        public static List<T> ConvertirDtoToList<T>(DataTable datos) where T : class, new()
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            var dataList = new List<T>(datos.Rows.Count);

            if (datos != null && datos.Rows.Count > 0)
            {
                var objFieldNames = (from PropertyInfo aProp in typeof(T).GetProperties(flags)
                                     select new { Name = aProp.Name, Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType }).ToList();

                var dataTblFieldNames = (from DataColumn aHeader in datos.Columns
                                         select new { Name = aHeader.ColumnName, Type = aHeader.DataType }).ToList();

                var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();

                foreach (DataRow dataRow in datos.AsEnumerable().ToList())
                {
                    var aTSource = new T();
                    foreach (var aField in commonFields)
                    {
                        PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);
                        var value = (dataRow[aField.Name] == DBNull.Value) ? null : dataRow[aField.Name];
                        propertyInfos.SetValue(aTSource, value, null);
                    }

                    dataList.Add(aTSource);
                }
            }
            else
            {
                dataList = new List<T>();
            }

            return dataList;
        }
        public static string ConvertObjectToXML<T>(T Obj)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            string xml = string.Empty;
            XmlDocument _XmlDocument = new XmlDocument();
            //Serializar objeto
            using (StringWriter sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    ser.Serialize(writer, Obj);
                    xml = sww.ToString(); // Your XML
                }
            }

            //Cargar en objeto xmldocument
            _XmlDocument.LoadXml(xml);
            //eliminar primer nodo de declaracion xml
            if (_XmlDocument.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                _XmlDocument.RemoveChild(_XmlDocument.FirstChild);
            return _XmlDocument.InnerXml;
        }


        public static bool ToExcelFile(DataTable dt, string filename)
        {
            bool Success = false;
            try
            {

                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(dt, "Sheet 1");
                //if (filename.Contains("."))
                //{
                //    int IndexOfLastFullStop = filename.LastIndexOf('.');
                //    filename = filename.Substring(0, IndexOfLastFullStop) + ".xlsx";
                //}
                //filename = filename + ".xlsx";
                wb.SaveAs(filename);
                Success = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return Success;
        }
        public static string AplicarUTF(string cadena)
        {
            string value = cadena;
            byte[] tempBytes;
            tempBytes = Encoding.GetEncoding("ISO-8859-8").GetBytes(cadena);
            value = Encoding.UTF8.GetString(tempBytes);
            return value;
        }

        public static string RemoveAccentsWithRegEx(string inputString)
        {
            Regex replace_a_Accents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex replace_e_Accents = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex replace_i_Accents = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex replace_o_Accents = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex replace_u_Accents = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            inputString = replace_a_Accents.Replace(inputString, "a");
            inputString = replace_e_Accents.Replace(inputString, "e");
            inputString = replace_i_Accents.Replace(inputString, "i");
            inputString = replace_o_Accents.Replace(inputString, "o");
            inputString = replace_u_Accents.Replace(inputString, "u");
            return inputString;
        }
    }
}
