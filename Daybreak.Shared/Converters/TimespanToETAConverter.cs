using System;
using System.Globalization;
using System.Windows.Data;

namespace Daybreak.Shared.Converters;

public sealed class TimespanToETAConverter : IValueConverter
{
    private const char PluralAppend = 's';

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return GetETAString(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static string GetETAString(object obj)
    {
        if (obj is not TimeSpan timeSpan)
        {
            return string.Empty;
        }

        if ((int)timeSpan.TotalDays > 0)
        {
            return $"{(int)timeSpan.TotalDays} day{((int)timeSpan.TotalDays > 1 ? PluralAppend : string.Empty)} remaining";
        }

        return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2} remaining";
    }
}
