using Daybreak.Models.Plugins;
using Daybreak.Services.ApplicationArguments;
using Daybreak.Services.Browser;
using Daybreak.Services.Drawing;
using Daybreak.Services.Mods;
using Daybreak.Services.Navigation;
using Daybreak.Services.Notifications;
using Daybreak.Services.Options;
using Daybreak.Services.Startup;
using Daybreak.Services.Updater.PostUpdate;
using Slim;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Daybreak.Services.Plugins;

public interface IPluginsService
{
    Task<bool> AddPlugin(string pathToPlugin);

    IEnumerable<AvailablePlugin> GetCurrentlyLoadedPlugins();

    void LoadPlugins(
        IServiceManager serviceManager,
        IOptionsProducer optionsProducer,
        IViewManager viewManager,
        IPostUpdateActionProducer postUpdateActionProducer,
        IStartupActionProducer startupActionProducer,
        IDrawingModuleProducer drawingModuleProducer,
        INotificationHandlerProducer notificationHandlerProducer,
        IModsManager modsManager,
        IBrowserExtensionsProducer browserExtensionsProducer,
        IArgumentHandlerProducer argumentHandlerProducer);

    IEnumerable<AvailablePlugin> GetAvailablePlugins();

    void SaveEnabledPlugins(IEnumerable<AvailablePlugin> availablePlugins);
}
