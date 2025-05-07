using System;
using System.Runtime.Serialization;

namespace Logic.Exceptions
{
    [Serializable]
    public class NoTurnosFoundException : Exception
    {
        public NoTurnosFoundException(string message = "No se encontraron turnos con el criterio de búsqueda seleccionado.") : base(message)
        {
        }

        public NoTurnosFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoTurnosFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}