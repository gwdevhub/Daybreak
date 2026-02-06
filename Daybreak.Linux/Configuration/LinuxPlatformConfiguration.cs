using Daybreak.Extensions;
using Daybreak.Linux.Services.Credentials;
using Daybreak.Linux.Services.Injection;
using Daybreak.Linux.Services.Keyboard;
using Daybreak.Linux.Services.Privilege;
using Daybreak.Linux.Services.Screens;
using Daybreak.Linux.Services.Startup.Actions;
using Daybreak.Linux.Services.Startup.Notifications;
using Daybreak.Linux.Services.Wine;
using Daybreak.Shared.Models.Plugins;
using Daybreak.Shared.Services.Credentials;
using Daybreak.Shared.Services.FileProviders;
using Daybreak.Shared.Services.Initialization;
using Daybreak.Shared.Services.Injection;
using Daybreak.Shared.Services.Keyboard;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Shared.Services.Screens;
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
        // Notification handlers
        services.AddScoped<WinePrefixSetupHandler>();

        // Screen manager (Linux-specific dummy implementation)
        services.AddHostedSingleton<IScreenManager, ScreenManager>();

        // Keyboard hook service (Linux dummy - no-op)
        services.AddHostedSingleton<IKeyboardHookService, KeyboardHookService>();

        // Privilege manager (Linux dummy - TODO: implement with pkexec/polkit)
        services.AddScoped<IPrivilegeManager, PrivilegeManager>();

        // Daybreak injector (Linux - Wine-based)
        services.AddScoped<IDaybreakInjector, DaybreakInjector>();

        // Credential protector (Linux AES with machine-id derived key)
        services.AddSingleton<ICredentialProtector, LinuxCredentialProtector>();
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

    public override void RegisterNotificationHandlers(INotificationHandlerProducer notificationHandlerProducer)
    {
        notificationHandlerProducer.RegisterNotificationHandler<WinePrefixSetupHandler>();
    }

    public override void RegisterMods(IModsProducer modsProducer)
    {
        modsProducer.RegisterMod<IWinePrefixManager, WinePrefixManager>(singleton: true);
    }
}
