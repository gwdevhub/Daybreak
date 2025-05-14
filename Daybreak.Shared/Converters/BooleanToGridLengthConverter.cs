using System;
using System.Extensions;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Daybreak.Shared.Converters;

public sealed class BooleanToGridLengthConverter : IValueConverter
{
    private static GridLength Collapsed = new(0);

    public GridLength VisibleValue { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return this.GetVisibility(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private object GetVisibility(object value)
    {
        if (value is not bool)
        {
            return this.VisibleValue;
        }

        var objValue = value.Cast<bool>();
        if (objValue)
        {
            return this.VisibleValue;
        }
        else
        {
            return Collapsed;
        }
    }
}

