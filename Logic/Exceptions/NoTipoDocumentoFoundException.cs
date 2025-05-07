using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Exceptions
{
    public class NoTipoDocumentoFoundException : Exception
    {
        public NoTipoDocumentoFoundException() : base("No se encontraron tipos de documento con el critrerio de búsqueda seleccionado")
        { 
        }
    }
}
