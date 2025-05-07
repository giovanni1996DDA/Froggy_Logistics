using Services.Dao.Implemenations.SQLServer.Helpers;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dao.Implementations.SQLServer
{
    public class DocTypeDao : SqlTransactRepository<DocType>
    {
        public DocTypeDao(SqlConnection context, SqlTransaction _transaction) : base(context, _transaction)
        {
        }
    }
}
