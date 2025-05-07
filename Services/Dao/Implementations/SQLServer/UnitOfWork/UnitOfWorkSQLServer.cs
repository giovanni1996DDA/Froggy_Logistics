using Services.Dao.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dao.Implementations.SQLServer.UnitOfWork
{
    public class UnitOfWorkSQLServer : IUnitOfWork
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ServicesSqlConnection"].ConnectionString;

        public UnitOfWorkSQLServer()
        {
        }
        /// <summary>
        /// Crea la connection a la BBDD en cuestion
        /// </summary>
        /// <returns></returns>
        public IUnitOfWorkAdapter Create()
        {
            return new UnitOfWorkSQLServerAdapter(connectionString);
        }
    }
}
