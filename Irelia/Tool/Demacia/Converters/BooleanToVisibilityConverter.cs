using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Demacia.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility rv = Visibility.Visible;
            try
            {
                var x = bool.Parse(value.ToString());
                rv = (x ? Visibility.Visible : Visibility.Collapsed);
            }
            catch (Exception)
            {
            }
            return rv;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            Visibility rv = (Visibility)value;
            return (rv == Visibility.Visible);
        }
    }
}
