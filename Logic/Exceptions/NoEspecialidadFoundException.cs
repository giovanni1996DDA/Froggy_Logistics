using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Exceptions
{
    public class NoEspecialidadFoundException : Exception
    {
        public NoEspecialidadFoundException() : base("No se encontraron especialidades con el criterio de búsqueda") 
        { 
        }
    }
}
