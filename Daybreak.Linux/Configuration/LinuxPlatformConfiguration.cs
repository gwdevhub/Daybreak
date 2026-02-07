using Daybreak.Extensions;
using Daybreak.Linux.Services.ApplicationLauncher;
using Daybreak.Linux.Services.Credentials;
using Daybreak.Linux.Services.Injection;
using Daybreak.Linux.Services.Keyboard;
using Daybreak.Linux.Services.MDns;
using Daybreak.Linux.Services.Privilege;
using Daybreak.Linux.Services.Screens;
using Daybreak.Linux.Services.Startup.Actions;
using Daybreak.Linux.Services.Startup.Notifications;
using Daybreak.Linux.Services.UMod;
using Daybreak.Linux.Services.DirectSong;
using Daybreak.Linux.Services.Registry;
using Daybreak.Linux.Services.Themes;
using Daybreak.Linux.Services.SevenZip;
using Daybreak.Linux.Services.Wine;
using Daybreak.Shared.Models.Plugins;
using Daybreak.Shared.Services.ApplicationLauncher;
using Daybreak.Shared.Services.Credentials;
using Daybreak.Shared.Services.DirectSong;
using Daybreak.Shared.Services.Registry;
using Daybreak.Shared.Services.FileProviders;
using Daybreak.Shared.Services.Initialization;
using Daybreak.Shared.Services.Injection;
using Daybreak.Shared.Services.Keyboard;
using Daybreak.Shared.Services.MDns;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Shared.Services.Screens;
using Daybreak.Shared.Services.Themes;
using Daybreak.Shared.Services.SevenZip;
using Daybreak.Shared.Services.UMod;
using Microsoft.Extensions.DependencyInjection;

namespace Daybreak.Linux.Configuration;

/// <summary>
/// Linux-specific platform configuration.
/// Registers Linux-specific services and overrides.
/// </summary>
public sealed class LinuxPlatformConfiguration : PluginConfigurationBase
{
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddHostedSingleton<IScreenManager, ScreenManager>();
        services.AddHostedSingleton<IKeyboardHookService, KeyboardHookService>();
        services.AddScoped<IPrivilegeManager, PrivilegeManager>();
        services.AddScoped<IDaybreakInjector, DaybreakInjector>();
        services.AddSingleton<ICredentialProtector, CredentialProtector>();
        services.AddSingleton<IWinePidMapper, WinePidMapper>();
        services.AddScoped<IGuildWarsReadyChecker, GuildWarsReadyChecker>();
        services.AddScoped<IGuildWarsProcessFinder, GuildWarsProcessFinder>();
        services.AddSingleton<IModPathResolver, ModPathResolver>();
        services.AddSingleton<IDirectSongRegistrar, DirectSongRegistrar>();
        services.AddScoped<IRegistryService, RegistryService>();
        services.AddSingleton<ISystemThemeDetector, SystemThemeDetector>();
        services.AddSingleton<ISevenZipExtractor, SevenZipArchiveExtractor>();
        services.AddHostedSingleton<IMDomainRegistrar, PortScanningDomainRegistrar>();
    }

    public override void RegisterStartupActions(IStartupActionProducer startupActionProducer)
    {
        // Check Wine prefix setup on startup
        startupActionProducer.RegisterAction<SetupWinePrefixAction>();
    }

    public override void RegisterProviderAssemblies(IFileProviderProducer fileProviderProducer)
    {
        fileProviderProducer.RegisterAssembly(typeof(LinuxPlatformConfiguration).Assembly);
    }

    public override void RegisterNotificationHandlers(
        INotificationHandlerProducer notificationHandlerProducer
    )
    {
        notificationHandlerProducer.RegisterNotificationHandler<WinePrefixSetupHandler>();
    }

    public override void RegisterMods(IModsProducer modsProducer)
    {
        modsProducer.RegisterMod<IWinePrefixManager, WinePrefixManager>(singleton: true);
    }
}
