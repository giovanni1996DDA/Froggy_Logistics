using Services.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Dao.Interfaces;
using System.Reflection;
using Services.Dao.Implementations.SQLServer.Mappers;
using Services.Dao.Implemenations.SQLServer.Helpers;

namespace Services.Dao.Implementations.SQLServer
{
    public class RolPermisoDao : SqlTransactRepository<RolPermisoRelation>, IRolPermisoDao
    {
        public RolPermisoDao(SqlConnection context, SqlTransaction _transaction) : base(context, _transaction)
        {
        }
    }
}
