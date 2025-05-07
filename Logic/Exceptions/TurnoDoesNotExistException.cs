using System;
using System.Runtime.Serialization;

namespace Logic.Exceptions
{
    [Serializable]
    public class TurnoDoesNotExistException : Exception
    {
        public TurnoDoesNotExistException()
        {
        }

        public TurnoDoesNotExistException(string message = "El turno especificado no existe.") : base(message)
        {
        }

        public TurnoDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TurnoDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}