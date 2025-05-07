using Services.Facade.Extensions;
using System;
using System.Runtime.Serialization;

namespace Services.Logic
{
    [Serializable]
    public class RoleAlreadyExistInFatherException : Exception
    {
        public RoleAlreadyExistInFatherException(string message = "El rol que se está intentando agregar ya existe como hijo.") : base(message.Translate())
        {
        }

        public RoleAlreadyExistInFatherException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RoleAlreadyExistInFatherException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}