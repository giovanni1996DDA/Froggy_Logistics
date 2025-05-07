using Services.Facade.Extensions;
using System;
using System.Runtime.Serialization;

namespace Services.Logic.Exceptions
{
    [Serializable]
    public class NoRolesFoundException : Exception
    {
        public NoRolesFoundException()
        {
        }

        public NoRolesFoundException(string message) : base(message.Translate())
        {
        }

        public NoRolesFoundException(string message, Exception innerException) : base(message.Translate(), innerException)
        {
        }

        protected NoRolesFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}