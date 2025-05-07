using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dao.Implementations.SQLServer.Mappers
{
    /// <summary>
    /// Clase encargada de mapear los resultados de consultas SQL a objetos del tipo Permiso.
    /// </summary>
    internal class PermisoMapper
    {
        /// <summary>
        /// Mapea un arreglo de objetos (generalmente los valores obtenidos de una consulta SQL) a una instancia de la clase Permiso.
        /// </summary>
        /// <param name="values">Arreglo de objetos que contiene los valores de las columnas obtenidos de la base de datos.</param>
        /// <returns>Una instancia de Permiso con los valores mapeados desde la base de datos.</returns>
        public static Permiso Map(object[] values)
        {
            return new Permiso()
            {
                Id = Guid.Parse($"{values[(int)PermisoColumns.Id]}"),
                Nombre = (string)values[(int)PermisoColumns.Nombre],
                TipoPermiso = (int)values[(int)PermisoColumns.TipoPermiso]
            };
        }
    }

    /// <summary>
    /// Enum que define los índices de las columnas en la base de datos correspondientes a las propiedades del objeto Permiso.
    /// </summary>
    internal enum PermisoColumns
    {
        /// <summary>
        /// Índice de la columna que representa el Modulo.
        /// </summary>
        Modulo = 0,

        /// <summary>
        /// Índice de la columna que representa el Nombre del permiso.
        /// </summary>
        TipoPermiso = 1,
        /// <summary>
        /// Índice de la columna que representa el Id del permiso.
        /// </summary>
        Id = 2,
        /// <summary>
        /// Índice de la columna que representa el Nombre del permiso.
        /// </summary>
        Nombre = 3

    }
}
