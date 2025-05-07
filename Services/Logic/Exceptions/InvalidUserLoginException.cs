using Services.Facade.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logic.Exceptions
{
    internal class InvalidUserLoginException : Exception
    {
        public InvalidUserLoginException() : base("Username o Password incorrecto".Translate())
        { 
        }
    }
}
