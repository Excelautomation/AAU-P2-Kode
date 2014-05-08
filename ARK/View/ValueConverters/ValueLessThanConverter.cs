﻿using System;
using System.Windows.Data;

namespace ARK.View.ValueConverters
{
    public class ValueLessThanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((int)value < (int)parameter)
                return (bool)true;
            else return (bool)false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}