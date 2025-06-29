using System.Globalization;
using System.Windows.Data;

namespace Daybreak.Shared.Converters;
public sealed class DoubleMultiplierConverter : IValueConverter
{
    public double Multiplier { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not double d)
        {
            return value;
        }

        return d * this.Multiplier;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
