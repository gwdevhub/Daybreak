using Daybreak.Shared.Models;
using Daybreak.Shared.Models.ColorPalette;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Services.Credentials;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.LaunchConfigurations;
using System.Extensions;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;

public sealed class LaunchConfigurationsViewModel(
    ILaunchConfigurationService launchConfigurationService,
    ICredentialManager credentialManager,
    IGuildWarsExecutableManager guildWarsExecutableManager,
    IViewManager viewManager)
    : ViewModelBase<LaunchConfigurationsViewModel, LaunchConfigurationsView>
{
    private readonly ILaunchConfigurationService launchConfigurationService = launchConfigurationService;
    private readonly ICredentialManager credentialManager = credentialManager;
    private readonly IGuildWarsExecutableManager guildWarsExecutableManager = guildWarsExecutableManager;
    private readonly IViewManager viewManager = viewManager;

    public List<string> Executables { get; } = [];
    public List<LoginCredentials> Credentials { get; } = [];
    public List<LaunchConfigurationWithCredentials> LaunchConfigurations { get; } = [];
    public List<string> AvailableColors { get; } = [string.Empty, .. AccentColor.Accents.Select(a => a.Name.ToString())];

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
        this.LaunchConfigurations.Insert(0, config);
        this.RefreshView();
    }

    public void ExecutableChanged(LaunchConfigurationWithCredentials configuration, string newValue)
    {
        configuration.ExecutablePath = string.IsNullOrWhiteSpace(newValue) ? null : newValue;
        this.launchConfigurationService.SaveConfiguration(configuration);
        this.RefreshView();
    }

    public void CredentialsChanged(LaunchConfigurationWithCredentials configuration, string newCredsId)
    {
        if (this.Credentials.FirstOrDefault(c => c.Identifier == newCredsId) is not LoginCredentials newCreds)
        {
            return;
        }

        configuration.Credentials = newCreds;
        this.launchConfigurationService.SaveConfiguration(configuration);
        this.RefreshView();
    }

    public void CustomArgsChanged(LaunchConfigurationWithCredentials configuration, string newArgs)
    {
        configuration.Arguments = newArgs;
        this.launchConfigurationService.SaveConfiguration(configuration);
        this.RefreshView();
    }

    public void DeleteLaunchConfiguration(LaunchConfigurationWithCredentials configuration)
    {
        this.launchConfigurationService.DeleteConfiguration(configuration);
        this.LaunchConfigurations.RemoveAt(this.LaunchConfigurations.FindIndex(c => c.Identifier == configuration.Identifier));
        this.RefreshView();
    }

    public void CustomNameChanged(LaunchConfigurationWithCredentials configuration, string newName)
    {
        configuration.Name = newName;
        this.launchConfigurationService.SaveConfiguration(configuration);
        this.RefreshView();
    }

    public void SteamSupportChanged(LaunchConfigurationWithCredentials configuration, bool isEnabled)
    {
        configuration.SteamSupport = isEnabled;
        this.launchConfigurationService.SaveConfiguration(configuration);
        this.RefreshView();
    }

    public void CustomModLoadoutChanged(LaunchConfigurationWithCredentials configuration, bool isEnabled)
    {
        configuration.CustomModLoadoutEnabled = isEnabled;
        this.launchConfigurationService.SaveConfiguration(configuration);
        this.RefreshView();
    }

    public void ManageCustomMods(LaunchConfigurationWithCredentials configuration)
    {
        this.viewManager.ShowView<ModsView>((nameof(ModsView.LaunchConfigurationIdentifier), configuration.Identifier ?? throw new InvalidOperationException("Managed configuration identifier cannot be null")));
    }

    public void ColorChanged(LaunchConfigurationWithCredentials configuration, string newColor)
    {
        configuration.Color = string.IsNullOrWhiteSpace(newColor) ? null : newColor;
        this.launchConfigurationService.SaveConfiguration(configuration);
        this.RefreshView();
    }

    public string GetColorDisplayName(string colorName)
    {
        return string.IsNullOrWhiteSpace(colorName) ? "Default" : colorName;
    }

    public async Task MoveConfigUp(LaunchConfigurationWithCredentials config)
    {
        var currentIndex = this.LaunchConfigurations.FindIndex(c => c.Identifier == config.Identifier);
        if (currentIndex <= 0)
        {
            return;
        }

        this.LaunchConfigurations.RemoveAt(currentIndex);
        this.LaunchConfigurations.Insert(currentIndex - 1, config);
        this.launchConfigurationService.SaveLaunchConfigurations(this.LaunchConfigurations);
        await this.RefreshViewAsync();
        if (config.Identifier is string identifier &&
            this.View is not null)
        {
            await this.View.ScrollToConfigAsync(identifier);
        }
    }

    public async Task MoveConfigDown(LaunchConfigurationWithCredentials config)
    {
        var currentIndex = this.LaunchConfigurations.FindIndex(c => c.Identifier == config.Identifier);
        if (currentIndex < 0 || currentIndex >= this.LaunchConfigurations.Count - 1)
        {
            return;
        }

        this.LaunchConfigurations.RemoveAt(currentIndex);
        this.LaunchConfigurations.Insert(currentIndex + 1, config);
        this.launchConfigurationService.SaveLaunchConfigurations(this.LaunchConfigurations);
        await this.RefreshViewAsync();
        if (config.Identifier is string identifier &&
            this.View is not null)
        {
            await this.View.ScrollToConfigAsync(identifier);
        }
    }
}
