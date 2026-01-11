using Daybreak.Shared.Models.Plugins;

namespace Daybreak.Shared.Services.Plugins;

public interface IPluginsService
{
    Task<bool> AddPlugin(string pathToPlugin);

    IEnumerable<AvailablePlugin> GetCurrentlyLoadedPlugins();

    void LoadPlugins();

    IEnumerable<AvailablePlugin> GetAvailablePlugins();

    void SaveEnabledPlugins(IEnumerable<AvailablePlugin> availablePlugins);
}
