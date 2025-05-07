using Services.Dao.Implemenations.SQLServer.Helpers;
using Services.Dao.Implementations.SQLServer.Mappers;
using Services.Dao.Interfaces;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dao.Implementations.SQLServer
{
    internal class RolDao : SqlTransactRepository<Rol>, IRolDao
    {
        private static List<string> excludedProps = new List<string>()
        {
            "HasChildren",
            "Accesos"
        };
        public RolDao(SqlConnection context, SqlTransaction _transaction) : base(context, _transaction, excludedProps)
        {
        }

        public override void Create(Rol entity)
        {
            base.Create(entity);
        }
    }
}
