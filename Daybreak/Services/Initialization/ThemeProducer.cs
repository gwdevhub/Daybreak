using Daybreak.Shared.Models.Themes;
using Daybreak.Shared.Services.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Extensions.Core;

namespace Daybreak.Services.Initialization;

public sealed class ThemeProducer(IServiceCollection services, ILogger<ThemeProducer> logger)
    : IThemeProducer
{
    private readonly IServiceCollection services = services;
    private readonly ILogger<ThemeProducer> logger = logger;

    public void RegisterTheme(Theme theme)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        Theme.Themes.Add(theme);
        scopedLogger.LogDebug("Registered theme: {Theme.Name}", theme.Name);
    }
}
