using Services.Facade.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logic.Exceptions
{
    public class NoAccesosFoundException : Exception
    {
        public NoAccesosFoundException() : base("No se encontraro accesos con el criterio de busqueda seleccionado".Translate())
        {

        }
    }
}
