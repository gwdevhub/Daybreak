using Daybreak.Shared.Models.Guildwars;
using System.Globalization;
using System.Windows.Data;

namespace Daybreak.Shared.Converters;
public sealed class ItemBaseToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string s)
        {
            return ItemBase.Parse(s);
        }
        else if(value is ItemBase itemBase)
        {
            return itemBase.Name ?? string.Empty;
        }

        throw new InvalidOperationException($"Unable to convert value of type {value.GetType().Name}");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string s)
        {
            return ItemBase.Parse(s);
        }
        else if (value is ItemBase itemBase)
        {
            return itemBase.Name ?? string.Empty;
        }

        throw new InvalidOperationException($"Unable to convert value of type {value.GetType().Name}");
    }
}
