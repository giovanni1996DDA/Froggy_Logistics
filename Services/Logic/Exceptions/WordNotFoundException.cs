using Services.Facade.Extensions;
using System;
using System.Runtime.Serialization;

namespace Services.Logic.Exceptions
{
    [Serializable]
    public class WordNotFoundException : Exception
    {
        public WordNotFoundException()
        {
        }

        public WordNotFoundException(string message = "No se encontró la palabra buscada.") : base(message.Translate())
        {
        }

        public WordNotFoundException(string message, Exception innerException) : base(message.Translate(), innerException)
        {
        }

        protected WordNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}