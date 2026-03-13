using Daybreak.Shared.Models.LaunchConfigurations;

namespace Daybreak.Shared.Services.LaunchConfigurations;

public interface ILaunchConfigurationService
{
    IEnumerable<LaunchConfigurationWithCredentials> GetLaunchConfigurations();

    LaunchConfigurationWithCredentials? CreateConfiguration();

    bool SaveConfiguration(LaunchConfigurationWithCredentials launchConfigurationWithCredentials);

    void SaveLaunchConfigurations(List<LaunchConfigurationWithCredentials> launchConfigurations);

    bool DeleteConfiguration(LaunchConfigurationWithCredentials launchConfigurationWithCredentials);

    bool IsValid(LaunchConfigurationWithCredentials launchConfiguration);
}
