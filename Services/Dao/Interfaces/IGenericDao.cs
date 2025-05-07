using Services.Dao.Implementations.SQLServer;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dao.Interfaces
{
    public interface IGenericDao<T>
    {
        /// <summary>
        /// Crea un nuevo registro en la base de datos para la entidad especificada.
        /// </summary>
        /// <param name="entity">La entidad de tipo <typeparamref name="T"/> que se va a crear.</param>
        void Create(T entity);

        /// <summary>
        /// Obtiene los registros de la base de datos en base a los filtros especificados.
        /// Devuelve una lista de objetos de tipo <typeparamref name="T"/>.
        /// </summary>
        /// <param name="entity">La entidad prototipo por la cual se realizará la búsqueda.</param>
        /// <param name="whereCallback">Función opcional para filtrar las propiedades de la entidad en la búsqueda. Si no se especifica, se consideran todas las propiedades.</param>
        /// <returns>Una lista de objetos de tipo <typeparamref name="T"/>.</returns>
        List<T> Get(List<FilterProperty> filters = null);

        /// <summary>
        /// Obtiene un solo registro de la base de datos en base a los filtros especificados.
        /// Devuelve una lista de objetos de tipo <typeparamref name="T"/>.
        /// </summary>
        /// <param name="entity">La entidad prototipo por la cual se realizará la búsqueda.</param>
        /// <param name="whereCallback">Función opcional para filtrar las propiedades de la entidad en la búsqueda. Si no se especifica, se consideran todas las propiedades.</param>
        /// <returns>Una lista de objetos de tipo <typeparamref name="T"/>.</returns>
        T GetOne(List<FilterProperty> filters = null);

        /// <summary>
        /// Verifica si existe una entidad de tipo <typeparamref name="T"/> en la base de datos en base a los filtros especificados.
        /// </summary>
        /// <param name="entity">La entidad prototipo por la cual se realizará la verificación.</param>
        /// <param name="whereCallback">Función opcional para filtrar las propiedades de la entidad en la verificación. Si no se especifica, se consideran todas las propiedades.</param>
        /// <returns>Verdadero si la entidad existe; de lo contrario, falso.</returns>
        bool Exists(List<FilterProperty> filters = null);
        //bool ExistsNonKeys(T entity, List<FilterProperty> filters = null);

        /// <summary>
        /// Actualiza un registro en la base de datos para la entidad especificada.
        /// </summary>
        /// <param name="entity">La entidad de tipo <typeparamref name="T"/> que se va a actualizar.</param>
        /// <param name="whereCallback">Función opcional para filtrar las propiedades de la entidad en la actualización.</param>
        void Update(List<FilterProperty> filters = null);

        /// <summary>
        /// Elimina un registro de la base de datos para la entidad especificada.
        /// </summary>
        /// <param name="entity">La entidad de tipo <typeparamref name="T"/> que se va a eliminar.</param>
        /// <param name="whereCallback">Función opcional para filtrar las propiedades de la entidad en la eliminación.</param>
        void Delete(List<FilterProperty> filters = null);
    }
}
