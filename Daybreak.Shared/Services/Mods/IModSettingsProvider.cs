using Daybreak.Shared.Models.LaunchConfigurations;

namespace Daybreak.Shared.Services.Mods;

public interface IModSettingsProvider
{
    T Get<T>(string modName, T globalSettings, LaunchConfigurationWithCredentials? configuration = null)
        where T : class;

    void Save<T>(string modName, T settings, LaunchConfigurationWithCredentials? configuration = null)
        where T : class;
}
