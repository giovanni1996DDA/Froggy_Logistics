using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Exceptions
{
    public class PacienteAlreadyExistsException : Exception
    {
        public PacienteAlreadyExistsException() : base("Ya existe un paciente con ese número de documento")
        {
        }
    }
}
