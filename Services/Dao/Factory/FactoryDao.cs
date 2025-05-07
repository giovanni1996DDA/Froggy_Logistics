using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Dao.Interfaces.UnitOfWork;
using System.Runtime.Remoting.Contexts;

namespace Services.Dao.Factory
{
    /// <summary>
    /// Clase estática encargada de crear y gestionar la instancia de la unidad de trabajo (UnitOfWork) según el tipo de backend configurado.
    /// </summary>
    public static class FactoryDao
    {
        /// <summary>
        /// Constructor estático que inicializa la instancia de UnitOfWork basada en la configuración del tipo de backend.
        /// Lee el valor de "BackendType" del archivo de configuración y crea la instancia correspondiente.
        /// </summary>
        static FactoryDao()
        {
            int backTypeValue = int.Parse(ConfigurationManager.AppSettings["BackendType"]);

            string backTypeName = Enum.GetName(typeof(BackendType), backTypeValue);

            string unitOfWorkString = $"Services.Dao.Implementations.SQLServer.UnitOfWork.UnitOfWork{backTypeName}";

            Type UnitOfWorkType = Type.GetType(unitOfWorkString);

            // Crea la instancia del tipo de repositorio correspondiente al backend seleccionado.
            var UnitOfWorkInstance = Activator.CreateInstance(UnitOfWorkType) as IUnitOfWork;

            UnitOfWork = UnitOfWorkInstance;
        }

        /// <summary>
        /// Propiedad que expone la instancia de UnitOfWork, la cual es creada en el constructor estático.
        /// </summary>
        public static IUnitOfWork UnitOfWork { get; private set; }
    }

    /// <summary>
    /// Enum que define los posibles tipos de backend que se pueden utilizar en la aplicación.
    /// </summary>
    internal enum BackendType
    {
        /// <summary>
        /// Backend en memoria (Memory).
        /// </summary>
        Memory,

        /// <summary>
        /// Backend en SQL Server.
        /// </summary>
        SQLServer,

        /// <summary>
        /// Backend en SQLite.
        /// </summary>
        Sqlite,

        /// <summary>
        /// Backend en Oracle.
        /// </summary>
        Oracle
    }
}
