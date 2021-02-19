namespace Sanofi.DT.General
{
    public class DTFilaValidaciones
    {
        public string Nombre { get; set; }
        public string Valor { get; set; }

        // 0 = ninguna
        // 1 = nulo - entero
        // 2 = nulo - fecha
        // 3 = nulo - decimal
        // 4 = fecha
        // 5 = int
        // 6 = null
        // 7 = decimal
        public int Validaciones { get; set; }
        public int? Id { get; set; }
    }
}
