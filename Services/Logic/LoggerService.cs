using Services.Dao.Factory;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logic
{
    internal class LoggerService
    {
        public LoggerService() 
        { 
        }
        public void WriteLog(Log log, Exception ex = null)
        {
            if (log.TraceLevel == System.Diagnostics.TraceLevel.Error)
            {
                //Enviar mensaje vía wp a fulanito...
            }

            var conn = FactoryDao.UnitOfWork.Create();
            conn.Repositories.LoggerRepository.WriteLog(log, ex);
        }
    }
}
