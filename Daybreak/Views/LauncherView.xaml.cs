﻿using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.FocusView;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Models.Onboarding;
using Daybreak.Shared.Services.Api;
using Daybreak.Shared.Services.ApplicationLauncher;
using Daybreak.Shared.Services.InternetChecker;
using Daybreak.Shared.Services.LaunchConfigurations;
using Daybreak.Shared.Services.Menu;
using Daybreak.Shared.Services.Mods;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Onboarding;
using Daybreak.Shared.Services.Screens;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
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

    [GenerateDependencyProperty(InitialValue = true)]
    private bool reApplyButtonEnabled = true;

    [GenerateDependencyProperty]
    private bool canReapply;

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
            var launchingTask = Task.Factory.StartNew(() => this.LaunchGuildWars(this.cancellationTokenSource?.Token ?? CancellationToken.None), TaskCreationOptions.LongRunning).Unwrap();
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

    private async void ReapplyButton_Clicked(object _, EventArgs __)
    {
        if (this.LatestConfiguration is LauncherViewContext context &&
            context.ModsToReapply?.Any() is true)
        {
            await Task.Factory.StartNew(() => this.ReApplyMods(context, this.cancellationTokenSource?.Token ?? CancellationToken.None), TaskCreationOptions.LongRunning);
        }
    }

    private async ValueTask SetLaunchButtonState(CancellationToken cancellationToken)
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
            this.CanLaunch = !this.launching;

        }

        foreach(var config in this.LaunchConfigurations)
        {
            var isSelected = config == this.LatestConfiguration;
            await Task.Factory.StartNew(() => this.CheckGameState(config, isSelected, cancellationToken), cancellationToken);
        }
    }

    private async Task KillGuildWars()
    {
        var latestConfig = await this.Dispatcher.InvokeAsync(() => this.LatestConfiguration);
        if (latestConfig.AppContext is null)
        {
            return;
        }

        this.applicationLauncher.KillGuildWarsProcess(latestConfig.AppContext);
    }

    private async Task LaunchGuildWars(CancellationToken cancellationToken)
    {
        var latestConfig = await this.Dispatcher.InvokeAsync(() => this.LatestConfiguration);
        if (latestConfig.Configuration is null)
        {
            return;
        }

        this.launchConfigurationService.SetLastLaunchConfigurationWithCredentials(latestConfig.Configuration);
        await this.Dispatcher.InvokeAsync(() => this.CanLaunch = false);
        try
        {
            if (latestConfig.AppContext is not null)
            {
                await this.AttachContext(latestConfig.AppContext, cancellationToken);
            }
            else if (latestConfig.ApiContext is not null)
            {
                await this.AttachContext(latestConfig, latestConfig.ApiContext, cancellationToken);
            }
            else
            {
                await this.LaunchContext(latestConfig, cancellationToken);
            }
        }
        catch (Exception)
        {
            await this.Dispatcher.InvokeAsync(() => this.CanLaunch = true);
        }
    }

    private async Task CheckGameState(LauncherViewContext launcherViewContext, bool isSelected, CancellationToken cancellationToken)
    {
        if (launcherViewContext.Configuration is null ||
            launcherViewContext.Configuration.Credentials is null ||
            !isSelected)
        {
            launcherViewContext.GameRunning = false;
            launcherViewContext.CanLaunch = false;
            launcherViewContext.CanAttach = false;
            launcherViewContext.CanKill = false;
            launcherViewContext.AppContext = default;
            launcherViewContext.ApiContext = default;
            return;
        }

        if (this.applicationLauncher.GetGuildwarsProcesses().Count == 0)
        {
            launcherViewContext.GameRunning = false;
            launcherViewContext.CanLaunch = true;
            launcherViewContext.CanAttach = false;
            launcherViewContext.CanKill = false;
            launcherViewContext.AppContext = default;
            launcherViewContext.ApiContext = default;
            return;
        }

        (var appContext, var apiContext) = await this.GetAppAndApiContext(launcherViewContext, cancellationToken);
        if (appContext is not null)
        {
            launcherViewContext.ModsToReapply = await this.CheckModsToReapply(launcherViewContext, appContext, cancellationToken).ConfigureAwait(true);
            if (apiContext is null)
            {
                launcherViewContext.AppContext = appContext;
                launcherViewContext.ApiContext = apiContext;
                launcherViewContext.GameRunning = false;
                launcherViewContext.CanLaunch = false;
                launcherViewContext.CanAttach = false;
                launcherViewContext.CanKill = true;
            }
            else
            {
                launcherViewContext.AppContext = appContext;
                launcherViewContext.ApiContext = apiContext;
                launcherViewContext.GameRunning = true;
                launcherViewContext.CanLaunch = false;
                launcherViewContext.CanAttach = this.focusViewOptions.Value.Enabled;
                launcherViewContext.CanKill = !this.focusViewOptions.Value.Enabled;
            }
        }
        else
        {
            if (apiContext is null)
            {
                launcherViewContext.AppContext = appContext;
                launcherViewContext.ApiContext = apiContext;
                launcherViewContext.GameRunning = false;
                launcherViewContext.CanLaunch = true;
                launcherViewContext.CanAttach = false;
                launcherViewContext.CanKill = false;
            }
            else
            {
                var processIdResponse = await apiContext.GetProcessId(cancellationToken);
                if (processIdResponse is not null)
                {
                    launcherViewContext.AppContext = new GuildWarsApplicationLaunchContext
                    {
                        GuildWarsProcess = Process.GetProcessById(processIdResponse.ProcessId),
                        ProcessId = (uint)processIdResponse.ProcessId,
                        LaunchConfiguration = launcherViewContext.Configuration
                    };

                    launcherViewContext.ModsToReapply = await this.CheckModsToReapply(launcherViewContext, launcherViewContext.AppContext, cancellationToken).ConfigureAwait(true);
                    launcherViewContext.CanKill = !this.focusViewOptions.Value.Enabled;
                }
                else
                {
                    launcherViewContext.CanKill = false;
                }

                launcherViewContext.ApiContext = apiContext;
                launcherViewContext.GameRunning = true;
                launcherViewContext.CanLaunch = false;
                launcherViewContext.CanAttach = this.focusViewOptions.Value.Enabled;
            }
        }
    }

    private async Task<(GuildWarsApplicationLaunchContext? AppContext, ScopedApiContext? ApiContext)> GetAppAndApiContext(LauncherViewContext launcherViewContext, CancellationToken cancellationToken)
    {
        if (launcherViewContext.Configuration is null ||
            launcherViewContext.Configuration.Credentials is null)
        {
            return default;
        }

        var processContext = this.applicationLauncher.GetGuildwarsProcess(launcherViewContext.Configuration);
        if (processContext is not null)
        {
            var maybeApiContext = await this.daybreakApiService.GetDaybreakApiContext(processContext.GuildWarsProcess, cancellationToken);

            // If the API is available but it does not belong to the desired user, return null
            if (maybeApiContext is not null &&
                await maybeApiContext.GetLoginInfo(cancellationToken) is LoginInfo loginInfo &&
                loginInfo.Email != launcherViewContext.Configuration.Credentials.Username)
            {
                return (processContext, default);
            }

            return (processContext, maybeApiContext);
        }

        var apiContext = await this.daybreakApiService.FindDaybreakApiContextByCredentials(launcherViewContext.Configuration.Credentials, cancellationToken);
        return (processContext, apiContext);
    }

    private async Task AttachContext(GuildWarsApplicationLaunchContext context, CancellationToken cancellationToken)
    {
        if (!this.focusViewOptions.Value.Enabled)
        {
            return;
        }

        using var notificationToken = this.notificationService.NotifyInformation(
                    title: "Attaching to Guild Wars process...",
                    description: "Attempting to attach to Guild Wars process");
        var apiContext = await this.daybreakApiService.AttachDaybreakApiContext(context, cancellationToken);

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

    private async Task AttachContext(LauncherViewContext launcherViewContext, ScopedApiContext apiContext, CancellationToken cancellationToken)
    {
        if (launcherViewContext.Configuration is null)
        {
            return;
        }

        if (!this.focusViewOptions.Value.Enabled)
        {
            return;
        }

        using var notificationToken = this.notificationService.NotifyInformation(
                    title: "Attaching to Guild Wars process...",
                    description: "Attempting to attach to Guild Wars process");

        var processIdResponse = await apiContext.GetProcessId(cancellationToken);
        if (processIdResponse is null ||
            Process.GetProcessById(processIdResponse.ProcessId) is not Process guildWarsProcess)
        {
            notificationToken.Cancel();
            this.notificationService.NotifyError(
                title: "Could not attach to Guild Wars",
                description: "Could not find the Api context to attach to Guild Wars. Check the logs for more details");
            await this.Dispatcher.InvokeAsync(() => this.CanLaunch = true);
            return;
        }

        var launchContext = new GuildWarsApplicationLaunchContext
        {
            ProcessId = (uint)processIdResponse.ProcessId,
            GuildWarsProcess = guildWarsProcess,
            LaunchConfiguration = launcherViewContext.Configuration
        };

        await this.daybreakApiService.AttachDaybreakApiContext(launchContext, apiContext, cancellationToken);
        this.launchConfigurationService.SetLastLaunchConfigurationWithCredentials(launcherViewContext.Configuration);
        this.viewManager.ShowView<FocusView>(new FocusViewContext { ApiContext = apiContext, LaunchContext = launchContext });
        this.menuService.CloseMenu();
    }

    private async Task LaunchContext(LauncherViewContext launcherViewContext, CancellationToken cancellationToken)
    {
        if (launcherViewContext.Configuration is null)
        {
            return;
        }

        var launchNotificationToken = this.notificationService.NotifyInformation(
                    title: "Launching Guild Wars...",
                    description: $"Attempting to launch Guild Wars process at {launcherViewContext.Configuration.ExecutablePath}",
                    expirationTime: DateTime.MaxValue);
        var launchedContext = await this.applicationLauncher.LaunchGuildwars(launcherViewContext.Configuration, cancellationToken);
        if (launchedContext is null)
        {
            this.notificationService.NotifyError(
                title: "Could not launch Guild Wars",
                description: $"Could not launch Guild Wars at {launcherViewContext.Configuration.ExecutablePath}. Check the logs for more details");
            launchNotificationToken.Cancel();
            return;
        }

        launchNotificationToken.Cancel();
        this.daybreakApiService.RequestInstancesAnnouncement();
        this.launchConfigurationService.SetLastLaunchConfigurationWithCredentials(launcherViewContext.Configuration);
        if (!this.focusViewOptions.Value.Enabled)
        {
            return;
        }

        var attachNotificationToken = this.notificationService.NotifyInformation(
                title: "Attaching to Guild Wars process...",
                description: "Attempting to attach to Guild Wars process");

        //Wait 1 second to allow the launched Guild Wars process to advertise itself
        await Task.Delay(1000, cancellationToken);
        var apiContext = await this.daybreakApiService.AttachDaybreakApiContext(launchedContext, cancellationToken);
        attachNotificationToken.Cancel();

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

    private async Task<IEnumerable<IModService>> CheckModsToReapply(LauncherViewContext launcherViewContext, GuildWarsApplicationLaunchContext appContext, CancellationToken cancellationToken)
    {
        var currentMods = launcherViewContext.ModsToReapply?.ToList() ?? [];
        var mods = (await this.applicationLauncher.CheckMods(appContext, cancellationToken)).ToList();
        if (mods.Any(m => !currentMods.Contains(m)))
        {
            this.notificationService.NotifyInformation(
                title: "Mods changed",
                description: "The mods have changed since Guild Wars was launched. Please click 'Reapply mods' to reapply the mod configuration");
        }

        if (mods.Count > 0)
        {
            await this.Dispatcher.InvokeAsync(() => this.CanReapply = true);
        }
        else
        {
            await this.Dispatcher.InvokeAsync(() => this.CanReapply = false);
        }

        return mods;
    }

    private async Task ReApplyMods(LauncherViewContext launcherViewContext, CancellationToken cancellationToken)
    {
        var modsToApply = launcherViewContext.ModsToReapply?.ToList() ?? [];
        if (modsToApply.Count == 0 ||
            launcherViewContext.AppContext is null)
        {
            return;
        }

        await this.Dispatcher.InvokeAsync(() => this.ReApplyButtonEnabled = false);
        using var token = this.notificationService.NotifyInformation(
            title: "Reapplying mods",
            description: "Reapplying mods to the running Guild Wars process. This may take a few seconds");
        if (!await this.applicationLauncher.ReapplyMods(launcherViewContext.AppContext, modsToApply, cancellationToken))
        {
            this.notificationService.NotifyError(
                title: "Could not reapply mods",
                description: "Could not reapply the mods to the running Guild Wars process. Check the logs for more details");
        }
        else
        {
            this.notificationService.NotifyInformation(
                title: "Mods have been reapplied",
                description: "Mods have been reapplied to the running Guild Wars process");
        }

        await this.Dispatcher.InvokeAsync(() => this.ReApplyButtonEnabled = true);
    }
}
