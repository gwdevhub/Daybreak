using Daybreak.Configuration.Options;
using Daybreak.Models;
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
    private readonly CancellationTokenSource cancellationTokenSource = new();

    [GenerateDependencyProperty]
    private LauncherViewContext latestConfiguration = default!;

    [GenerateDependencyProperty]
    private bool canLaunch;

    public ObservableCollection<LauncherViewContext> LaunchConfigurations { get; } = [];

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

    private bool IsOnboarded()
    {
        var onboardingStage = this.onboardingService.CheckOnboardingStage();
        if (onboardingStage is LauncherOnboardingStage.Default)
        {
            throw new InvalidOperationException($"Unexpected onboarding stage {onboardingStage}");
        }

        if (onboardingStage is LauncherOnboardingStage.NeedsCredentials or LauncherOnboardingStage.NeedsExecutable or LauncherOnboardingStage.NeedsConfiguration)
        {
            this.viewManager.ShowView<LauncherOnboardingView>(onboardingStage);
            return false;
        }

        return true;
    }

    private void RetrieveLaunchConfigurations()
    {
        var latestLaunchConfiguration = this.launchConfigurationService.GetLastLaunchConfigurationWithCredentials();
        this.LaunchConfigurations.ClearAnd().AddRange(this.launchConfigurationService.GetLaunchConfigurations().Select(c => new LauncherViewContext { Configuration = c, CanLaunch = false }));
        this.LatestConfiguration = this.LaunchConfigurations.FirstOrDefault(c => c.Configuration?.Equals(latestLaunchConfiguration) is true);
    }

    private async void PeriodicallyCheckSelectedConfigState()
    {
        while (!this.cancellationTokenSource.IsCancellationRequested)
        {
            await this.Dispatcher.InvokeAsync(() => this.CanLaunch = this.LatestConfiguration?.CanLaunch ?? false);
            await Task.Delay(TimeSpan.FromSeconds(1), this.cancellationTokenSource.Token);
        }
    }

    private void StartupView_Loaded(object sender, RoutedEventArgs e)
    {
        if (!this.IsOnboarded())
        {
            return;
        }

        this.RetrieveLaunchConfigurations();
        this.PeriodicallyCheckSelectedConfigState();
    }

    private void StartupView_Unloaded(object sender, RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource?.Dispose();
    }

    private async void DropDownButton_SelectionChanged(object _, object e)
    {
        if (e is not LauncherViewContext context)
        {
            return;
        }

        if (this.LatestConfiguration is null ||
            this.LatestConfiguration.CanLaunch is false)
        {
            await this.Dispatcher.InvokeAsync(() => this.CanLaunch = false);
        }
        else
        {
            await this.Dispatcher.InvokeAsync(() => this.CanLaunch = true);
        }
    }

    private async void DropDownButton_Clicked(object _, object e)
    {
        await this.Dispatcher.InvokeAsync(() => this.CanLaunch = true);
        if (this.LatestConfiguration is null ||
            this.LatestConfiguration.CanLaunch is false)
        {
            await this.Dispatcher.InvokeAsync(() => this.CanLaunch = false);
            return;
        }

        var launchingTask = await new TaskFactory().StartNew(async () =>
        {
            var latestConfig = await this.Dispatcher.InvokeAsync(() => this.LatestConfiguration);
            if (this.applicationLauncher.GetGuildwarsProcess(latestConfig.Configuration!) is GuildWarsApplicationLaunchContext context)
            {
                // Detected already running guildwars process
                await this.Dispatcher.InvokeAsync(() => this.CanLaunch = false);
                if (this.focusViewOptions.Value.Enabled)
                {
                    this.menuService.CloseMenu();
                    this.viewManager.ShowView<FocusView>(context);
                }

                this.launchConfigurationService.SetLastLaunchConfigurationWithCredentials(latestConfig.Configuration!);
                return;
            }

            try
            {
                var launchedContext = await this.applicationLauncher.LaunchGuildwars(latestConfig.Configuration!);
                if (launchedContext is null)
                {
                    await this.Dispatcher.InvokeAsync(() => this.CanLaunch = false);
                    return;
                }

                this.launchConfigurationService.SetLastLaunchConfigurationWithCredentials(latestConfig.Configuration!);
                if (this.focusViewOptions.Value.Enabled)
                {
                    await this.Dispatcher.InvokeAsync(() => this.CanLaunch = false);
                    this.menuService.CloseMenu();
                    this.viewManager.ShowView<FocusView>(launchedContext);
                }
            }
            catch (Exception)
            {
            }
        }, TaskCreationOptions.LongRunning);

        await launchingTask;
        await this.Dispatcher.InvokeAsync(() => this.CanLaunch = false);
    }
}
