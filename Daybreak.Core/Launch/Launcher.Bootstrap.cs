using Daybreak.Configuration.Options;
using Daybreak.Extensions;
using Daybreak.Services.Options;
using Daybreak.Services.Plugins;
using Daybreak.Services.Themes;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Plugins;
using Daybreak.Shared.Services.Themes;
using Microsoft.Extensions.DependencyInjection;
using System.Extensions;

namespace Daybreak.Launch;

public partial class Launcher
{
    public static ServiceProvider SetupBootstrap()
    {
        var serviceCollection = new ServiceCollection();
        SetupLogging(serviceCollection);
        BootstrapOptionsManager(serviceCollection);
        BootstrapThemeOptions(serviceCollection);
        BootstrapScreenManagerOptions(serviceCollection);
        BootstrapPluginsService(serviceCollection);
        return serviceCollection.BuildServiceProvider();
    }

    private static void BootstrapPluginsService(IServiceCollection services)
    {
        services.AddDaybreakOptions<PluginsServiceOptions>();
        services.AddSingleton<IPluginsService, PluginsService>();
    }

    private static void BootstrapOptionsManager(IServiceCollection services)
    {
        services.AddSingleton<OptionsManager>();
        services.AddSingleton<IOptionsProvider>(sp => sp.GetRequiredService<OptionsManager>());
    }

    private static void BootstrapThemeOptions(IServiceCollection services)
    {
        services.AddDaybreakOptions<ThemeOptions>();
        services.AddSingleton<IThemeManager, BlazorThemeInteropService>();
        services.AddHostedService(sp => sp.GetRequiredService<IThemeManager>().Cast<BlazorThemeInteropService>());
    }

    private static void BootstrapScreenManagerOptions(IServiceCollection services)
    {
        // Only register options here - actual ScreenManager is registered by platform-specific configuration
        services.AddDaybreakOptions<ScreenManagerOptions>();
    }
}
