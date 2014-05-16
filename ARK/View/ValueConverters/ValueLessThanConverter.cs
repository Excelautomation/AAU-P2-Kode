using System;
using System.Windows.Data;

namespace ARK.View.ValueConverters
{
    using System.Globalization;

    public class ValueLessThanConverter : IValueConverter
    {
        #region Public Methods and Operators

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value < (int)parameter)
            {
                return (bool)true;
            }
            else
            {
                return (bool)false;
            }
        }

        public object ConvertBack(
            object value, 
            Type targetType, 
            object parameter, 
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}