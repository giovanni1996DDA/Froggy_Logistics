using System;
using System.Runtime.Serialization;

namespace Logic
{
    [Serializable]
    public class TurnoIsOverlappingException : Exception
    {
        public TurnoIsOverlappingException(string message = "El turno se superpone con otro turno del mismo profesional.") : base(message)
        {
        }

        public TurnoIsOverlappingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TurnoIsOverlappingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}