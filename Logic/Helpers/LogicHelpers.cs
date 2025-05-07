using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Helpers
{
    public static class LogicHelpers
    {
        public static void SetGuidEmptyToNull(object instance)
        {
            var properties = instance.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(Guid?) || property.PropertyType == typeof(Guid))
                {
                    var value = property.GetValue(instance);

                    if (value != null && (Guid)value == Guid.Empty)
                    {
                        if (property.PropertyType == typeof(Guid?))
                        {
                            property.SetValue(instance, null);
                        }
                    }
                }
            }
        }
    }
}
