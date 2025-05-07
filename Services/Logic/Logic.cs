using Services.Dao.Implementations.SQLServer;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logic
{
    public abstract class Logic<T>
    {
        public List<FilterProperty> BuildFilters(T obj)
        {
            List<FilterProperty> filters = new List<FilterProperty>();

            obj.GetType().GetProperties().ToList().ForEach(p =>
            {
                if (p.GetValue(obj) == null)
                    return;

                if (Attribute.IsDefined(p, typeof(NotMappedAttribute)))
                    return;

                filters.Add(
                                new FilterProperty()
                                {
                                    Operation = FilterOperation.Equals,
                                    PropertyName = p.Name,
                                    Value = p.GetValue(obj)
                                }
                            );
            });
            return filters;
        }
    }
}
