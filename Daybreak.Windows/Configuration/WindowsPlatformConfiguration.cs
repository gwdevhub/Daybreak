using Daybreak.Configuration;
using Daybreak.Extensions;
using Daybreak.Services.Graph;
using Daybreak.Services.MDns;
using Daybreak.Shared.Models.Plugins;
using Daybreak.Shared.Services.Credentials;
using Daybreak.Shared.Services.Initialization;
using Daybreak.Shared.Services.Injection;
using Daybreak.Shared.Services.Keyboard;
using Daybreak.Shared.Services.MDns;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Shared.Services.Screens;
using Daybreak.Shared.Services.Shortcuts;
using Daybreak.Shared.Services.DirectSong;
using Daybreak.Shared.Services.SevenZip;
using Daybreak.Shared.Services.UMod;
using Daybreak.Shared.Services.ReShade;
using Daybreak.Shared.Services.ApplicationLauncher;
using Daybreak.Windows.Services.ApplicationLauncher;
using Daybreak.Windows.Services.Credentials;
using Daybreak.Windows.Services.Graph;
using Daybreak.Windows.Services.Injection;
using Daybreak.Windows.Services.Keyboard;
using Daybreak.Windows.Services.Monitoring;
using Daybreak.Windows.Services.Privilege;
using Daybreak.Windows.Services.Screens;
using Daybreak.Windows.Services.Shortcuts;
using Daybreak.Windows.Services.SevenZip;
using Daybreak.Windows.Services.UMod;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Desktop;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using Daybreak.Windows.Services.DirectSong;
using Daybreak.Services.ReShade;
using Daybreak.Services.Startup.Actions;

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

        services.AddScoped<IGraphClient, BlazorGraphClient>();
        services.AddHostedSingleton<IScreenManager, ScreenManager>();
        services.AddHostedSingleton<IShortcutManager, ShortcutManager>();
        services.AddHostedService<MemoryUsageMonitor>();
        services.AddHostedService<DiskUsageMonitor>();
        services.AddHostedSingleton<IKeyboardHookService, KeyboardHookService>();
        services.AddScoped<IPrivilegeManager, PrivilegeManager>();
        services.AddSingleton<ICredentialProtector, CredentialProtector>();
        services.AddScoped<IDaybreakInjector, DaybreakInjector>();
        services.AddScoped<IGuildWarsReadyChecker, GuildWarsReadyChecker>();
        services.AddScoped<IGuildWarsProcessFinder, GuildWarsProcessFinder>();
        services.AddSingleton<IModPathResolver, ModPathResolver>();
        services.AddSingleton<IDirectSongRegistrar, DirectSongRegistrar>();
        services.AddSingleton<ISevenZipExtractor, SevenZipArchiveExtractor>();
        services.AddHostedSingleton<IMDomainRegistrar, MDomainRegistrar>();
    }

    public override void RegisterMods(IModsProducer modsProducer)
    {
        modsProducer.RegisterMod<IReShadeService, ReShadeService>();
        modsProducer.RegisterMod<IGuildwarsScreenPlacer, GuildwarsScreenPlacer>();
    }

    public override void RegisterStartupActions(IStartupActionProducer startupActionProducer)
    {
        startupActionProducer.RegisterAction<UpdateReShadeAction>();
    }
}
