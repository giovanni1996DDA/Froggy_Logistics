using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dao.Implementations.SQLServer
{
    public class FilterProperty
    {
        public string PropertyName { get; set; }  // Nombre de la propiedad (columna en la base de datos)
        public object Value { get; set; }         // Valor a comparar
        public FilterOperation Operation { get; set; }  // Tipo de operación (=, IN, LIKE, etc.)
        public FilterProperty()
        {

        }
        public FilterProperty(string propertyName, object value, FilterOperation operation = FilterOperation.Equals)
        {
            PropertyName = propertyName;
            Value = value;
            Operation = operation;
        }
    }

    public enum FilterOperation
    {
        Equals,     // Comparación "="
        NotEquals,  // Comparación "!="
        GreaterThan,  // Comparación ">"
        LessThan,     // Comparación "<"
        In,           // Comparación "IN"
        Like          // Comparación "LIKE"
    }
}
