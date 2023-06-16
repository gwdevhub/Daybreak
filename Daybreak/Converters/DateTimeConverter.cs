using System;
using System.Globalization;
using System.Windows.Data;

namespace Daybreak.Converters;
public sealed class DateTimeConverter : IValueConverter
{
    public bool LongTime { get; set; }
    public bool LongDate { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not DateTime dateTime)
        {
            return string.Empty;
        }

        if (this.LongTime)
        {
            return dateTime.ToLongTimeString();
        }

        if (this.LongDate)
        {
            return dateTime.ToLongDateString();
        }

        return dateTime.ToShortDateString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new InvalidOperationException();
    }
}
