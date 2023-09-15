using Daybreak.Models.Plugins;
using Daybreak.Services.Drawing;
using Daybreak.Services.Mods;
using Daybreak.Services.Navigation;
using Daybreak.Services.Notifications;
using Daybreak.Services.Options;
using Daybreak.Services.Startup;
using Daybreak.Services.Updater.PostUpdate;
using Slim;
using System.Collections.Generic;

namespace Daybreak.Services.Plugins;

public interface IPluginsService
{
    IEnumerable<AvailablePlugin> GetCurrentlyLoadedPlugins();

    void LoadPlugins(
        IServiceManager serviceManager,
        IOptionsProducer optionsProducer,
        IViewManager viewManager,
        IPostUpdateActionProducer postUpdateActionProducer,
        IStartupActionProducer startupActionProducer,
        IDrawingModuleProducer drawingModuleProducer,
        INotificationHandlerProducer notificationHandlerProducer,
        IModsManager modsManager);

    IEnumerable<AvailablePlugin> GetAvailablePlugins();

    void SaveEnabledPlugins(IEnumerable<AvailablePlugin> availablePlugins);
}
