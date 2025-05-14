using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Effects;

namespace Daybreak.Shared.Converters;
public sealed class BooleanToEffectConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool boolean)
        {
            return default!;
        }

        if (parameter is not Effect effect)
        {
            return default!;
        }

        if (!boolean)
        {
            return default!;
        }

        return effect;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
