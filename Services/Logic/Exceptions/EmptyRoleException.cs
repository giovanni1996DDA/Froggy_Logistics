using Services.Facade.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logic.Exceptions
{
    public class EmptyRoleException : Exception
    {
        public EmptyRoleException() : base("El rol no puede estar vacío.".Translate())
        { 
        }
    }
}
