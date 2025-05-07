using Services.Dao.Implemenations.SQLServer.Helpers;
using Services.Dao.Interfaces;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dao.Implementations.SQLServer
{
    internal class UserDao : SqlTransactRepository<User>, IUserDao 
    {
        public UserDao(SqlConnection context, SqlTransaction _transaction) : base(context, _transaction)
        {
        }
    }
}
