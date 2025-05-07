using Services.Facade.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logic.Exceptions
{
    public class InvalidUserException : Exception
    {
        public InvalidUserException(string msg) : base(msg.Translate())
        { 
        }
    }
}
