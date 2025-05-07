using Services.Facade.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logic.Exceptions
{
    public class NoUsersFoundException : Exception
    {
        public NoUsersFoundException() : base("No se encontraron usuarios con el criterio de búsqueda seleccionado.".Translate()) 
        { 
        }
    }
}
