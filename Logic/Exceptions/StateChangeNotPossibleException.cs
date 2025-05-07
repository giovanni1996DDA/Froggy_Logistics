using System;
using System.Runtime.Serialization;

namespace Logic.Exceptions
{
    [Serializable]
    public class StateChangeNotPossibleException : Exception
    {
        public StateChangeNotPossibleException(string message = "No es posible realizar el cambio de estado.") : base(message)
        {
        }

        public StateChangeNotPossibleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StateChangeNotPossibleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}