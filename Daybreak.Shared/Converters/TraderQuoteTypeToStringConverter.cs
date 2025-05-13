using Daybreak.Models.Trade;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Daybreak.Converters;
public sealed class TraderQuoteTypeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(string))
        {
            throw new InvalidOperationException($"Unable to convert from {nameof(TraderQuoteType)} to {targetType.Name}");
        }

        return value?.ToString() ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string s)
        {
            throw new InvalidOperationException($"Unable to convert to {targetType.Name}");
        }

        if (!Enum.TryParse<TraderQuoteType>(s, out var enumValue))
        {
            throw new InvalidOperationException($"Invalid string value provided {s}");
        }

        return enumValue;
    }
}
