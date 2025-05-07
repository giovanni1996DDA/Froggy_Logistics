using Services.Dao.Implemenations.SQLServer.Helpers;
using Services.Dao.Interfaces;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dao.Implementations.SQLServer
{
    /// <summary>
    /// Clase que implementa el sistema de registro de logs en archivos y en base de datos.
    /// Hereda de SqlTransactRepository para manejar operaciones con la base de datos para logs.
    /// </summary>
    public class LoggerDao : SqlTransactRepository<Log>
    {
        /// <summary>
        /// Constructor que inicializa LoggerDao con la conexión y transacción SQL activas.
        /// </summary>
        /// <param name="context">Conexión SQL.</param>
        /// <param name="_transaction">Transacción SQL.</param>
        public LoggerDao(SqlConnection context, SqlTransaction _transaction) : base(context, _transaction)
        {
        }

        /// <summary>
        /// Ruta donde se guardarán los logs de error.
        /// </summary>
        private static string PathLogError { get; set; } = ConfigurationManager.AppSettings["PathLogError"];

        /// <summary>
        /// Ruta donde se guardarán los logs de información.
        /// </summary>
        private static string PathLogInfo { get; set; } = ConfigurationManager.AppSettings["PathLogInfo"];

        /// <summary>
        /// Escribe un log en el archivo correspondiente, dependiendo del nivel de traza.
        /// Para errores graves, se adjunta el stack trace de la excepción.
        /// </summary>
        /// <param name="log">El objeto log que contiene el mensaje y nivel de traza.</param>
        /// <param name="ex">Excepción opcional, utilizada para logs de nivel Error.</param>
        public void WriteLog(Log log, Exception ex = null)
        {
            switch (log.TraceLevel)
            {
                case TraceLevel.Error:
                    string formatMessage = FormatMessage(log);
                    formatMessage += ex.StackTrace;

                    WriteToFile(PathLogError, formatMessage);
                    break;

                case TraceLevel.Warning:
                case TraceLevel.Verbose:
                case TraceLevel.Info:
                    // Aplicando particularidades para cada severidad...
                    WriteToFile(PathLogInfo, FormatMessage(log));
                    break;
            }
        }

        /// <summary>
        /// Formatea el mensaje de log con la fecha, nivel de traza y mensaje.
        /// </summary>
        /// <param name="log">El objeto log que contiene la información a formatear.</param>
        /// <returns>Un string con el mensaje formateado.</returns>
        private string FormatMessage(Log log)
        {
            return $"{log.Date.ToString("dd/MM/yyyy HH:mm:ss")} [{log.TraceLevel}] : {log.Message}";
        }

        /// <summary>
        /// Escribe el mensaje en el archivo especificado, con la opción de añadir contenido al archivo existente.
        /// </summary>
        /// <param name="path">Ruta del archivo donde se escribirá el log.</param>
        /// <param name="message">El mensaje a escribir en el archivo.</param>
        private void WriteToFile(string path, string message)
        {
            // Definir política de depuración de logs (Corte)
            path = DateTime.Now.ToString("dd-MM-yyyy") + path;

            using (StreamWriter str = new StreamWriter(path, true))
            {
                str.WriteLine(message);
            }
        }
    }
}
