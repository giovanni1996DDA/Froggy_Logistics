using Services.Facade.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logic.Exceptions
{
    public class NoScreensFoundException : Exception
    {
        public NoScreensFoundException() : base("No se encontraron pantallas con el criterio de búsqueda seleccionado.".Translate())
        {
        }
    }
}
