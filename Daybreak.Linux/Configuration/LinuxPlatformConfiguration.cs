using Daybreak.Extensions;
using Daybreak.Linux.Services.Injection;
using Daybreak.Linux.Services.Keyboard;
using Daybreak.Linux.Services.Privilege;
using Daybreak.Linux.Services.Screens;
using Daybreak.Shared.Models.Plugins;
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
        // Screen manager (Linux-specific dummy implementation)
        services.AddHostedSingleton<IScreenManager, ScreenManager>();

        // Keyboard hook service (Linux dummy - no-op)
        services.AddHostedSingleton<IKeyboardHookService, KeyboardHookService>();

        // Privilege manager (Linux dummy - TODO: implement with pkexec/polkit)
        services.AddScoped<IPrivilegeManager, PrivilegeManager>();

        // Daybreak injector (Linux - Wine-based, TODO: implement)
        services.AddScoped<IDaybreakInjector, DaybreakInjector>();

        // TODO: Register additional Linux-specific service implementations
        // For example:
        // - Wine-based Guild Wars launcher
    }
}
