using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace UnitTestBase
{
    public static class TestHelpersBase
    {
        public static object GetPrivateField(this object obj, string name)
        {
            var fieldInfo = obj.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null) return fieldInfo.GetValue(obj);
            return null;
        }

        public static object GetPrivateProperty(this object obj, string name)
        {
            var propertyInfo = obj.GetType().GetProperty(name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (propertyInfo != null) return propertyInfo.GetValue(obj, null);
            return null;
        }

        public static int GetFieldCountMarkedWithAttribute(Type targetType, Type attributeType)
        {
            var fields = targetType
                .GetProperties(System.Reflection.BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(f => f
                    .GetCustomAttributes(attributeType, false).FirstOrDefault() != null);
            return fields.Count();
        }

        public static int GetRequiredFieldCountWithJsonAttribute(Type targetType, string propertyName)
        {
            var fields = targetType
                .GetProperties(System.Reflection.BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(f => f.GetCustomAttributes(typeof(JsonPropertyAttribute), false).FirstOrDefault() != null);
            return fields.Count(f=> CheckJsonAttributeName(f, propertyName));
        }

        private static bool CheckJsonAttributeName(PropertyInfo prop, string propertyName)
        {
            var attr = prop.GetCustomAttribute<JsonPropertyAttribute>(true);
            if(attr.PropertyName.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase))
            { 
                return true;
            }
            return false;
        }
    }
}
