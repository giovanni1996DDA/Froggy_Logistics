using Services.Dao.Implementations.Generics;
using Services.Domain;
using Services.Logic.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Services.Dao.Implementations.Generics
{
    public static class LanguageDao
    {
        private static string filePath = ConfigurationManager.AppSettings["LanguagePath"];

        public static string Translate(string key)
        {
            string lang = ConfigurationManager.AppSettings["CurrentLanguage"];
            string fileName = filePath + "language." + lang;

            using (StreamReader sr = new StreamReader(fileName))
            {
                while (!sr.EndOfStream)
                {
                    string [] line = sr.ReadLine().Split('=');

                    if (line[0].ToLower() == key.ToLower()) return line[1];
                }
            }
            throw new WordNotFoundException();
        }

        public static void WriteKey(string key)
        {
            string lang = ConfigurationManager.AppSettings["CurrentLanguage"];
            string fileName = filePath + "language." + lang;

            using (StreamWriter sw = new StreamWriter(fileName, append: true))
            {
                sw.WriteLine($"{key}=");
            }
        }

        public static List<LanguageItem> GetLanguages()
        {

            var languageFiles = Directory.GetFiles(filePath, "language.*")
                            .Select(name => name.Split('.').Last())   // Obtener la parte despues de "language."
                            .ToList();


            var languages = new List<LanguageItem>();

            foreach (var lang in languageFiles)
            {
                try
                {
                    var culture = new CultureInfo(lang);
                    languages.Add(new LanguageItem
                    {
                        DisplayName = culture.NativeName,
                        Value = lang
                    });
                }
                catch (CultureNotFoundException)
                {
                    continue;
                }
                catch (Exception)
                {

                    throw;
                }

            }
            return languages;
        }

        public static string GetCurrentLanguage()
        {
            return ConfigurationManager.AppSettings["CurrentLanguage"];
        }

        public static void SetCurrentLanguage(string language)
        {
            // Cargar la configuración actual
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // Actualizar la clave si existe, o agregarla si no existe
            if (config.AppSettings.Settings["CurrentLanguage"] != null)
            {
                config.AppSettings.Settings["CurrentLanguage"].Value = language;
            }

            // Guardar los cambios en el archivo de configuración
            config.Save(ConfigurationSaveMode.Modified);

            // Refrescar la sección para asegurarse de que los cambios se reflejen en tiempo de ejecución
            ConfigurationManager.RefreshSection("appSettings");

        }
    }
}
