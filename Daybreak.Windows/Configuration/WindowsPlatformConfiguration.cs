using Daybreak.Shared.Models.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace Daybreak.Windows.Configuration;

/// <summary>
/// Windows-specific platform configuration.
/// Registers Windows-only services like Graph client, shortcuts, and monitoring.
/// </summary>
public sealed class WindowsPlatformConfiguration : PluginConfigurationBase
{
    public override void RegisterServices(IServiceCollection services)
    {
        // TODO: Register Windows-specific services
        // - Graph services (BlazorGraphClient, DaybreakMsalHttpClientProvider, etc.)
        // - Shortcut services (ShortcutManager)
        // - Monitoring services (DiskUsageMonitor, MemoryUsageMonitor)
    }
}
