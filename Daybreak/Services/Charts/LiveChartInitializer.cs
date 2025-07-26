using Daybreak.Shared.Models.Metrics;
using Daybreak.Shared.Models.Trade;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.Charts;

internal sealed class LiveChartInitializer : ILiveChartInitializer, IApplicationLifetimeService
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
                .AddLightTheme());
    }
}
