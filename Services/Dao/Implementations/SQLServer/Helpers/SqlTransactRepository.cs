using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Specialized;
using System.Configuration;
using Services.Domain;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Services.Dao.Implementations.SQLServer;
using Services.Dao.Interfaces;
using Services.Dao.Implementations.SQLServer.Mappers;
using System.ComponentModel.DataAnnotations;
using Services.Logic.Exceptions;

namespace Services.Dao.Implemenations.SQLServer.Helpers
{
    /// <summary>
    /// Clase abstracta que proporciona una implementación base para la gestión de transacciones SQL para entidades genéricas.
    /// Implementa las operaciones CRUD usando ADO.NET.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad para la que se implementan las operaciones CRUD.</typeparam>
    public abstract class SqlTransactRepository<T> : IGenericDao<T>
    {
        /// <summary>
        /// Conexión de SQL utilizada para realizar operaciones de la base de datos.
        /// </summary>
        protected SqlConnection _context;

        /// <summary>
        /// Transacción SQL activa.
        /// </summary>
        protected SqlTransaction _transaction;

        /// <summary>
        /// Nombre de la tabla en la base de datos asociada a la entidad.
        /// </summary>
        protected string _TableName;

        /// <summary>
        /// Lista de las propiedades de la entidad T que se utilizan para construir las consultas SQL.
        /// </summary>
        protected List<PropertyInfo> Props = new List<PropertyInfo>();

        #region Statements
        protected string InsertStatement => $"INSERT INTO {_TableName}";
        protected string UpdateStatement => $"UPDATE {_TableName}";
        protected string DeleteStatement => $"DELETE FROM {_TableName}";
        protected string SelectStatement => $"SELECT {QueryBuilder.BuildColumnNames<T>()} FROM {_TableName}";
        protected string SelectSingleStatement => $"SELECT TOP 1 {QueryBuilder.BuildColumnNames<T>()} FROM {_TableName}";
        #endregion

        /// <summary>
        /// Constructor que inicializa el repositorio con la conexión SQL y transacción activa.
        /// </summary>
        /// <param name="context">Conexión SQL.</param>
        /// <param name="_transaction">Transacción SQL.</param>
        /// <param name="ExcludedProps">Lista de propiedades excluidas para las consultas SQL.</param>
        public SqlTransactRepository(SqlConnection context, SqlTransaction _transaction, List<string> ExcludedProps = null)
        {
            this._context = context;
            this._transaction = _transaction;

            this._TableName = $"[Dbo].[{typeof(T).Name}]";

            ExcludedProps = ExcludedProps ?? new List<string>();

            Props = typeof(T).GetProperties().Where(prop => !ExcludedProps.Contains(prop.Name)).ToList();
        }

