using Daybreak.Configuration.Options;
using Daybreak.Models.Notifications;
using Daybreak.Models.Onboarding;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.InternetChecker;
using Daybreak.Services.Menu;
using Daybreak.Services.Navigation;
using Daybreak.Services.Notifications;
using Daybreak.Services.Onboarding;
using Daybreak.Services.Screens;
using System;
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
    private readonly IConnectivityStatus connectivityStatus;
    private readonly IOnboardingService onboardingService;
    private readonly IApplicationLauncher applicationDetector;
    private readonly IViewManager viewManager;
    private readonly ILiveOptions<LauncherOptions> launcherOptions;
    private readonly ILiveOptions<FocusViewOptions> focusViewOptions;
    private readonly IScreenManager screenManager;

    private CancellationTokenSource cancellationTokenSource = new();
    private bool gameRunning = false;
    private bool internetAvailable = false;

    [GenerateDependencyProperty]
    private bool launchButtonEnabled;

    public LauncherView(
        IMenuService menuService,
        IConnectivityStatus connectivityStatus,
        IOnboardingService onboardingService,
        IApplicationLauncher applicationDetector,
        IViewManager viewManager,
        ILiveOptions<FocusViewOptions> focusViewOptions,
        ILiveOptions<LauncherOptions> launcherOptions,
        IScreenManager screenManager)
    {
        this.menuService = menuService.ThrowIfNull();
        this.connectivityStatus = connectivityStatus.ThrowIfNull();
        this.onboardingService = onboardingService.ThrowIfNull();
        this.screenManager = screenManager.ThrowIfNull();
        this.focusViewOptions = focusViewOptions.ThrowIfNull();
        this.launcherOptions = launcherOptions.ThrowIfNull();
        this.applicationDetector = applicationDetector.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
    }

    private async void PeriodicallyCheckGameState(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await this.Dispatcher.InvokeAsync(this.CheckGameState);
            await Task.Delay(1000, cancellationToken);
        }
    }

    private void CheckGameState()
    {
        this.internetAvailable = this.connectivityStatus.IsInternetAvailable;
        this.gameRunning = this.applicationDetector.IsGuildwarsRunning;
        this.CheckLaunchButtonState();
        this.CheckAndSwitchToFocusView();
    }

    private void CheckAndSwitchToFocusView()
    {
        if (!this.focusViewOptions.Value.Enabled)
        {
            return;
        }

        if (!this.applicationDetector.IsGuildwarsRunning)
        {
            return;
        }

        this.menuService.CloseMenu();
        this.viewManager.ShowView<FocusView>();
    }

    private void CheckLaunchButtonState()
    {
        if (this.internetAvailable is false || this.gameRunning is true)
        {
            this.LaunchButtonEnabled = false;
        }
        else
        {
            this.LaunchButtonEnabled = true;
        }
    }

    private void StartupView_Loaded(object sender, RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource = new CancellationTokenSource();
        this.PeriodicallyCheckGameState(this.cancellationTokenSource.Token);
    }

    private void StartupView_Unloaded(object sender, RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
    }

    private async void LaunchButton_Clicked(object sender, EventArgs e)
    {
        var onboardingStage = await this.onboardingService.CheckOnboardingStage();
        if (onboardingStage is LauncherOnboardingStage.Default)
        {
            throw new InvalidOperationException($"Unexpected onboarding stage {onboardingStage}");
        }

        if (onboardingStage is LauncherOnboardingStage.NeedsCredentials or LauncherOnboardingStage.NeedsExecutable)
        {
            this.viewManager.ShowView<LauncherOnboardingView>(onboardingStage);
            return;
        }

        this.LaunchButtonEnabled = false;
        _ = await this.applicationDetector.LaunchGuildwars();
        this.CheckGameState();
    }
}
