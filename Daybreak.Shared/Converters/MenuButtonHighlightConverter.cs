using System;
using System.Globalization;
using System.Windows.Data;

namespace Daybreak.Converters;

public class MenuButtonHighlightConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType == typeof(double) &&
            value is bool boolean)
        {
            return boolean ? 0.4 : 0;
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType == typeof(bool) &&
            value is double doubleValue)
        {
            return doubleValue == 0.4;
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}
