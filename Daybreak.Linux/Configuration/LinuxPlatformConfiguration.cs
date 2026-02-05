using Daybreak.Shared.Models.Plugins;
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
        // TODO: Register Linux-specific service implementations
        // For example:
        // - Linux screen manager (using X11/Wayland APIs)
        // - Wine-based Guild Wars launcher
        // - Linux keyboard hook service (or no-op)
    }
}
