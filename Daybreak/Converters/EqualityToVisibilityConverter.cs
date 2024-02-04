using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Daybreak.Converters;

public sealed class EqualityToVisibilityConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length != 2)
        {
            return Visibility.Collapsed;
        }

        return values[0] == values[1] ? Visibility.Visible : Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

