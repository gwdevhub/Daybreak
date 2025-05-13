using Daybreak.Models.Guildwars;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Daybreak.Converters;
public sealed class ItemBaseToIntConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int i)
        {
            return ItemBase.Parse(i, modifiers: null);
        }
        else if(value is ItemBase itemBase)
        {
            return itemBase.Id;
        }

        throw new InvalidOperationException($"Unable to convert value of type {value.GetType().Name}");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int i)
        {
            return ItemBase.Parse(i, modifiers: null);
        }
        else if (value is ItemBase itemBase)
        {
            return itemBase.Id;
        }

        throw new InvalidOperationException($"Unable to convert value of type {value.GetType().Name}");
    }
}
