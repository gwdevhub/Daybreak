using Daybreak.Configuration.Options;
using Daybreak.Models.LaunchConfigurations;
using Daybreak.Models.Onboarding;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.InternetChecker;
using Daybreak.Services.LaunchConfigurations;
using Daybreak.Services.Menu;
using Daybreak.Services.Navigation;
using Daybreak.Services.Onboarding;
using Daybreak.Services.Screens;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for LauncherView.xaml
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
public partial class LauncherView : UserControl
{
    private readonly IMenuService menuService;
    private readonly ILaunchConfigurationService launchConfigurationService;
    private readonly IConnectivityStatus connectivityStatus;
    private readonly IOnboardingService onboardingService;
    private readonly IApplicationLauncher applicationLauncher;
    private readonly IViewManager viewManager;
    private readonly IScreenManager screenManager;
    private readonly ILiveOptions<FocusViewOptions> focusViewOptions;

    [GenerateDependencyProperty]
    private LaunchConfigurationWithCredentials latestConfiguration = default!;

    [GenerateDependencyProperty]
    private bool loading;

    public ObservableCollection<LaunchConfigurationWithCredentials> LaunchConfigurations { get; } = new();

    public LauncherView(
        IMenuService menuService,
        ILaunchConfigurationService launchConfigurationService,
        IConnectivityStatus connectivityStatus,
        IOnboardingService onboardingService,
        IApplicationLauncher applicationLauncher,
        IViewManager viewManager,
        IScreenManager screenManager,
        ILiveOptions<FocusViewOptions> focusViewOptions)
    {
        this.menuService = menuService.ThrowIfNull();
        this.launchConfigurationService = launchConfigurationService.ThrowIfNull();
        this.connectivityStatus = connectivityStatus.ThrowIfNull();
        this.onboardingService = onboardingService.ThrowIfNull();
        this.screenManager = screenManager.ThrowIfNull();
        this.applicationLauncher = applicationLauncher.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.focusViewOptions = focusViewOptions.ThrowIfNull();
        this.InitializeComponent();
    }

    private void CheckOnboardingState()
    {
        var onboardingStage = this.onboardingService.CheckOnboardingStage();
        if (onboardingStage is LauncherOnboardingStage.Default)
        {
            throw new InvalidOperationException($"Unexpected onboarding stage {onboardingStage}");
        }

        if (onboardingStage is LauncherOnboardingStage.NeedsCredentials or LauncherOnboardingStage.NeedsExecutable or LauncherOnboardingStage.NeedsConfiguration)
        {
            this.viewManager.ShowView<LauncherOnboardingView>(onboardingStage);
            return;
        }
    }

    private void RetrieveLaunchConfigurations()
    {
        this.LaunchConfigurations.ClearAnd().AddRange(this.launchConfigurationService.GetLaunchConfigurations());
        this.LatestConfiguration = this.launchConfigurationService.GetLastLaunchConfigurationWithCredentials();
    }

    private void StartupView_Loaded(object sender, RoutedEventArgs e)
    {
        this.CheckOnboardingState();
        this.RetrieveLaunchConfigurations();
    }

    private void StartupView_Unloaded(object sender, RoutedEventArgs e)
    {
    }

    private async void SplitButton_Click(object sender, RoutedEventArgs e)
    {
        await this.Dispatcher.InvokeAsync(() => this.Loading = true);
        if (this.LatestConfiguration is null)
        {
            await this.Dispatcher.InvokeAsync(() => this.Loading = false);
            return;
        }

        if (this.applicationLauncher.GetGuildwarsProcess(this.LatestConfiguration) is GuildWarsApplicationLaunchContext context)
        {
            // Detected already running guildwars process
            await this.Dispatcher.InvokeAsync(() => this.Loading = false);
            if (this.focusViewOptions.Value.Enabled)
            {
                this.menuService.CloseMenu();
                this.viewManager.ShowView<FocusView>(context);
            }

            return;
        }

        try
        {
            var launchedContext = await this.applicationLauncher.LaunchGuildwars(this.LatestConfiguration);
            if (launchedContext is null)
            {
                await this.Dispatcher.InvokeAsync(() => this.Loading = false);
                return;
            }

            this.launchConfigurationService.SetLastLaunchConfigurationWithCredentials(this.LatestConfiguration);
            if (this.focusViewOptions.Value.Enabled)
            {
                await this.Dispatcher.InvokeAsync(() => this.Loading = false);
                this.menuService.CloseMenu();
                this.viewManager.ShowView<FocusView>(launchedContext);
            }
        }
        catch (Exception)
        {
        }

        await this.Dispatcher.InvokeAsync(() => this.Loading = false);
    }
}
