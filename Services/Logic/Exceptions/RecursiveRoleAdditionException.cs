using Services.Facade.Extensions;
using System;
using System.Runtime.Serialization;

namespace Services.Logic.Exceptions
{
    [Serializable]
    public class RecursiveRoleAdditionException : Exception
    {
        public RecursiveRoleAdditionException(string message = "No se puede agregar un rol hijo cuando ya es padre del rol destino.") : base(message.Translate())
        {
        }

        public RecursiveRoleAdditionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RecursiveRoleAdditionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}