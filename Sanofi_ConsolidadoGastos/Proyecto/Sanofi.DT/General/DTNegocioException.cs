using System;
using System.Runtime.Serialization;

namespace Sanofi.DT.General
{
    public class DTNegocioException : Exception
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase DTNegocioException.
        /// </summary>
        public DTNegocioException()
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase DTNegocioException.
        /// </summary>
        /// <param name="mensaje">Mensaje de error que explica la razón de la excepción.</param>
        public DTNegocioException(string mensaje)
            : base(mensaje)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase DTNegocioException.
        /// </summary>
        /// <param name="exception">Objeto Exception.</param>
        public DTNegocioException(Exception exception)
            : base(exception.Message)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase DTNegocioException.
        /// </summary>
        /// <param name="mensaje">Mensaje de error que explica la razón de la excepción.</param>
        /// <param name="innerExcepcion">Excepción que causa la excepción actual, o
        /// ReferenceData null si no es especificada la innerExcepcion.</param>
        public DTNegocioException(string mensaje, Exception innerExcepcion)
            : base(mensaje, innerExcepcion)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase DTNegocioException.
        /// </summary>
        /// <param name="info">Información de la serialización.</param>
        /// <param name="context">Contexto de la serializáción.</param>
        protected DTNegocioException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
