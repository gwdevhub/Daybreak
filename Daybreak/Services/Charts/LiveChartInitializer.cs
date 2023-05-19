using Daybreak.Models.Metrics;
using Daybreak.Models.Trade;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.Charts;

public sealed class LiveChartInitializer : ILiveChartInitializer, IApplicationLifetimeService
{
    public void OnClosing()
    {
    }

    public void OnStartup()
    {
        LiveCharts.Configure(config =>
            config.AddSkiaSharp()
                .AddDefaultMappers()
                .AddDarkTheme()
                .AddLightTheme()
                .HasMap<Metric>((metric, point) =>
                {
                    point.SecondaryValue = metric.Timestamp.Ticks;
                    point.PrimaryValue = Convert.ToDouble(metric.Measurement);
                })
                .HasMap<TraderQuote>((quote, point) =>
                {
                    point.SecondaryValue = quote.Timestamp?.Ticks ?? 0;
                    point.PrimaryValue = ((double)quote.Price) / 20d;
                }));
    }
}
