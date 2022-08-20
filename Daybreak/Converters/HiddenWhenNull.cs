using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Daybreak.Converters
{
    public class HiddenWhenNull : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.GetVerticalAlignment(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private object GetVerticalAlignment(object value)
        {
            if (value is null)
            {
                return Visibility.Hidden;
            }
            else
            {
                return Visibility.Visible;
            }
        }
    }
}
