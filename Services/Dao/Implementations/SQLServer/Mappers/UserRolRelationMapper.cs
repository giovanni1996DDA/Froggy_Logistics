using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dao.Implementations.SQLServer.Mappers
{
    /// <summary>
    /// Clase estática encargada de mapear los resultados de consultas SQL a objetos del tipo UserRolRelation.
    /// </summary>
    internal static class UserRolRelationMapper
    {
        /// <summary>
        /// Mapea un arreglo de objetos (generalmente los valores obtenidos de una consulta SQL) a una instancia de la clase UserRolRelation.
        /// </summary>
        /// <param name="values">Arreglo de objetos que contiene los valores de las columnas obtenidos de la base de datos.</param>
        /// <returns>Una instancia de UserRolRelation con los valores mapeados desde la base de datos.</returns>
        public static UserRolRelation Map(object[] values)
        {
            return new UserRolRelation()
            {
                IdUser = Guid.Parse($"{values[(int)UserRolColumns.IdUser]}"),
                IdRol = Guid.Parse($"{values[(int)UserRolColumns.IdRol]}"),
            };
        }
    }

    /// <summary>
    /// Enum que define los índices de las columnas en la base de datos correspondientes a las propiedades del objeto UserRolRelation.
    /// </summary>
    internal enum UserRolColumns
    {
        /// <summary>
        /// Índice de la columna que representa el Id del usuario.
        /// </summary>
        IdUser = 0,

        /// <summary>
        /// Índice de la columna que representa el Id del rol.
        /// </summary>
        IdRol = 1,
    }
}
