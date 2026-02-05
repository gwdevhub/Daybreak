using Daybreak.Configuration;
using Daybreak.Extensions;
using Daybreak.Services.Graph;
using Daybreak.Shared.Models.Plugins;
using Daybreak.Shared.Services.Initialization;
using Daybreak.Shared.Services.Screens;
using Daybreak.Shared.Services.Shortcuts;
using Daybreak.Windows.Services.Graph;
using Daybreak.Windows.Services.Monitoring;
using Daybreak.Windows.Services.Screens;
using Daybreak.Windows.Services.Shortcuts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Desktop;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Daybreak.Windows.Configuration;

/// <summary>
/// Windows-specific platform configuration.
/// Registers Windows-only services like Graph client, shortcuts, and monitoring.
/// </summary>
public sealed class WindowsPlatformConfiguration : PluginConfigurationBase
{
    public override void RegisterServices(IServiceCollection services)
    {
        // Register IPublicClientApplication for MSAL
        services.AddSingleton<IPublicClientApplication>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<PublicClientApplication>>();
            try
            {
                return PublicClientApplicationBuilder.Create(SecretManager.GetSecret(SecretKeys.AadApplicationId))
                    .WithLogging((logLevel, message, containsPii) =>
                    {
                        if (containsPii && logLevel > Microsoft.Identity.Client.LogLevel.Info)
                        {
                            message = "[REDACTED]";
                        }

                        var equivalentLogLevel = logLevel switch
                        {
                            Microsoft.Identity.Client.LogLevel.Error => LogLevel.Error,
                            Microsoft.Identity.Client.LogLevel.Warning => LogLevel.Warning,
                            Microsoft.Identity.Client.LogLevel.Info => LogLevel.Information,
                            Microsoft.Identity.Client.LogLevel.Verbose => LogLevel.Debug,
                            _ => LogLevel.None
                        };

                        logger.Log(equivalentLogLevel, message);
                    }, enablePiiLogging: true, enableDefaultPlatformLogging: true)
                    .WithCacheOptions(new CacheOptions { UseSharedCache = true })
                    .WithRedirectUri(BlazorGraphClient.RedirectUri)
                    .WithWindowsEmbeddedBrowserSupport()
                    .WithHttpClientFactory(new DaybreakMsalHttpClientProvider(new HttpClient()))
                    .Build();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to create PublicClientApplication, using dummy implementation");
                return new DummyPublicClientApplication();
            }
        });

        // Graph client
        services.AddScoped<IGraphClient, BlazorGraphClient>();

        // Screen manager (Windows-specific)
        services.AddHostedSingleton<IScreenManager, ScreenManager>();

        // Shortcut manager
        services.AddHostedSingleton<IShortcutManager, ShortcutManager>();

        // Performance monitoring (Windows-only PerformanceCounter)
        services.AddHostedService<MemoryUsageMonitor>();
        services.AddHostedService<DiskUsageMonitor>();
    }

    public override void RegisterMods(IModsProducer modsProducer)
    {
        // Windows-specific mods
        modsProducer.RegisterMod<IGuildwarsScreenPlacer, GuildwarsScreenPlacer>();
    }
}
