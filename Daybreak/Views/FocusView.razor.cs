using Daybreak.Shared.Services.Api;
using Daybreak.Shared.Services.LaunchConfigurations;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions.Core;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;

//TODO: Setup FocusView
public sealed class FocusViewModel(
    IViewManager viewManager,
    INotificationService notificationService,
    ILaunchConfigurationService launchConfigurationService,
    IDaybreakApiService daybreakApiService,
    ILogger<FocusView> logger)
    : ViewModelBase<FocusViewModel, FocusView>
{
    private readonly IViewManager viewManager = viewManager.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly ILaunchConfigurationService launchConfigurationService = launchConfigurationService.ThrowIfNull();
    private readonly IDaybreakApiService daybreakApiService = daybreakApiService.ThrowIfNull();
    private readonly ILogger<FocusView> logger = logger.ThrowIfNull();

    private ScopedApiContext? apiContext;
    private Process? process;

    public override async ValueTask ParametersSet(FocusView view, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var launchConfigId = view.ConfigurationId;
        var daybreakLaunchConfig = this.launchConfigurationService.GetLaunchConfigurations()
            .FirstOrDefault(x => x.Identifier == launchConfigId);
        if (daybreakLaunchConfig is null)
        {
            scopedLogger.LogError("Launch configuration with ID {configId} not found", launchConfigId);
            this.notificationService.NotifyError(
                title: "Focus View Error",
                description: $"Could not find launch configuration by id {launchConfigId}");

            this.viewManager.ShowView<LaunchView>();
            return;
        }

        if (!int.TryParse(view.ProcessId, out var processId))
        {
            scopedLogger.LogError("Process id is invalid {processId}", view.ProcessId);
            this.notificationService.NotifyError(
                title: "Focus View Error",
                description: $"Could not find GuildWars process by id {processId}");

            this.viewManager.ShowView<LaunchView>();
            return;
        }

        this.process = Process.GetProcessById(processId);
        var apiContext = await this.daybreakApiService.GetDaybreakApiContext(this.process, cancellationToken);
        if (apiContext is null)
        {
            scopedLogger.LogError("Could not attach to GuildWars process");
            this.notificationService.NotifyError(
                title: "Focus View Error",
                description: $"Could not attach to GuildWars process");

            this.viewManager.ShowView<LaunchView>();
            return;
        }

        this.apiContext = apiContext;
    }
}
