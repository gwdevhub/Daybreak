using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.FocusView;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Models.Onboarding;
using Daybreak.Shared.Services.Api;
using Daybreak.Shared.Services.ApplicationLauncher;
using Daybreak.Shared.Services.InternetChecker;
using Daybreak.Shared.Services.LaunchConfigurations;
using Daybreak.Shared.Services.Menu;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Onboarding;
using Daybreak.Shared.Services.Screens;
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
    private readonly INotificationService notificationService;
    private readonly IDaybreakApiService daybreakApiService;
    private readonly IMenuService menuService;
    private readonly ILaunchConfigurationService launchConfigurationService;
    private readonly IConnectivityStatus connectivityStatus;
    private readonly IOnboardingService onboardingService;
    private readonly IApplicationLauncher applicationLauncher;
    private readonly IViewManager viewManager;
    private readonly IScreenManager screenManager;
    private readonly ILiveOptions<FocusViewOptions> focusViewOptions;

    private CancellationTokenSource? cancellationTokenSource;
    private bool launching;

    [GenerateDependencyProperty]
    private LauncherViewContext latestConfiguration = default!;

    [GenerateDependencyProperty]
    private bool canLaunch;

    public ObservableCollection<LauncherViewContext> LaunchConfigurations { get; } = [];

    public LauncherView(
        INotificationService notificationService,
        IDaybreakApiService daybreakApiService,
        IMenuService menuService,
        ILaunchConfigurationService launchConfigurationService,
        IConnectivityStatus connectivityStatus,
        IOnboardingService onboardingService,
        IApplicationLauncher applicationLauncher,
        IViewManager viewManager,
        IScreenManager screenManager,
        ILiveOptions<FocusViewOptions> focusViewOptions)
    {
        this.notificationService = notificationService.ThrowIfNull();
        this.daybreakApiService = daybreakApiService.ThrowIfNull();
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
        var latestLaunchConfiguration = this.DataContext is LaunchConfigurationWithCredentials desiredConfig ?
            desiredConfig :
            this.launchConfigurationService.GetLastLaunchConfigurationWithCredentials();
        this.LaunchConfigurations.ClearAnd().AddRange(this.launchConfigurationService.GetLaunchConfigurations().Select(c => new LauncherViewContext { Configuration = c, CanLaunch = false }));
        this.LatestConfiguration = this.LaunchConfigurations.FirstOrDefault(c => c.Configuration?.Equals(latestLaunchConfiguration) is true);

        if (this.DataContext is LaunchConfigurationWithCredentials)
        {
            this.DropDownButton_Clicked(this, default!);
        }
    }

    private async Task PeriodicallyCheckSelectedConfigState(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (!this.launching)
            {
                await this.Dispatcher.InvokeAsync(() => this.SetLaunchButtonState(cancellationToken));
            }

            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
    }

    private void StartupView_Loaded(object sender, RoutedEventArgs e)
    {
        if (!this.IsOnboarded())
        {
            return;
        }

        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = this.cancellationTokenSource.Token;
        this.RetrieveLaunchConfigurations();
        Task.Factory.StartNew(() => this.PeriodicallyCheckSelectedConfigState(cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    private void StartupView_Unloaded(object sender, RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource?.Dispose();
    }

    private void DropDownButton_SelectionChanged(object _, object e)
    {
        if (e is not LauncherViewContext)
        {
            return;
        }

        this.CanLaunch = false;
    }

    private async void DropDownButton_Clicked(object _, object __)
    {
        this.launching = true;
        await this.Dispatcher.InvokeAsync(() => this.CanLaunch = false);

        if (this.LatestConfiguration.CanKill)
        {
            var killingTask = await new TaskFactory().StartNew(this.KillGuildWars, TaskCreationOptions.LongRunning);
            try
            {
                await killingTask;
            }
            catch
            {
            }
        }
        else
        {
            var launchingTask = await new TaskFactory().StartNew(() => this.LaunchGuildWars(this.cancellationTokenSource?.Token ?? CancellationToken.None), TaskCreationOptions.LongRunning);
            try
            {
                await launchingTask;
            }
            catch
            {
            }
        }

        this.launching = false;
    }

    private void SetLaunchButtonState(CancellationToken cancellationToken)
    {
        if (this.LatestConfiguration is null)
        {
            this.CanLaunch = false;
            return;
        }

        if (this.LatestConfiguration.CanKill ||
            this.LatestConfiguration.CanLaunch ||
            this.LatestConfiguration.CanAttach)
        {
            this.CanLaunch = true;

        }

        foreach(var config in this.LaunchConfigurations)
        {
            var isSelected = config == this.LatestConfiguration;
            Task.Factory.StartNew(() => this.CheckGameState(config, isSelected, cancellationToken), cancellationToken);
        }
    }

    private async Task KillGuildWars()
    {
        var latestConfig = await this.Dispatcher.InvokeAsync(() => this.LatestConfiguration);
        var context = this.applicationLauncher.GetGuildwarsProcess(latestConfig.Configuration!);
        if (context is null)
        {
            return;
        }

        this.applicationLauncher.KillGuildWarsProcess(context);
    }

    private async Task LaunchGuildWars(CancellationToken cancellationToken)
    {
        var latestConfig = await this.Dispatcher.InvokeAsync(() => this.LatestConfiguration);
        if (latestConfig.Configuration is null)
        {
            return;
        }

        if (this.applicationLauncher.GetGuildwarsProcess(latestConfig.Configuration) is GuildWarsApplicationLaunchContext context)
        {
            try
            {
                // Detected already running guildwars process
                await this.Dispatcher.InvokeAsync(() => this.CanLaunch = false);
                if (this.focusViewOptions.Value.Enabled)
                {
                    var notificationToken = this.notificationService.NotifyInformation(
                            title: "Attaching to Guild Wars process...",
                            description: "Attempting to attach to Guild Wars process");
                    var apiContext = await this.daybreakApiService.AttachDaybreakApiContext(context, cancellationToken);
                    notificationToken.Cancel();

                    if (apiContext is null)
                    {
                        this.notificationService.NotifyError(
                            title: "Could not attach to Guild Wars",
                            description: "Could not find the Api context to attach to Guild Wars. Check the logs for more details");
                        await this.Dispatcher.InvokeAsync(() => this.CanLaunch = true);
                    }
                    else
                    {
                        this.viewManager.ShowView<FocusView>(new FocusViewContext { ApiContext = apiContext, LaunchContext = context });
                        this.menuService.CloseMenu();
                    }
                }

                this.launchConfigurationService.SetLastLaunchConfigurationWithCredentials(latestConfig.Configuration);
                return;
            }
            catch(Exception)
            {
                await this.Dispatcher.InvokeAsync(() => this.CanLaunch = true);
            }
        }
        else
        {
            try
            {
                var launchedContext = await this.applicationLauncher.LaunchGuildwars(latestConfig.Configuration, cancellationToken);
                await this.Dispatcher.InvokeAsync(() => this.CanLaunch = false);
                if (launchedContext is null)
                {
                    return;
                }

                this.launchConfigurationService.SetLastLaunchConfigurationWithCredentials(latestConfig.Configuration);
                if (this.focusViewOptions.Value.Enabled)
                {
                    var notificationToken = this.notificationService.NotifyInformation(
                        title: "Attaching to Guild Wars process...",
                        description: "Attempting to attach to Guild Wars process");
                    var apiContext = await this.daybreakApiService.AttachDaybreakApiContext(launchedContext, cancellationToken);
                    notificationToken.Cancel();

                    if (apiContext is null)
                    {
                        this.notificationService.NotifyError(
                            title: "Could not attach to Guild Wars",
                            description: "Could not find the Api context to attach to Guild Wars. Check the logs for more details");
                        await this.Dispatcher.InvokeAsync(() => this.CanLaunch = true);
                    }
                    else
                    {
                        this.viewManager.ShowView<FocusView>(new FocusViewContext { ApiContext = apiContext, LaunchContext = launchedContext });
                        this.menuService.CloseMenu();
                    }
                }
            }
            catch (Exception)
            {
                await this.Dispatcher.InvokeAsync(() => this.CanLaunch = true);
            }
        }
    }

    private async Task CheckGameState(LauncherViewContext launcherViewContext, bool isSelected, CancellationToken cancellationToken)
    {
        if (launcherViewContext.Configuration is null)
        {
            launcherViewContext.GameRunning = false;
            launcherViewContext.CanLaunch = false;
            launcherViewContext.CanAttach = false;
            launcherViewContext.CanKill = false;
            return;
        }

        if (this.applicationLauncher.GetGuildwarsProcess(launcherViewContext.Configuration) is not GuildWarsApplicationLaunchContext context)
        {
            launcherViewContext.GameRunning = false;
            launcherViewContext.CanLaunch = true;
            launcherViewContext.CanAttach = false;
            launcherViewContext.CanKill = false;
            return;
        }
        else
        {
            launcherViewContext.GameRunning = true;
        }

        // If FocusView is disabled, don't initialize memory scanner, instead just allow the user to kill the game
        if (!this.focusViewOptions.Value.Enabled ||
            !isSelected)
        {
            launcherViewContext.GameRunning = true;
            launcherViewContext.CanLaunch = false;
            launcherViewContext.CanAttach = false;
            launcherViewContext.CanKill = false;
            return;
        }

        if (await this.daybreakApiService.GetDaybreakApiContext(context.GuildWarsProcess, cancellationToken) is not ScopedApiContext apiContext)
        {
            launcherViewContext.GameRunning = false;
            launcherViewContext.CanLaunch = false;
            launcherViewContext.CanAttach = false;
            launcherViewContext.CanKill = true;
            return;
        }

        if (!await apiContext.IsAvailable(cancellationToken))
        {
            launcherViewContext.CanKill = true;
            launcherViewContext.GameRunning = true;
            launcherViewContext.CanLaunch = false;
            launcherViewContext.CanAttach = false;
            return;
        }

        var mainPlayerInfo = await apiContext.GetMainPlayerInfo(cancellationToken);
        if (mainPlayerInfo?.Email != context.LaunchConfiguration.Credentials?.Username)
        {
            launcherViewContext.GameRunning = false;
            launcherViewContext.CanAttach = false;
            launcherViewContext.CanLaunch = false;
            launcherViewContext.CanKill = true;
            return;
        }

        launcherViewContext.GameRunning = true;
        launcherViewContext.CanAttach = this.focusViewOptions.Value.Enabled;
        launcherViewContext.CanLaunch = false;
        launcherViewContext.CanKill = false;
    }
}
