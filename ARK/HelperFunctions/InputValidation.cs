using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ARK.HelperFunctions
{
    public static class InputValidation
    {
        #region Static Fields

        private static readonly Regex FirstDoubleFromString = new Regex(
            @"(?'number'\d+(?:(?:,|\.)\d+)?)", 
            RegexOptions.Compiled);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Finds the first occurence of a number, with or without decimal part, in the input string.
        /// </summary>
        /// <param name="input">The string to search.</param>
        /// <param name="number">Output parameter used to return the number. Is 0 if no number is found.</param>
        /// <returns>Returns true if a number is found in the input, otherwise returns false.</returns>
        public static bool PositiveNumFromString(string input, out double number)
        {
            CaptureCollection temp;

            if ((temp = FirstDoubleFromString.Match(input).Groups["number"].Captures).Count > 0)
            {
                MessageBox.Show(temp[0].Value);

                number = Convert.ToDouble(temp[0].Value);
                return true;
            }
            else
            {
                number = 0;
                return false;
            }
        }

        #endregion
    }
}