using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ARK.View.ValidationRules
{
    public class RegexExclusiveMatchRule : ValidationRule
    {
        public string Pattern { get; set; }

        public string ErrorMessage { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var result = new ValidationResult(true, null);

            if (Regex.IsMatch(value as string, Pattern))
            {
                result = new ValidationResult(false, ErrorMessage);
            }

            return result;
        }
    }
}
