namespace Sanofi.DT.Usuarios
{
    public class DTMaestroUsuario
    {
        public string Descripcion { get; set; }
        public int IdRol { get; set; }
        public string Usuario { get; set; }
        public int IdUsuario { get; set; }
        public bool Estado { get; set; }
        public int IdModalidad { set; get; }
        public string Nombre { set; get; }
        public string RutaAcceso { get; set; }
        public string IdRoles { get; set; }
    }
}
