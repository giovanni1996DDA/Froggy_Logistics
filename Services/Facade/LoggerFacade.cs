using Services.Domain;
using Services.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Facade
{
    public static class LoggerFacade
    {
        public static void WriteLog(Log log, Exception ex = null)
        {
            LoggerService loggerService = new LoggerService();
            loggerService.WriteLog(log, ex);
        }
        /// <summary>
        /// Si solo se usa la bitácora con motivos de información (DEFAULT = INFO)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public static void WriteLog(string message, TraceLevel level = TraceLevel.Info)
        {
            LoggerService loggerService = new LoggerService();
            loggerService.WriteLog(new Log(message, level));
        }
        public static void WriteException(Exception ex)
        {
            LoggerService loggerService = new LoggerService();
            loggerService.WriteLog(new Log(ex.Message, TraceLevel.Error), ex);
        }
    }
}
