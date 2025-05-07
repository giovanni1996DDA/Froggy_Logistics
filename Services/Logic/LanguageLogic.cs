using Services.Dao.Implementations.Generics;
using Services.Domain;
using Services.Logic.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logic
{
    public class LanguageLogic
    {
        #region Singleton
        private static LanguageLogic _instance = new LanguageLogic();
        public static LanguageLogic Instance { 
            get 
            { 
                return _instance; 
            } 
        }
        private LanguageLogic() 
        {
        }
        #endregion

        public string Translate(string key)
        {
            try
            {
                //si empieza por * se igniora la traduccion. es un comodin
                if (key.StartsWith("*")) return key;

                string returning = LanguageDao.Translate(key);

                if (string.IsNullOrEmpty(returning)) return "????";

                return returning;

            }
            catch(FileNotFoundException ex)
            {
                throw;
            }
            catch(WordNotFoundException ex)
            {
                LanguageDao.WriteKey(key);
                return "????";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<LanguageItem> GetLanguages()
        {
            try
            {
                return LanguageDao.GetLanguages();
            }

            catch (Exception)
            {
                throw;
            }
        }
        internal string GetCurrentLanguage()
        {
            return LanguageDao.GetCurrentLanguage();
        }

        internal void SetCurrentLanguage(string language)
        {
            LanguageDao.SetCurrentLanguage(language);
        }
    }
}
