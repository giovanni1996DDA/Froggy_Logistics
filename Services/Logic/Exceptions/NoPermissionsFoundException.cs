using Services.Facade.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logic.Exceptions
{
    public class NoPermissionsFoundException : Exception
    {
        public NoPermissionsFoundException() : base("No se encontraron permisos con el criterio especificado".Translate())
        { 
        }
    }
}
