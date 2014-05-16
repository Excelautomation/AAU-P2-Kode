using System;
using System.Windows.Data;

namespace ARK.View.ValueConverters
{
    using System.Globalization;

    public class EnumToValueConverter<T> : IValueConverter
    {
        #region Public Properties

        public T Enum1 { get; set; }

        public T Enum2 { get; set; }

        public T Enum3 { get; set; }

        #endregion

        #region Public Methods and Operators

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return this.Enum1;
                case 1:
                    return this.Enum2;
                case 2:
                    return this.Enum3;
                default:
                    return null;
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

    public class EnumToStringConverter : EnumToValueConverter<string>
    {
    }
}