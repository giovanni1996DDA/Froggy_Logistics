using Services.Facade.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logic.Exceptions
{
    internal class UserDoesNotExistException : Exception
    {
        public UserDoesNotExistException() : base("El usuario no existe.".Translate())
        { 
        }
    }
}
