using Daybreak.Shared.Models.Plugins;
using Daybreak.Shared.Services.ApplicationArguments;
using Daybreak.Shared.Services.Initialization;
using Microsoft.Extensions.DependencyInjection;

namespace Daybreak.Shared.Services.Plugins;

public interface IPluginsService
{
    Task<bool> AddPlugin(string pathToPlugin);

    IEnumerable<AvailablePlugin> GetCurrentlyLoadedPlugins();

    void LoadPlugins(
        IServiceCollection services,
        IOptionsProducer optionsProducer,
        IViewProducer viewProducer,
        IPostUpdateActionProducer postUpdateActionProducer,
        IStartupActionProducer startupActionProducer,
        INotificationHandlerProducer notificationHandlerProducer,
        IModsProducer modsManager,
        IArgumentHandlerProducer argumentHandlerProducer,
        IMenuServiceProducer menuServiceProducer,
        IThemeProducer themeProducer);

    IEnumerable<AvailablePlugin> GetAvailablePlugins();

    void SaveEnabledPlugins(IEnumerable<AvailablePlugin> availablePlugins);
}
