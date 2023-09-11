using System;
using System.Globalization;
using System.Windows.Data;

namespace Daybreak.Converters;
public sealed class DoubleMultiplierConverter : IValueConverter
{
    public double Multiplier { get; set; } = 1;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not double doubleValue)
        {
            return value;
        }

        return doubleValue * this.Multiplier;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
