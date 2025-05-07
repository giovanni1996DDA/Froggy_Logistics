using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dao.Implementations.SQLServer.Mappers
{
    /// <summary>
    /// Clase encargada de mapear los resultados de consultas SQL a objetos del tipo RolRolRelation.
    /// </summary>
    internal class RolRolRelationMapper
    {
        /// <summary>
        /// Mapea un arreglo de objetos (generalmente los valores obtenidos de una consulta SQL) a una instancia de la clase RolRolRelation.
        /// </summary>
        /// <param name="values">Arreglo de objetos que contiene los valores de las columnas obtenidos de la base de datos.</param>
        /// <returns>Una instancia de RolRolRelation con los valores mapeados desde la base de datos.</returns>
        public static RolRolRelation Map(object[] values)
        {
            return new RolRolRelation()
            {
                FatherId = Guid.Parse($"{values[(int)RolRolColumns.FatherId]}"),
                ChildId = Guid.Parse($"{values[(int)RolRolColumns.ChildId]}"),
            };
        }
    }

    /// <summary>
    /// Enum que define los índices de las columnas en la base de datos correspondientes a las propiedades del objeto RolRolRelation.
    /// </summary>
    internal enum RolRolColumns
    {
        /// <summary>
        /// Índice de la columna que representa el Id del Rol padre.
        /// </summary>
        FatherId = 0,

        /// <summary>
        /// Índice de la columna que representa el Id del Rol hijo.
        /// </summary>
        ChildId = 1,
    }
}
