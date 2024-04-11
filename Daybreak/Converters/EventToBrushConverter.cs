using Daybreak.Models;
using Daybreak.Models.Guildwars;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Daybreak.Converters;
internal sealed class EventToBrushConverter : IValueConverter
{
    private static readonly Dictionary<Event, Brush> EventMapping = new()
    {
        { Event.CanthanNewYear, new SolidColorBrush(ColorPalette.Amber) },
        { Event.LuckyTreatsWeek, new SolidColorBrush(ColorPalette.Green) },
        { Event.AprilFoolsDay, new SolidColorBrush(ColorPalette.DeepPurple) },
        { Event.SweetTreatsWeek, new SolidColorBrush(ColorPalette.Indigo) },
        { Event.AnniversaryCelebration, new SolidColorBrush(ColorPalette.Yellow) },
        { Event.DragonFestival, new SolidColorBrush(ColorPalette.DeepOrange) },
        { Event.WintersdayInJuly, new SolidColorBrush(ColorPalette.LightBlue) },
        { Event.WayfarersReverie, new SolidColorBrush(ColorPalette.Magenta) },
        { Event.PirateWeek, new SolidColorBrush(ColorPalette.Gold) },
        { Event.BreastCancerAwarenessMonth, new SolidColorBrush(ColorPalette.Pink) },
        { Event.Halloween, new SolidColorBrush(ColorPalette.Orange) },
        { Event.SpecialTreatsWeek, new SolidColorBrush(ColorPalette.Lime) },
        { Event.WintersdayJanuary, new SolidColorBrush(ColorPalette.BlueGrey) },
        { Event.WintersdayDecember, new SolidColorBrush(ColorPalette.BlueGrey) }
    };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Event e ||
            targetType != typeof(Brush))
        {
            throw new InvalidOperationException($"Unable to convert from {value.GetType().Name} to {targetType.Name}");
        }

        return EventMapping[e];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
