using Services.Facade.Extensions;
using System;
using System.Runtime.Serialization;

namespace Services.Logic.Exceptions
{
    [Serializable]
    public class PermisoWithCredentialsAlreadyExistsException : Exception
    {
        public PermisoWithCredentialsAlreadyExistsException(string message = "Ya existe un permiso con ese tipo y ese modulo creado.") : base(message.Translate())
        {
        }

        public PermisoWithCredentialsAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PermisoWithCredentialsAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}