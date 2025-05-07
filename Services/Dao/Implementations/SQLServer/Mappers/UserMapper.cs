using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dao.Implementations.SQLServer.Mappers
{
    /// <summary>
    /// Clase estática encargada de mapear los resultados de consultas SQL a objetos del tipo User.
    /// </summary>
    internal static class UserMapper
    {
        /// <summary>
        /// Mapea un arreglo de objetos (generalmente los valores obtenidos de una consulta SQL) a una instancia de la clase User.
        /// </summary>
        /// <param name="values">Arreglo de objetos que contiene los valores de las columnas obtenidos de la base de datos.</param>
        /// <returns>Una instancia de User con los valores mapeados desde la base de datos.</returns>
        public static User Map(object[] values)
        {
            return new User()
            {
                Id = Guid.Parse($"{values[(int)UserColumns.Id]}"),
                UserName = (string)values[(int)UserColumns.UserName],
                Password = (string)values[(int)UserColumns.Password],
                Nombre = (string)values[(int)UserColumns.Nombre],
                Apellido = (string)values[(int)UserColumns.Apellido],
                Email = (string)values[(int)UserColumns.Email]
            };
        }
    }

    /// <summary>
    /// Enum que define los índices de las columnas en la base de datos correspondientes a las propiedades del objeto User.
    /// </summary>
    internal enum UserColumns
    {
        /// <summary>
        /// Índice de la columna que representa el Id del usuario.
        /// </summary>
        Id = 0,

        /// <summary>
        /// Índice de la columna que representa el nombre de usuario.
        /// </summary>
        UserName = 1,

        /// <summary>
        /// Índice de la columna que representa la contraseña del usuario.
        /// </summary>
        Password = 2,

        /// <summary>
        /// Índice de la columna que representa el nombre del usuario.
        /// </summary>
        Nombre = 3,

        /// <summary>
        /// Índice de la columna que representa el apellido del usuario.
        /// </summary>
        Apellido = 4,

        /// <summary>
        /// Índice de la columna que representa el email del usuario.
        /// </summary>
        Email = 5,
    }
}
