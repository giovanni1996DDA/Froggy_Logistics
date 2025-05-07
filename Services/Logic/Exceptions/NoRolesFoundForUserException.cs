using Services.Facade.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Services.Logic.Exceptions
{
    internal class NoRolesFoundForUserException : Exception
    {
        public NoRolesFoundForUserException() : base($"No se encontraron roles con el criterio de búsqueda seleccionado.".Translate()) 
        { 
        }
    }
}