        /// <summary>
        /// Ejecuta una consulta de no retorno (INSERT, UPDATE, DELETE) en la base de datos.
        /// </summary>
        /// <param name="commandText">Texto del comando SQL.</param>
        /// <param name="commandType">Tipo de comando SQL (texto o procedimiento almacenado).</param>
        /// <param name="parameters">Parámetros del comando SQL.</param>
        /// <returns>Número de filas afectadas por la consulta.</returns>
        protected Int32 ExecuteNonQuery(String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            CheckNullables(parameters);

            using (var cmd = CreateCommand(commandText))
            {
                //AGREGADO
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                //AGREGADO

                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Valida los parámetros y reemplaza valores nulos por DBNull.
        /// </summary>
        /// <param name="parameters">Arreglo de parámetros SQL.</param>
        private void CheckNullables(SqlParameter[] parameters)
        {
            foreach (SqlParameter item in parameters)
            {
                if (item.SqlValue == null)
                {
                    item.SqlValue = DBNull.Value;
                }
            }
        }

        /// <summary>
        /// Ejecuta una consulta de retorno escalar (SELECT con una sola columna o valor).
        /// </summary>
        /// <param name="commandText">Texto del comando SQL.</param>
        /// <param name="commandType">Tipo de comando SQL (texto o procedimiento almacenado).</param>
        /// <param name="parameters">Parámetros del comando SQL.</param>
        /// <returns>Objeto que representa el valor escalar devuelto.</returns>
        protected Object ExecuteScalar(String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            using (var cmd = CreateCommand(commandText))
            {
                cmd.CommandType = commandType;

                cmd.Parameters.AddRange(parameters);

                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Ejecuta una consulta de retorno múltiple (SELECT) y devuelve un SqlDataReader.
        /// </summary>
        /// <param name="commandText">Texto del comando SQL.</param>
        /// <param name="commandType">Tipo de comando SQL (texto o procedimiento almacenado).</param>
        /// <param name="parameters">Parámetros del comando SQL.</param>
        /// <returns>Un SqlDataReader que contiene los resultados de la consulta.</returns>
        protected SqlDataReader ExecuteReader(String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            using (var cmd = CreateCommand(commandText))
            {
                cmd.CommandType = commandType;

                cmd.Parameters.AddRange(parameters);
                // When using CommandBehavior.CloseConnection, the connection will be closed when the 
                // IDataReader is closed.
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
        }

        /// <summary>
        /// Inserta un nuevo registro en la base de datos.
        /// </summary>
        /// <param name="entity">Entidad a insertar.</param>
        public virtual void Create(T entity)
        {
            string columnNames = QueryBuilder.BuildColumnNames<T>();
            string paramNames = QueryBuilder.BuildParamNames<T>();

            string insertStatement = $"{InsertStatement} ({columnNames}) VALUES ({paramNames})";

            SqlParameter[] parameters = QueryBuilder.BuildParams<T>(entity);

            ExecuteNonQuery(insertStatement, CommandType.Text, parameters);
        }

        /// <summary>
        /// Obtiene una lista de registros que cumplen con los criterios de la entidad de ejemplo.
        /// </summary>
        /// <param name="entity">Entidad de ejemplo para aplicar filtros.</param>
        /// <param name="whereCallback">Callback opcional para personalizar los filtros de búsqueda.</param>
        /// <returns>Lista de entidades que coinciden con los criterios.</returns>
        public virtual List<T> Get(List<FilterProperty> filters )
        {
            // Construir la cláusula WHERE basada en los filtros
            string whereClause = QueryBuilder.BuildWhere<T>(filters);

            // Construir los parámetros para las propiedades válidas
            SqlParameter[] parameters = QueryBuilder.BuildParams<T>(filters);

            // Construir la consulta SELECT completa
            string selectStatement = $"{SelectStatement} {whereClause}";

            // Ejecutar la consulta y devolver los resultados
            return ExecuteSelectQuery(selectStatement, parameters);
        }

        /// <summary>
        /// Obtiene un solo registro que cumple con los criterios de la entidad de ejemplo.
        /// </summary>
        /// <param name="entity">Entidad de ejemplo para aplicar filtros.</param>
        /// <param name="whereCallback">Callback opcional para personalizar los filtros de búsqueda.</param>
        /// <returns>Una entidad que coincide con los criterios.</returns>
        public virtual T GetOne(List<FilterProperty> filters = null)
        {
            //if (filters == null)
            //    filters = BuildFilters(entity);

            //string whereClause = QueryBuilder.BuildWhere<T>(filters);
            //SqlParameter[] parameters = QueryBuilder.BuildParams(filters);
            //string selectStatement = $"{SelectSingleStatement} {whereClause}";

            List<T> returning = Get(filters);

            if (returning.Count == 0)
                return default;

            return returning.FirstOrDefault();
        }

        /// <summary>
        /// Verifica si existe un registro que cumple con los criterios de la entidad de ejemplo.
        /// </summary>
        /// <param name="entity">Entidad de ejemplo para aplicar filtros.</param>
        /// <param name="whereCallback">Callback opcional para personalizar los filtros de búsqueda.</param>
        /// <returns>True si el registro existe, false en caso contrario.</returns>
        public virtual bool Exists(List<FilterProperty> filters = null)
        {
            string whereClause = QueryBuilder.BuildWhere<T>(filters, includeNonKeys: false);
            SqlParameter[] parameters = QueryBuilder.BuildParams<T>(filters);
            string selectStatement = $"{SelectSingleStatement} {whereClause}";

            return ExecuteScalar(selectStatement, CommandType.Text, parameters) != null;
        }
        //public virtual bool ExistsNonKeys(T entity, List<FilterProperty> filters = null)
        //{
        //    if (filters == null)
        //        filters = BuildFilters(entity,includeKeys: false, includeNonKeys: true);

        //    string whereClause = QueryBuilder.BuildWhere<T>(filters,includeKeys:false, includeNonKeys: true);
        //    SqlParameter[] parameters = QueryBuilder.BuildParams<T>(filters, includeKeys:false, includeNonKeys:true);
        //    string selectStatement = $"{SelectSingleStatement} {whereClause}";

        //    return ExecuteScalar(selectStatement, CommandType.Text, parameters) != null;
        //}

        /// <summary>
        /// Actualiza un registro existente en la base de datos.
        /// </summary>
        /// <param name="entity">Entidad a actualizar.</param>
        /// <param name="whereCallback">Callback opcional para personalizar los filtros de búsqueda.</param>
        public virtual void Update(List<FilterProperty> filters = null)
        {
            string setClause = QueryBuilder.BuildSet<T>(filters);
            string whereClause = QueryBuilder.BuildWhere<T>(filters, includeKeys:true, includeNonKeys:false);
            SqlParameter[] parameters = QueryBuilder.BuildParams<T>(filters, includeKeys: true, includeNonKeys: true);

            string updateStatement = $"{UpdateStatement} {setClause} {whereClause}";
            ExecuteNonQuery(updateStatement, CommandType.Text, parameters);
        }

        /// <summary>
        /// Elimina un registro de la base de datos.
        /// </summary>
        /// <param name="entity">Entidad a eliminar.</param>
        /// <param name="whereCallback">Callback opcional para personalizar los filtros de búsqueda.</param>
        public virtual void Delete(List<FilterProperty> filters = null)
        {
            string whereClause = QueryBuilder.BuildWhere<T>(filters);
            SqlParameter[] parameters = QueryBuilder.BuildParams<T>(filters);

            string deleteStatement = $"{DeleteStatement} {whereClause}";
            ExecuteNonQuery(deleteStatement, CommandType.Text, parameters);
        }
        private List<T> ExecuteSelectQuery(string commandText, SqlParameter[] parameters)
        {
            List<T> results = new List<T>();

            using (var reader = ExecuteReader(commandText, CommandType.Text, parameters))
            {
                while (reader.Read())
                {
                    object[] data = new object[reader.FieldCount];
                    reader.GetValues(data);

                    Type mapperType = Type.GetType($"{this.GetType().Namespace}.Mappers.{typeof(T).Name}Mapper");
                    MethodInfo mapperMethod = mapperType.GetMethod("Map", BindingFlags.Static | BindingFlags.Public);

                    results.Add((T)mapperMethod.Invoke(null, new object[] { data }));
                }
            }
            return results;
        }
        private List<FilterProperty> BuildFilters(T entity, bool includeKeys = true, bool includeNonKeys = true)
        {
            var filters = new List<FilterProperty>();

            // Obtener todas las propiedades de la clase T
            var props = QueryBuilder.GetProperties(typeof(T));

            foreach (var prop in props)
            {
                bool isKey = Attribute.IsDefined(prop, typeof(KeyAttribute));

                // Filtrar según los parámetros includeKeys y includeNonKeys
                if ((isKey && !includeKeys) || (!isKey && !includeNonKeys))
                    continue;

                var value = prop.GetValue(entity);

                // Incluir Guid.Empty como valor válido
                if (value == null)
                    continue;

                // Agregar la propiedad al filtro
                filters.Add(new FilterProperty(prop.Name, value, FilterOperation.Equals));
            }

            return filters;
        }


        /// <summary>
        /// Crea un comando SQL a partir de la consulta proporcionada.
        /// </summary>
        /// <param name="query">Consulta SQL.</param>
        /// <returns>Objeto SqlCommand listo para ejecutar.</returns>
        private SqlCommand CreateCommand(string query)
        {
            return new SqlCommand(query, _context, _transaction);
        }
    }
}
