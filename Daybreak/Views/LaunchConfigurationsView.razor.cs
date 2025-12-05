using Daybreak.Shared.Models;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Services.Credentials;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.LaunchConfigurations;
using System.Extensions;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class LaunchConfigurationsViewModel(
    ILaunchConfigurationService launchConfigurationService,
    ICredentialManager credentialManager,
    IGuildWarsExecutableManager guildWarsExecutableManager)
    : ViewModelBase<LaunchConfigurationsViewModel, LaunchConfigurationsView>
{
    private readonly ILaunchConfigurationService launchConfigurationService = launchConfigurationService;
    private readonly ICredentialManager credentialManager = credentialManager;
    private readonly IGuildWarsExecutableManager guildWarsExecutableManager = guildWarsExecutableManager;

    public List<string> Executables { get; } = [];
    public List<LoginCredentials> Credentials { get; } = [];
    public List<LaunchConfigurationWithCredentials> LaunchConfigurations { get; } = [];

    public override ValueTask ParametersSet(LaunchConfigurationsView view, CancellationToken cancellationToken)
    {
        this.Credentials.ClearAnd().AddRange(this.credentialManager.GetCredentialList());
        this.LaunchConfigurations.ClearAnd().AddRange(this.launchConfigurationService.GetLaunchConfigurations());
        this.Executables.ClearAnd().AddRange(this.guildWarsExecutableManager.GetExecutableList()).Add(string.Empty);
        return base.ParametersSet(view, cancellationToken);
    }

    public void CreateNewLaunchConfiguration()
    {
        var config = this.launchConfigurationService.CreateConfiguration();
        if (config is null)
        {
            return;
        }

        config.Credentials = this.Credentials.FirstOrDefault();
        config.ExecutablePath = this.Executables.FirstOrDefault();
        this.launchConfigurationService.SaveConfiguration(config);
        this.LaunchConfigurations.Add(config);
    }

    public void ExecutableChanged(LaunchConfigurationWithCredentials configuration, string newValue)
    {
        configuration.ExecutablePath = string.IsNullOrWhiteSpace(newValue) ? null : newValue;
        this.launchConfigurationService.SaveConfiguration(configuration);
    }

    public void CredentialsChanged(LaunchConfigurationWithCredentials configuration, string newCredsId)
    {
        if (this.Credentials.FirstOrDefault(c => c.Identifier == newCredsId) is not LoginCredentials newCreds)
        {
            return;
        }

        configuration.Credentials = newCreds;
        this.launchConfigurationService.SaveConfiguration(configuration);
    }

    public void CustomArgsChanged(LaunchConfigurationWithCredentials configuration, string newArgs)
    {
        configuration.Arguments = newArgs;
        this.launchConfigurationService.SaveConfiguration(configuration);
    }

    public void DeleteLaunchConfiguration(LaunchConfigurationWithCredentials configuration)
    {
        this.launchConfigurationService.DeleteConfiguration(configuration);
        this.LaunchConfigurations.Remove(configuration);
    }
}
