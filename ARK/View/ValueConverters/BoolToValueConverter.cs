using System;
using System.Windows.Data;

namespace ARK.View.ValueConverters
{
    using System.Globalization;

    public class BoolToValueConverter<T> : IValueConverter
    {
        #region Public Properties

        public T FalseValue { get; set; }

        public T TrueValue { get; set; }

        #endregion

        #region Public Methods and Operators

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return this.FalseValue;
            }

            return (bool)value ? this.TrueValue : this.FalseValue;
        }

        public object ConvertBack(
            object value, 
            Type targetType, 
            object parameter, 
            CultureInfo culture)
        {
            return value != null ? value.Equals(this.TrueValue) : false;
        }

        #endregion
    }

    public class BoolToStringConverter : BoolToValueConverter<string>
    {
    }
}