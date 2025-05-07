using Services.Dao.Interfaces;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Services.Dao.Implementations.SQLServer.Mappers;
using Services.Dao.Implemenations.SQLServer.Helpers;

namespace Services.Dao.Implementations.SQLServer
{
    internal class UserPermisoDao : SqlTransactRepository<UserPermisoRelation>, IUserPermisoDao
    {
        public UserPermisoDao(SqlConnection context, SqlTransaction _transaction) : base(context, _transaction)
        {
        }
    }
}
