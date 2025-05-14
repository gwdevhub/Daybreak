using System;
using System.Globalization;
using System.Windows.Data;

namespace Daybreak.Shared.Converters;
public sealed class DateTimeConverter : IValueConverter
{
    public string? Format { get; set; }
    public bool LongTime { get; set; }
    public bool LongDate { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not DateTime dateTime)
        {
            return string.Empty;
        }

        if (this.Format is not null)
        {
            return dateTime.ToString(this.Format);
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
