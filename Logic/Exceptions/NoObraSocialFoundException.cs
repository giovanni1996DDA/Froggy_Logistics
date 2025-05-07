using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Exceptions
{
    public class NoObraSocialFoundException : Exception
    {
        public NoObraSocialFoundException() : base("No se encontraron obras sociales con el criterio de búsqueda seleccionado.")
        {
        }
    }
}
