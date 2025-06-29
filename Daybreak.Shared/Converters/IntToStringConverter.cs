using System.Globalization;
using System.Windows.Data;

namespace Daybreak.Shared.Converters;

public sealed class IntToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType == typeof(string) &&
            value is int i)
        {
            return i.ToString();
        }
        else if(targetType == typeof(int) &&
            value is string s)
        {
            return int.Parse(s);
        }
        else
        {
            throw new InvalidOperationException($"Unable to convert from {value.GetType().Name} to {targetType.Name}");
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType == typeof(string) &&
            value is int i)
        {
            return i.ToString();
        }
        else if (targetType == typeof(int) &&
            value is string s)
        {
            return int.Parse(s);
        }
        else
        {
            throw new InvalidOperationException($"Unable to convert from {value.GetType().Name} to {targetType.Name}");
        }
    }
}
