using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Daybreak.Shared.Converters;
public class RoundedRectConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var w = (double)values[0];
        var h = (double)values[1];
        var r = System.Convert.ToDouble(parameter);
        return new RectangleGeometry(new Rect(0, 0, w, h), r, r);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
