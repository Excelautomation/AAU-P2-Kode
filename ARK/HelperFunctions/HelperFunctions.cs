using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ARK.HelperFunctions
{
    public static class HelperFunctions
    {
        public static T CloneObject<T>(T input) where T : class, new()
        {
            var output = new T();
            var inputType = typeof (T);

            var fields = inputType.GetFields();
            var props = inputType.GetProperties().Where(x => x.CanRead && x.CanWrite && x.GetAccessors().Length == 0);
            var colProps = inputType.GetProperties().Where(x => x.GetAccessors().Length != 0);

            foreach (var field in fields)
            {
                field.SetValue(output, field.GetValue(input));
            }

            foreach (var prop in props)
            {
                prop.SetValue(output, prop.GetValue(input));
            }

            foreach (var colProp in colProps)
            {
                //CloneCollection(colProp);
            }

            return output;
        }

        public static IEnumerable<T> CloneCollection<T>(IEnumerable<T> input) where T : class, new()
        {
            var output = new List<T>();

            foreach (var item in input)
            {
                output.Add(CloneObject<T>(item));
            }

            return output;
        }
    }
}
