using Services.Facade.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Exceptions
{
    public class PacienteDoesNotExistsException : Exception
    {
        public PacienteDoesNotExistsException() : base("No existe ningun paciente con los criterios de búsqueda seleccionados.".Translate())
        {
        }
    }
}
