namespace Sanofi.DT.Gastos
{
    using System;

    public class DTGastos
    {
        public string NomCuenta { get; set; }
        public string NomCentroC { get; set; }
        public string NroCuenta { get; set; }
        public double? CentroCoste { get; set; }
        public string Concatenar { get; set; }
        public string Tipo { get; set; }
        public decimal? Mes { get; set; }
        public decimal? Ajustes { get; set; }
        public decimal? TotalMes { get; set; }
        public int Fila { get; set; }
        public int Recent { get; set; }
    }
    

}