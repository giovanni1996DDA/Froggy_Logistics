using Services.Dao.Interfaces;
using Services.Dao.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dao.Implementations.SQLServer.UnitOfWork
{
    public class UnitOfWorkSQLServerRepository : IUnitOfWorkRepository
    {
        public IUserDao UserRepository { get; private set; }
        public IRolDao RolRepository { get; private set; }
        public IRolRolDao RolRolRepository { get; private set; }
        public IPermisoDao PermisoRepository { get; private set; }
        public IUserRolDao UserRolRepository { get; private set; }
        public IUserPermisoDao UserPermisoRepository { get; private set; }
        public IRolPermisoDao RolPermisoRepository { get; private set; }
        public LoggerDao LoggerRepository { get; private set; }

        public UnitOfWorkSQLServerRepository(SqlConnection context, SqlTransaction transaction)
        {
            //Levanto los repositorios configurados en el app.config
            NameValueCollection concreteRepositories = ConfigurationManager.GetSection("ConcreteRepositories") as NameValueCollection;

            foreach (var concreteRepositoryKey in concreteRepositories)
            {
                string repoName = concreteRepositories[$"{concreteRepositoryKey}"];
                //Obtengo el tipo del repo
                Type repoType = Type.GetType($"Services.Dao.Implementations.SQLServer.{repoName}");

                //creo la instancia del tipo de repo
                var repoInstance = Activator.CreateInstance(repoType, new object[]
                {
                context, transaction
                });

                //obtengo la propiedad de la instancia actual de la clase UnitOfWorkSqlServerRepository
                PropertyInfo property = this.GetType().GetProperty($"{concreteRepositoryKey}");

                //Verifico por las dudas que sea compatible y asigno
                if (property != null && property.PropertyType.IsAssignableFrom(repoType))
                {
                    property.SetValue(this, repoInstance);

                }
            }
        }
    }
}
