using Daybreak.Models.LaunchConfigurations;
using System.Collections.Generic;

namespace Daybreak.Services.LaunchConfigurations;
public interface ILaunchConfigurationService
{
    IEnumerable<LaunchConfigurationWithCredentials> GetLaunchConfigurations();

    LaunchConfigurationWithCredentials? CreateConfiguration();

    bool SaveConfiguration(LaunchConfigurationWithCredentials launchConfigurationWithCredentials);

    bool DeleteConfiguration(LaunchConfigurationWithCredentials launchConfigurationWithCredentials);

    bool IsValid(LaunchConfigurationWithCredentials launchConfiguration);

    LaunchConfigurationWithCredentials? GetLastLaunchConfigurationWithCredentials();

    void SetLastLaunchConfigurationWithCredentials(LaunchConfigurationWithCredentials launchConfigurationWithCredentials);
}
