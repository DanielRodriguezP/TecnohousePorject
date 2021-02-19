using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanofi.DT.Consolidado
{
    public class DTConsolidadoExcel
    {
        public string Ejercicio { get; set; }
        public int Periodo { get; set; }
        public string Cuenta { get; set; }
        public string Descripcion { get; set; }
        public string ctaCenco { get; set; }
        public string CNATClase { get; set; }
        public string CNATsubc { get; set; }
        public string Cnco { get; set; }
        public string Denominacion { get; set; }
        public string Responsable { get; set; }
        public decimal Valor { get; set; }
        public string ISIndicator { get; set; }
        public string MCMS { get; set; }
        public string MasaSalarial { get; set; }
        public string Agrupador { get; set; }

        public string CuentaAnterior { get; set; }
        public int Tipo { get; set; }
    }
}
