using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.HelperFunctions
{
    public static class CloneFunctions
    {
        public static T CloneObject<T>(T input) where T : class, new()
        {
            var inputType = input.GetType();
            var output = new T();

            var fields = inputType.GetFields().Where(x => x.Name != "_entityWrapper");
            var colFields = fields.Where(x => x.FieldType != typeof(string) && GetEnumerableType(x.FieldType) != null);
            var simpleFields =
                fields.Where(x => x.FieldType == typeof(string) || GetEnumerableType(x.FieldType) == null);

            var readWriteProps = inputType.GetProperties().Where(x => x.CanRead && x.CanWrite);
            var colProps = readWriteProps.Where(x => GetEnumerableType(x.PropertyType) != null && x.PropertyType != typeof(string));
            var simpleProps = readWriteProps.Where(x => x.PropertyType == typeof(string) || GetEnumerableType(x.PropertyType) == null);

            foreach (var field in simpleFields)
            {
                field.SetValue(output, field.GetValue(input));
            }

            foreach (var colField in colFields)
            {
                colField.SetValue(output, CloneCollection(colField.GetValue(input) as IEnumerable<object>));
            }

            foreach (var prop in simpleProps)
            {
                prop.SetValue(output, prop.GetValue(input));
            }

            foreach (var colProp in colProps)
            {
                colProp.SetValue(output, CloneCollection(colProp.GetValue(input) as IEnumerable<object>));
            }

            return output;
        }

        public static IEnumerable<T> CloneCollection<T>(IEnumerable<T> input) where T : class, new()
        {
            var output = new List<T>();

            foreach (var item in input)
            {
                output.Add(CloneObject(item));
            }

            return output;
        }

        public static Type GetEnumerableType(Type type)
        {
            return (from intType in type.GetInterfaces()
                where intType.IsGenericType && intType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                select intType.GetGenericArguments()[0]).FirstOrDefault();
        }
    }
}
