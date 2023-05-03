using Daybreak.Configuration.Options;
using Daybreak.Models.Onboarding;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.Navigation;
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
    private readonly IOnboardingService onboardingService;
    private readonly IApplicationLauncher applicationDetector;
    private readonly IViewManager viewManager;
    private readonly ILiveOptions<LauncherOptions> launcherOptions;
    private readonly ILiveOptions<FocusViewOptions> focusViewOptions;
    private readonly IScreenManager screenManager;

    private CancellationTokenSource cancellationTokenSource = new();
    private bool leftBrowserMaximized = false;
    private bool rightBrowserMaximized = false;

    [GenerateDependencyProperty(InitialValue = true)]
    private bool buttonsVisible;
    [GenerateDependencyProperty]
    private bool launchButtonEnabled;

    public LauncherView(
        IOnboardingService onboardingService,
        IApplicationLauncher applicationDetector,
        IViewManager viewManager,
        ILiveOptions<FocusViewOptions> focusViewOptions,
        ILiveOptions<LauncherOptions> launcherOptions,
        IScreenManager screenManager)
    {
        this.onboardingService = onboardingService.ThrowIfNull();
        this.screenManager = screenManager.ThrowIfNull();
        this.focusViewOptions = focusViewOptions.ThrowIfNull();
        this.launcherOptions = launcherOptions.ThrowIfNull();
        this.applicationDetector = applicationDetector.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
    }

    private void PeriodicallyCheckGameState()
    {
        this.cancellationTokenSource = new CancellationTokenSource();
        System.Extensions.TaskExtensions.RunPeriodicAsync(() => this.Dispatcher.Invoke(this.CheckGameState), TimeSpan.Zero, TimeSpan.FromSeconds(1), this.cancellationTokenSource.Token);
    }

    private void CheckGameState()
    {
        this.LaunchButtonEnabled = this.applicationDetector.IsGuildwarsRunning is false;
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

        this.viewManager.ShowView<FocusView>();
    }

    private void StartupView_Loaded(object sender, RoutedEventArgs e)
    {
        this.PeriodicallyCheckGameState();
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

        var maybeProcess = await this.applicationDetector.LaunchGuildwars();
        if (maybeProcess is null)
        {
            return;
        }

        if (this.launcherOptions.Value.SetGuildwarsWindowSizeOnLaunch)
        {
            var id = this.launcherOptions.Value.DesiredGuildwarsScreen;
            var desiredScreen = this.screenManager.Screens.Skip(id).FirstOrDefault();
            if (desiredScreen is null)
            {
                throw new InvalidOperationException($"Unable to set guildwars on desired screen. No screen with id {id}");
            }

            await Task.Delay(1000);
            this.screenManager.MoveGuildwarsToScreen(desiredScreen);
        }
    }
}
