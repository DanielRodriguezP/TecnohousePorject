using System;

namespace Sanofi.DT.Excel
{
    public class DTErroresExcel
    {
        public Int32 IdErroresExcel { get; set; }
        public string Celda { get; set; }
        public string Campo { get; set; }
        public string Mensaje { get; set; }
        public int Fila { get; set; }
        public int Columna { get; set; }
        public Int16 IdModalidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Valor { get; set; }
    }
}
