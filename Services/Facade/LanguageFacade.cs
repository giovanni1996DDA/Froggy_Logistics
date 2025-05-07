using Services.Domain;
using Services.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Facade
{
    public static class LanguageFacade
    {
        public static string Translate(string key)
        {
            return LanguageLogic.Instance.Translate(key);
        }

        public static List<LanguageItem> GetLanguages()
        {
            return LanguageLogic.Instance.GetLanguages();
        }

        public static string GetCurrentLanguage()
        {
            return LanguageLogic.Instance.GetCurrentLanguage();
        }

        public static void SetCurrentLanguage(string language)
        {
            LanguageLogic.Instance.SetCurrentLanguage(language);
        }
    }
}
