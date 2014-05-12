using System;
using System.Windows.Data;

namespace ARK.View.ValueConverters
{
    public class EnumToValueConverter<T> : IValueConverter
    {
        public T Enum1 { get; set; }
        public T Enum2 { get; set; }
        public T Enum3 { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return Enum1;
                case 1:
                    return Enum2;
                case 2:
                    return Enum3;
                default:
                    return null;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EnumToStringConverter : EnumToValueConverter<String> { }
}
