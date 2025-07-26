using LiveChartsCore;
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
