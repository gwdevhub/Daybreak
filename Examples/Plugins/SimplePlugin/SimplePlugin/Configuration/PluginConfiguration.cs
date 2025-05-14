using Daybreak.Shared.Shared.Models.Plugins;
using Microsoft.Extensions.DependencyInjection;
using SimplePlugin.Services;
using System.Core.Extensions;

namespace SimplePlugin.Configuration;

public sealed class PluginConfiguration : PluginConfigurationBase
{
    public override void RegisterServices(IServiceCollection services)
    {
        services.ThrowIfNull();

        services.AddSingleton<EmitLoadedNotificationService>();
    }
}
