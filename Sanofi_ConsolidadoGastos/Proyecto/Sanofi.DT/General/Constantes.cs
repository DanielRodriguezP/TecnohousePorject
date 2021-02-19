namespace Sanofi.DT.General
{
    public class Constantes
    {
        /// <summary>
        /// Modalidad de Athena
        /// </summary>
        public const int IdModAthena = 1;
        /// <summary>
        /// Modalidad de Solped
        /// </summary>
        public const int IdModSolped = 2;
        /// <summary>
        /// Modalidad de Gastos
        /// </summary>
        public const int IdModGastos = 3;
        /// <summary>
        /// Expresión utilizada para validar los encabezados de las plantillas de excel
        /// </summary>
        public const string ValidacionEncabezados = "{\r\n  \"code\": \"N/A\"\r\n}";

        /// <summary>
        /// Expresión regular para los campos numéricos
        /// </summary>
        public const string ExpRegSoloNumeros = @"^[+-]?\d+(\d+)?$";

        /// <summary>
        /// Expresión regular para los campos numéricos
        /// </summary>
        public const string ExpRegAlfaNumericos = "^[A-Z0-9 a-z]*$";

        public const string ExpRegAlfaNumericosNumIde = @"^[A-Z0-9 a-z]*[-]?[\d]$";

        /// <summary>
        /// Expresión regular para los nombres
        /// </summary>
        public const string ExpRegAlfaNumericosNombres = "^[A-Z0-9 a-z ñÑ]*$";

        /// <summary>
        /// Expresión regular para los campos que admiten sólo letras y tildes
        /// </summary>
        public const string ExpRegSoloLetras = "^[A-Z a-z]*$";

        /// <summary>
        /// Expresión regular para validar numeros decimales
        /// </summary>
        public const string ExpRegNumeroPunto = "^[0-9,.-]*$";

        public const string ExpRegEspaciosBlanco = @"[^\s] ";
    }
}
