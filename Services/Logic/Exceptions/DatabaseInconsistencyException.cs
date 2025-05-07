using Services.Facade.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logic.Exceptions
{
    internal class DatabaseInconsistencyException : Exception
    {
        public DatabaseInconsistencyException() : base("Hay una inconsistencia crítica en BBDD. Revisar.".Translate()) 
        { 
        }
    }
}
