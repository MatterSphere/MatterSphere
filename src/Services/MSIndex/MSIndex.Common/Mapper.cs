using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using MSIndex.Common.Interfaces;

namespace MSIndex.Common
{
    public class Mapper : IMapper
    {
        public void Map<T>(ExpandoObject source, T destination) where T : class
        {
            var destinationPropertyList = destination.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite && p.GetSetMethod() != null).ToList();
            var destinationProperties = new Dictionary<string, PropertyInfo>();
            foreach (var destinationPropertyItem in destinationPropertyList)
            {
                try
                {
                    var key = GetMapKey(destinationPropertyItem);
                    destinationProperties.Add(key, destinationPropertyItem);
                }
                catch (CustomAttributeWasNotFoundException)
                {
                    
                }
            }

            var sourceProperties = source as IDictionary<string, object>;
            foreach (var sourceProperty in sourceProperties)
            {
                PropertyInfo propertyInfo;
                if (destinationProperties.TryGetValue(sourceProperty.Key, out propertyInfo))
                {
                    if (propertyInfo.PropertyType == typeof(bool))
                    {
                        bool boolValue = Convert.ToBoolean(Convert.ChangeType(sourceProperty.Value.ToString(), typeof(uint)));
                        propertyInfo.SetValue(destination, boolValue);
                    }
                    else
                    {
                        var value = Convert.ChangeType(sourceProperty.Value, propertyInfo.PropertyType);
                        propertyInfo.SetValue(destination, value);
                    }
                }
            }
        }

        private string GetMapKey(PropertyInfo propertyInfo)
        {
            object[] customAttributes = propertyInfo.GetCustomAttributes(true);
            foreach (object customAttribute in customAttributes)
            {
                MapKeyAttribute mapKeyAttribute = customAttribute as MapKeyAttribute;
                if (mapKeyAttribute != null)
                {
                    return mapKeyAttribute.Key;
                }
            }

            throw new CustomAttributeWasNotFoundException("Custom attribute MapKeyAttribute was not found");
        }
    }
}
