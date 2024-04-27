using Daybreak.Models;
using Daybreak.Models.LaunchConfigurations;
using Daybreak.Services.Credentials;
using Daybreak.Services.ExecutableManagement;
using Daybreak.Services.LaunchConfigurations;
using Daybreak.Services.Navigation;
using Daybreak.Services.Notifications;
using System;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Extensions;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Launch;
/// <summary>
/// Interaction logic for LaunchConfigurationView.xaml
/// </summary>
public partial class LaunchConfigurationView : UserControl
{
    private readonly INotificationService notificationService;
    private readonly ILaunchConfigurationService launchConfigurationService;
    private readonly IGuildWarsExecutableManager guildWarsExecutableManager;
    private readonly ICredentialManager credentialManager;
    private readonly IViewManager viewManager;

    [GenerateDependencyProperty]
    private LoginCredentials selectedCredentials = default!;
    [GenerateDependencyProperty]
    private string selectedPath = default!;
    [GenerateDependencyProperty]
    private string launchArguments = default!;

    public ObservableCollection<LoginCredentials> Credentials { get; set; } = [];
    public ObservableCollection<string> ExecutablePaths { get; set; } = [];

    public LaunchConfigurationView(
        INotificationService notificationService,
        ILaunchConfigurationService launchConfigurationService,
        IGuildWarsExecutableManager guildWarsExecutableManager,
        ICredentialManager credentialManager,
        IViewManager viewManager)
    {
        this.notificationService = notificationService.ThrowIfNull();
        this.launchConfigurationService = launchConfigurationService.ThrowIfNull();
        this.guildWarsExecutableManager = guildWarsExecutableManager.ThrowIfNull();
        this.credentialManager = credentialManager.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();

        this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (this.DataContext is not LaunchConfigurationWithCredentials config)
        {
            throw new InvalidOperationException($"{nameof(this.DataContext)} is not set to a {nameof(LaunchConfigurationWithCredentials)}");
        }

        this.Credentials.ClearAnd().AddRange(this.credentialManager.GetCredentialList());
        this.ExecutablePaths.ClearAnd().AddRange(this.guildWarsExecutableManager.GetExecutableList());
        if (config.Credentials is not null)
        {
            this.SelectedCredentials = config.Credentials;
        }
        
        if (config.ExecutablePath?.IsNullOrWhiteSpace() is false)
        {
            this.SelectedPath = config.ExecutablePath;
        }

        if (config.Arguments?.IsNullOrWhiteSpace() is false)
        {
            this.LaunchArguments = config.Arguments;
        }
    }

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<LaunchConfigurationsView>();
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (this.DataContext is not LaunchConfigurationWithCredentials config)
        {
            return;
        }

        config.Credentials = this.SelectedCredentials;
        config.ExecutablePath = this.SelectedPath;
        config.Arguments = this.LaunchArguments;
        if (!this.launchConfigurationService.SaveConfiguration(config))
        {
            this.notificationService.NotifyInformation(
                title: "Failed to save configuration",
                description: "Please review the configuration and make sure that you have assigned an executable and a set of credentials to the configuration");
            return;
        }

        this.viewManager.ShowView<LaunchConfigurationsView>();
    }
}
