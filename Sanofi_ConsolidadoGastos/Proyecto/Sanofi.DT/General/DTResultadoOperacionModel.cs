using Sanofi.DT.Mensajes;

namespace Sanofi.DT.General
{
    public class DTResultadoOperacionModel<T>
    {
        /// <summary>
        /// Mensaje que se a mostrar al usuario
        /// </summary>
        public DTMensaje Mensaje { get; set; }
        /// <summary>
        /// Respuesta de la operación realizada
        /// </summary>
        public bool Respuesta { get; set; }
        /// <summary>
        /// Resultado de la operación realizada
        /// </summary>
        public T Resultado { get; set; }
    }
}
