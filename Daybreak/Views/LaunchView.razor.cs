using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Models.Onboarding;
using Daybreak.Shared.Services.Api;
using Daybreak.Shared.Services.ApplicationLauncher;
using Daybreak.Shared.Services.LaunchConfigurations;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Onboarding;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Core.Extensions;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class LaunchViewModel : ViewModelBase<LaunchViewModel, LaunchView>, INotifyPropertyChanged, IDisposable
{
    private static readonly TimeSpan LaunchTimeout = TimeSpan.FromSeconds(10);

    private readonly IViewManager viewManager;
    private readonly INotificationService notificationService;
    private readonly IDaybreakApiService daybreakApiService;
    private readonly ILaunchConfigurationService launchConfigurationService;
    private readonly IOnboardingService onboardingService;
    private readonly IApplicationLauncher applicationLauncher;
    private readonly IOptions<FocusViewOptions> focusViewOptions;

    private CancellationTokenSource? cancellationTokenSource;

    public LaunchViewModel(
        IViewManager viewManager,
        INotificationService notificationService,
        IDaybreakApiService daybreakApiService,
        ILaunchConfigurationService launchConfigurationService,
        IOnboardingService onboardingService,
        IApplicationLauncher applicationLauncher,
        IOptions<FocusViewOptions> focusViewOptions)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.notificationService = notificationService.ThrowIfNull();
        this.daybreakApiService = daybreakApiService.ThrowIfNull();
        this.launchConfigurationService = launchConfigurationService.ThrowIfNull();
        this.onboardingService = onboardingService.ThrowIfNull();
        this.applicationLauncher = applicationLauncher.ThrowIfNull();
        this.focusViewOptions = focusViewOptions.ThrowIfNull();

        this.viewManager.ShowViewRequested += (_, viewRequest) =>
        {
            if (viewRequest.ViewType != typeof(LaunchView))
            {
                this.cancellationTokenSource?.Cancel();
                this.cancellationTokenSource?.Dispose();
                this.cancellationTokenSource = null;
            }
        };
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<LauncherViewContext> LaunchConfigurations { get; } = [];
    
    public LauncherViewContext? SelectedConfiguration 
    { 
        get;
        private set 
        { 
            if (field != value)
            {
                field = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.LaunchButtonText));
            }
        }
    }
    
    public bool CanLaunch 
    { 
        get;
        private set 
        { 
            if (field != value)
            {
                field = value;
                this.OnPropertyChanged();
            }
        }
    }

    public bool IsLaunching { get; private set; }

    public bool IsDropdownOpen 
    { 
        get; 
        set 
        { 
            if (field != value)
            {
                field = value;
                this.OnPropertyChanged();
            }
        } 
    }
    
    public string LaunchButtonText => this.GetLaunchButtonText();

    public override ValueTask ParametersSet(LaunchView view, CancellationToken cancellationToken)
    {
        if (!this.IsOnboarded())
        {
            this.viewManager.ShowView<LauncherOnboardingView>((nameof(LauncherOnboardingView.Status), this.onboardingService.CheckOnboardingStage().ToString()));
            return base.ParametersSet(view, cancellationToken);
        }

        this.Initialize();
        return base.ParametersSet(view, cancellationToken);
    }

    public async Task LaunchSelectedConfiguration()
    {
        if (this.SelectedConfiguration?.Configuration is null || this.IsLaunching)
        {
            return;
        }

        if (!this.CanLaunch)
        {
            return;
        }

        this.IsLaunching = true;
        this.OnPropertyChanged(nameof(this.IsLaunching));
        this.OnPropertyChanged(nameof(this.LaunchButtonText));
        this.UpdateCanLaunch();

        try
        {
            if (this.SelectedConfiguration.CanKill)
            {
                this.KillGuildWars();
            }
            else
            {
                await this.LaunchGuildWars(this.cancellationTokenSource?.Token ?? CancellationToken.None);
            }
        }
        catch (Exception ex)
        {
            this.notificationService.NotifyError(
                title: "Launch Failed", 
                description: $"Failed to launch: {ex.Message}");
        }
        finally
        {
            this.IsLaunching = false;
            this.OnPropertyChanged(nameof(this.IsLaunching));
            this.OnPropertyChanged(nameof(this.LaunchButtonText));
            this.UpdateCanLaunch();
        }
    }

    public void SelectConfiguration(LauncherViewContext configuration)
    {
        if (this.SelectedConfiguration != configuration)
        {
            this.SelectedConfiguration = configuration;
            this.UpdateCanLaunch();
        }
        
        this.IsDropdownOpen = false;
        this.RefreshView();
    }

    public void ToggleDropdown()
    {
        this.IsDropdownOpen = !this.IsDropdownOpen;
        this.RefreshView();
    }

    public void CloseDropdown()
    {
        this.IsDropdownOpen = false;
        this.RefreshView();
    }

    private void Initialize()
    {
        this.viewManager.ShowViewRequested += this.ViewManager_ShowViewRequested;
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = new CancellationTokenSource();

        this.RetrieveLaunchConfigurations();
        var ct = this.cancellationTokenSource.Token;
        Task.Factory.StartNew(() => this.PeriodicallyCheckSelectedConfigState(ct), ct, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    private void Cleanup()
    {
        this.viewManager.ShowViewRequested -= this.ViewManager_ShowViewRequested;
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = default;
    }

    private void ViewManager_ShowViewRequested(object? sender, TrailBlazr.Models.ViewRequest e)
    {
        if (e.ViewModelType == typeof(LaunchViewModel))
        {
            return;
        }

        this.Cleanup();
    }

    private bool IsOnboarded()
    {
        var onboardingStage = this.onboardingService.CheckOnboardingStage();
        if (onboardingStage is LauncherOnboardingStage.Default)
        {
            throw new InvalidOperationException($"Unexpected onboarding stage {onboardingStage}");
        }

        return onboardingStage is not (LauncherOnboardingStage.NeedsCredentials or LauncherOnboardingStage.NeedsExecutable or LauncherOnboardingStage.NeedsConfiguration);
    }

    private void RetrieveLaunchConfigurations()
    {
        var latestLaunchConfiguration = this.launchConfigurationService.GetLastLaunchConfigurationWithCredentials();
        var configurations = this.launchConfigurationService.GetLaunchConfigurations()
            .Select(c => new LauncherViewContext { Configuration = c, CanLaunch = false })
            .ToList();
        
        this.LaunchConfigurations.Clear();
        foreach (var config in configurations)
        {
            this.LaunchConfigurations.Add(config);
        }
        
        this.SelectedConfiguration = this.LaunchConfigurations.FirstOrDefault(c => c.Configuration?.Equals(latestLaunchConfiguration) is true) 
                              ?? this.LaunchConfigurations.FirstOrDefault();
    }

    private async Task PeriodicallyCheckSelectedConfigState(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (!this.IsLaunching)
                {
                    await this.SetLaunchButtonState(cancellationToken);
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception)
            {
            }
        }
    }

    private async Task SetLaunchButtonState(CancellationToken cancellationToken)
    {
        if (this.SelectedConfiguration is null)
        {
            this.CanLaunch = false;
            return;
        }

        if (this.IsLaunching)
        {
            this.CanLaunch = false;
            return;
        }

        foreach (var config in this.LaunchConfigurations)
        {
            var isSelected = config == this.SelectedConfiguration;
            await this.CheckGameState(config, isSelected, cancellationToken);
        }

        this.UpdateCanLaunch();
        this.OnPropertyChanged(nameof(this.LaunchButtonText));
        this.RefreshView();
    }

    private void UpdateCanLaunch()
    {
        if (this.SelectedConfiguration is null)
        {
            this.CanLaunch = false;
        }
        else
        {
            this.CanLaunch = (this.SelectedConfiguration.CanKill || this.SelectedConfiguration.CanLaunch || this.SelectedConfiguration.CanAttach) && !this.IsLaunching;
        }
    }

    private string GetLaunchButtonText()
    {
        var configName = this.SelectedConfiguration?.Configuration?.Credentials?.Username ?? "Guild Wars";
        if (this.IsLaunching)
        {
            return "Launching...";
        }

        if (this.SelectedConfiguration?.CanKill == true)
        {
            return $"Kill {configName}";
        }

        if (this.SelectedConfiguration?.CanAttach == true)
        {
            return $"Attach to {configName}";
        }

        return $"Launch {configName}";
    }

    private void KillGuildWars()
    {
        if (this.SelectedConfiguration?.AppContext is null)
        {
            return;
        }

        this.applicationLauncher.KillGuildWarsProcess(this.SelectedConfiguration.AppContext);
    }

    private async Task LaunchGuildWars(CancellationToken cancellationToken)
    {
        if (this.SelectedConfiguration?.Configuration is null)
        {
            return;
        }

        this.launchConfigurationService.SetLastLaunchConfigurationWithCredentials(this.SelectedConfiguration.Configuration);
        
        try
        {
            if (this.SelectedConfiguration.AppContext is not null)
            {
                await this.AttachContext(this.SelectedConfiguration.AppContext, cancellationToken);
            }
            else if (this.SelectedConfiguration.ApiContext is not null)
            {
                await this.AttachContext(this.SelectedConfiguration, this.SelectedConfiguration.ApiContext, cancellationToken);
            }
            else
            {
                await this.LaunchContext(this.SelectedConfiguration, cancellationToken);
            }
        }
        catch (Exception)
        {
            this.UpdateCanLaunch();
            throw;
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

        // Return early if the app and api context are already initialized
        if (launcherViewContext.AppContext is not null &&
            !launcherViewContext.AppContext.GuildWarsProcess.HasExited &&
            launcherViewContext.ApiContext is not null &&
            await launcherViewContext.ApiContext.IsAvailable(cancellationToken))
        {
            return (launcherViewContext.AppContext, launcherViewContext.ApiContext);
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
        }
        else
        {
            this.viewManager.ShowView<FocusView>(
                (nameof(FocusView.ProcessId), context.ProcessId.ToString()),
                (nameof(FocusView.ConfigurationId), context.LaunchConfiguration.Identifier ?? throw new InvalidOperationException("LaunchConfig identifier cannot be null")));
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
        this.viewManager.ShowView<FocusView>(
                (nameof(FocusView.ProcessId), launchContext.ProcessId.ToString()),
                (nameof(FocusView.ConfigurationId), launchContext.LaunchConfiguration.Identifier ?? throw new InvalidOperationException("LaunchConfig identifier cannot be null")));
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

        launcherViewContext.AppContext = launchedContext;
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
        var sw = Stopwatch.StartNew();
        var apiContext = await this.daybreakApiService.AttachDaybreakApiContext(launchedContext, cancellationToken);
        while (sw.Elapsed < LaunchTimeout && !cancellationToken.IsCancellationRequested && apiContext is null)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            apiContext = await this.daybreakApiService.AttachDaybreakApiContext(launchedContext, cancellationToken);
        }
        
        attachNotificationToken.Cancel();

        if (apiContext is null)
        {
            this.notificationService.NotifyError(
                title: "Could not attach to Guild Wars",
                description: "Could not find the Api context to attach to Guild Wars. Check the logs for more details");
        }
        else
        {
            launcherViewContext.ApiContext = apiContext;
            this.viewManager.ShowView<FocusView>(
                (nameof(FocusView.ProcessId), launchedContext.ProcessId.ToString()),
                (nameof(FocusView.ConfigurationId), launchedContext.LaunchConfiguration.Identifier ?? throw new InvalidOperationException("LaunchConfig identifier cannot be null")));
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Implement IDisposable to properly clean up resources
    private void DisposeInternal(bool disposing)
    {
        if (disposing)
        {
            this.cancellationTokenSource?.Cancel();
            this.cancellationTokenSource?.Dispose();
        }
    }

    public void Dispose()
    {
        this.DisposeInternal(true);
        GC.SuppressFinalize(this);
    }
}
