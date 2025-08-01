using Daybreak.Shared.Models.Plugins;
using Daybreak.Shared.Services.ApplicationArguments;
using Daybreak.Shared.Services.Browser;
using Daybreak.Shared.Services.Menu;
using Daybreak.Shared.Services.Mods;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Startup;
using Daybreak.Shared.Services.Updater.PostUpdate;
using Slim;

namespace Daybreak.Shared.Services.Plugins;

public interface IPluginsService
{
    Task<bool> AddPlugin(string pathToPlugin);

    IEnumerable<AvailablePlugin> GetCurrentlyLoadedPlugins();

    void LoadPlugins(
        IServiceManager serviceManager,
        IOptionsProducer optionsProducer,
        IViewProducer viewProducer,
        IPostUpdateActionProducer postUpdateActionProducer,
        IStartupActionProducer startupActionProducer,
        INotificationHandlerProducer notificationHandlerProducer,
        IModsManager modsManager,
        IBrowserExtensionsProducer browserExtensionsProducer,
        IArgumentHandlerProducer argumentHandlerProducer,
        IMenuServiceProducer menuServiceProducer);

    IEnumerable<AvailablePlugin> GetAvailablePlugins();

    void SaveEnabledPlugins(IEnumerable<AvailablePlugin> availablePlugins);
}
