using Daybreak.Shared.Models.Plugins;

namespace Daybreak.Shared.Services.Plugins;

public interface IPluginsService
{
    Task<bool> AddPlugin(string pathToPlugin);

    IEnumerable<AvailablePlugin> GetCurrentlyLoadedPlugins();

    void UpdateLoadedPlugins(IReadOnlyList<AvailablePlugin> loadedPlugins);

    IReadOnlyList<AvailablePlugin> LoadPlugins();

    IEnumerable<AvailablePlugin> GetAvailablePlugins();

    void SaveEnabledPlugins(IEnumerable<AvailablePlugin> availablePlugins);
}
