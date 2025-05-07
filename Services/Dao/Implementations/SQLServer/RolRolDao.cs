using Services.Domain;
using System.Data.SqlClient;
using Services.Dao.Interfaces;
using Services.Dao.Implemenations.SQLServer.Helpers;

namespace Services.Dao.Implementations.SQLServer
{
    internal class RolRolDao : SqlTransactRepository<RolRolRelation>, IRolRolDao
    {
        public RolRolDao(SqlConnection context, SqlTransaction _transaction) : base(context, _transaction)
        {
        }
    }
}
