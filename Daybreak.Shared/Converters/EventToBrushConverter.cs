using Daybreak.Shared.Models.Guildwars;
using System.Globalization;
using System.Windows.Data;
using Brush = System.Windows.Media.Brush;

namespace Daybreak.Shared.Converters;
public sealed class EventToBrushConverter : IValueConverter
{
    private static readonly Dictionary<Event, Brush> EventMapping = new()
    {
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
