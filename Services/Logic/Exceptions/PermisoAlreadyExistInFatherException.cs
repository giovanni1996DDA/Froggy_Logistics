using Services.Facade.Extensions;
using System;
using System.Runtime.Serialization;

namespace Services.Logic.Exceptions
{
    [Serializable]
    internal class PermisoAlreadyExistInFatherException : Exception
    {
        public PermisoAlreadyExistInFatherException(string message = "El permiso que se está intentando agregar ya existe como hijo.") : base(message.Translate().Translate())
        {
        }

        public PermisoAlreadyExistInFatherException(string message, Exception innerException) : base(message.Translate(), innerException)
        {
        }

        protected PermisoAlreadyExistInFatherException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}