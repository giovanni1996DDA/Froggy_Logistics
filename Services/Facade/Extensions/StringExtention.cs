using Services.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Facade.Extensions
{
    public static class StringExtention
    {
        public static string Translate(this string key)
        {
            return LanguageFacade.Translate(key);
        }
    }
}
