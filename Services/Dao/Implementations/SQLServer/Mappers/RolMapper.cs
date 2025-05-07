using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dao.Implementations.SQLServer.Mappers
{
    /// <summary>
    /// Clase estática encargada de mapear los resultados de consultas SQL a objetos del tipo Rol.
    /// </summary>
    internal static class RolMapper
    {
        /// <summary>
        /// Mapea un arreglo de objetos (generalmente los valores obtenidos de una consulta SQL) a una instancia de la clase Rol.
        /// </summary>
        /// <param name="values">Arreglo de objetos que contiene los valores de las columnas obtenidos de la base de datos.</param>
        /// <returns>Una instancia de Rol con los valores mapeados desde la base de datos.</returns>
        public static Rol Map(object[] values)
        {
            return new Rol()
            {
                Id = Guid.Parse($"{values[(int)RolColumns.Id]}"),
                Nombre = (string)values[(int)RolColumns.Nombre],
                Descripcion = values[(int)RolColumns.Descripcion] == DBNull.Value
                                                                      ? null
                                                                      : (string)values[(int)RolColumns.Descripcion]

            };
        }
    }

    /// <summary>
    /// Enum que define los índices de las columnas en la base de datos correspondientes a las propiedades del objeto Rol.
    /// </summary>
    internal enum RolColumns
    {
        /// <summary>
        /// Índice de la columna que representa la Descripción del rol.
        /// </summary>
        Descripcion = 0,

        /// <summary>
        /// Índice de la columna que representa el Id del rol.
        /// </summary>
        Id = 1,

        /// <summary>
        /// Índice de la columna que representa el Nombre del rol.
        /// </summary>
        Nombre = 2
    }
}
