using Daybreak.Extensions;
using Daybreak.Linux.Services.Screens;
using Daybreak.Shared.Models.Plugins;
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

        // TODO: Register additional Linux-specific service implementations
        // For example:
        // - Wine-based Guild Wars launcher
        // - Linux keyboard hook service (or no-op)
    }
}
