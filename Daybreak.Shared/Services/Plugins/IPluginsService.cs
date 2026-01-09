using Daybreak.Shared.Models.Plugins;
using Daybreak.Shared.Services.ApplicationArguments;
using Daybreak.Shared.Services.Menu;
using Daybreak.Shared.Services.Mods;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Startup;
using Daybreak.Shared.Services.Themes;
using Daybreak.Shared.Services.Updater.PostUpdate;
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
        IModsManager modsManager,
        IArgumentHandlerProducer argumentHandlerProducer,
        IMenuServiceProducer menuServiceProducer,
        IThemeProducer themeProducer);

    IEnumerable<AvailablePlugin> GetAvailablePlugins();

    void SaveEnabledPlugins(IEnumerable<AvailablePlugin> availablePlugins);
}
