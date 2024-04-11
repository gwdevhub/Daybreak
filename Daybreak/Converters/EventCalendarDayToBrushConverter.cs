using Daybreak.Models.Guildwars;
using Daybreak.Services.Events;
using ExCSS;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Daybreak.Converters;
public sealed class EventCalendarDayToBrushConverter : IValueConverter
{
    private readonly EventToBrushConverter eventToBrushConverter = new();
    private readonly IEventService eventService = Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IEventService>();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not DateTime dayDate)
        {
            throw new InvalidOperationException($"Unable to convert from {value.GetType().Name}");
        }

        dayDate = new DateTime(dayDate.Year, dayDate.Month, dayDate.Day, 19, 0, 0);
        var activeEvents = this.eventService.GetActiveEvents(dayDate).ToList();
        if (activeEvents.Count == 0)
        {
            return Brushes.Transparent;
        }

        if (activeEvents.Count == 1 &&
            activeEvents.FirstOrDefault() is Event ev)
        {
            return this.eventToBrushConverter.Convert(ev, typeof(Brush), parameter, culture);
        }

        var brushes = this.eventService.GetActiveEvents(dayDate).Select(e => this.eventToBrushConverter.Convert(e, typeof(Brush), parameter, culture)).OfType<SolidColorBrush>().ToList();
        var canvas = new Canvas();
        double width = 1.0 / brushes.Count; // Assuming a normalized width for each color segment

        for (int i = 0; i < brushes.Count; i++)
        {
            var rect = new Rectangle
            {
                Width = width,
                Height = 1,
                Fill = brushes[i],
                RenderTransform = new TranslateTransform(i * width, 0)
            };
            canvas.Children.Add(rect);
        }

        return new VisualBrush
        {
            Visual = canvas,
            Stretch = Stretch.None,
            TileMode = TileMode.None,
            Viewbox = new Rect(0, 0, 1, 1),
            ViewboxUnits = BrushMappingMode.RelativeToBoundingBox,
            Transform = new RotateTransform(45, 0.5, 0.5)
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
