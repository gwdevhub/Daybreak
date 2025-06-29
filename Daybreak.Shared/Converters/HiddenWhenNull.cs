using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Daybreak.Shared.Converters;

public class HiddenWhenNull : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return GetVerticalAlignment(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static object GetVerticalAlignment(object value)
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
