using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ARK.Model.Search
{
    public abstract class Searchable<T> : ISearchable<T>
    {
        public abstract T getTarget();

        public bool SearchMatching(params Func<T, bool>[] filters)
        {
            foreach (var filter in filters)
            {
                if (filter(getTarget()))
                {
                    return true;
                }
            }

            return false;
        }

        public bool SearchMatching(string searchExpression)
        {
            T target = getTarget();

            // Indlæs alle properties
            PropertyInfo[] properties = target.GetType().GetProperties();
            Dictionary<PropertyInfo, string> propertiesInfo = new Dictionary<PropertyInfo, string>();
            foreach (var prop in properties)
            {
                propertiesInfo.Add(prop, prop.GetValue(target).ToString());
            }

            // Finder expression
            Expression expression = TranslateSearchexpression(searchExpression);
            
            // HÅndter søgning
            return SearchMatching(expression, target, propertiesInfo);
        }

        private bool SearchMatching(Expression expression, T target, Dictionary<PropertyInfo, string> properties)
        {
            bool matching = expression.Type == ExpressionType.and;

            foreach (var element in expression.Input)
            {
                if (properties.Where(e => e.Value.Contains(element)).Count() > 0)
                {
                    if (expression.Type == ExpressionType.or)
                    {
                        matching = true;
                    }
                }
                else if (expression.Type == ExpressionType.and)
                {
                    matching = false;
                }
            }

            if (matching && expression.Expressions.Count() > 0)
            {
                foreach (var expr in expression.Expressions)
                {
                    if (SearchMatching(expression, target, properties))
                    {
                        if (expression.Type == ExpressionType.or)
                        {
                            matching = true;
                        }
                    }
                    else if (expression.Type == ExpressionType.and)
                    {
                        matching = false;
                    }
                }
            }
            
            return matching;
        }

        private Expression TranslateSearchexpression(string expression)
        {
            return TranslateSearchexpression(expression.Split(' '), ExpressionType.and);
        }

        private Expression TranslateSearchexpression(string[] expression, ExpressionType exType)
        {
            // Tjek expression
            if (expression == null || expression.Length == 0)
            {
                return null;
            }

            // Analyser searchstring
            List<string> input = new List<string>();
            for (int i = 0; i < expression.Count(); i++)
            {
                // Ignore and
                if (expression[i].ToLower() == "and") { }
                // Håndter or
                else if (expression[i].ToLower() == "or")
                {
                    // Tjek at or ikke var første keyword - ignorer or hvis det er første keyword
                    if (i != 0) { 
                        // Fjern forrige keyword
                        string last = input.Last();
                        input.RemoveAt(input.Count - 1);

                        return new Expression()
                        {
                            Input = input.ToArray(),
                            Type = exType,
                            Expressions = new Expression[] { 
                                TranslateSearchexpression(new string[] {
                                    last, expression[i]
                                }, ExpressionType.or),
                                TranslateSearchexpression(
                                    SubArray<string>(expression, i + 1),
                                    ExpressionType.and
                                )
                            }
                        };
                    }
                }

            }

            // Opret vores expression
            Expression output = new Expression()
            {
                Input = input.ToArray(),
                Type = exType
            };
            return output;
        }

        public static T[] SubArray<T>(T[] data, int index)
        {
            int length = data.Length - index;

            // Tjek længden
            if (length < 1)
            {
                return null;
            }

            // Kopir array
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        private class Expression
        {
            public string[] Input { get; set; }
            public Expression[] Expressions { get; set; }

            public ExpressionType Type { get; set; }
        }

        private enum ExpressionType { and, or }
    }
}
