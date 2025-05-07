using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dao.Implementations.SQLServer.Mappers
{
    /// <summary>
    /// Clase encargada de mapear los resultados de consultas SQL a objetos del tipo RolPermisoRelation.
    /// </summary>
    internal class RolPermisoRelationMapper
    {
        /// <summary>
        /// Mapea un arreglo de objetos (generalmente los valores obtenidos de una consulta SQL) a una instancia de la clase RolPermisoRelation.
        /// </summary>
        /// <param name="values">Arreglo de objetos que contiene los valores de las columnas obtenidos de la base de datos.</param>
        /// <returns>Una instancia de RolPermisoRelation con los valores mapeados desde la base de datos.</returns>
        public static RolPermisoRelation Map(object[] values)
        {
            return new RolPermisoRelation()
            {
                ID_Rol = Guid.Parse($"{values[(int)RolPermisoColumns.IdRol]}"),
                ID_Permiso = Guid.Parse($"{values[(int)RolPermisoColumns.IdPermiso]}"),
            };
        }
    }

    /// <summary>
    /// Enum que define los índices de las columnas en la base de datos correspondientes a las propiedades del objeto RolPermisoRelation.
    /// </summary>
    internal enum RolPermisoColumns
    {
        /// <summary>
        /// Índice de la columna que representa el Id del Rol.
        /// </summary>
        IdRol = 0,

        /// <summary>
        /// Índice de la columna que representa el Id del Permiso.
        /// </summary>
        IdPermiso = 1,
    }
}
