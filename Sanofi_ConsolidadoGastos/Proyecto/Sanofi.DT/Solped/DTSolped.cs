namespace Sanofi.DT.Solped
{
    using System;

    public class DTSolped
    {
        public int SolNro { get; set; }
        public string NroOC { get; set; }
        public int? LineaPedido { get; set; }
        public int NroArticulos { get; set; }
        public string NroSPE { get; set; }
        public string HACAT { get; set; }
        public string PersonaSolicitud { get; set; }
        public DateTime FechaEnvio { get; set; }
        public string Estado { get; set; }
        public string Tipo { get; set; }
        public string Articulo { get; set; }
        public decimal Cantidad { get; set; }
        public decimal TotalLinea { get; set; }
        public string CodigoIVA { get; set; }
        public string CuadroCuentas { get; set; }
        public string PersonaCreador { get; set; }
        public DateTime FechaPedido { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaEntrega { get; set; }
        public DateTime FechaCaducidad { get; set; }
        public string Proveedor { get; set; }
        public string Cuenta { get; set; }
        public int? IdOC { get; set; }
        public string LeandingCostC { get; set; }
        public string EstadoLinea { get; set; }
        public string Divisa { get; set; }
        public int Fila { get; set; }
        public int Recent { get; set; }
    }
    

}