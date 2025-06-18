using Daybreak.Shared.Models;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.Api;
using Daybreak.Shared.Services.Injection;
using Daybreak.Shared.Services.MDns;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions.Core;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Api;
public sealed class DaybreakApiService(
    IMDnsService mdnsService,
    IStubInjector stubInjector,
    INotificationService notificationService,
    ILogger<DaybreakApiService> logger)
    : IDaybreakApiService
{
    private const string DaybreakApiName = "Daybreak.API.dll";
    private const string ProcessIdPlaceholder = "{PID}";
    private const string DaybreakApiServiceName = $"daybreak-api-{ProcessIdPlaceholder}";
    private readonly IMDnsService mdnsService = mdnsService.ThrowIfNull();
    private readonly IStubInjector stubInjector = stubInjector.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly ILogger<DaybreakApiService> logger = logger.ThrowIfNull();

    public string Name { get; } = "Daybreak API";
    public bool IsEnabled { get; set; } = true;
    public bool IsInstalled { get; } = true;

    public IEnumerable<string> GetCustomArguments() => [];

    public Task<DaybreakAPIContext?> GetDaybreakApiContext(GuildWarsApplicationLaunchContext launchContext, CancellationToken cancellationToken)
    {
        return this.GetDaybreakApiContext(launchContext.GuildWarsProcess, cancellationToken);
    }

    public async Task<DaybreakAPIContext?> GetDaybreakApiContext(Process guildWarsProcess, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (guildWarsProcess.HasExited)
        {
            scopedLogger.LogWarning("Guild Wars process has exited");
            return default;
        }

        var serviceName = DaybreakApiServiceName.Replace(ProcessIdPlaceholder, guildWarsProcess.Id.ToString());
        var serviceUris = await this.mdnsService.FindLocalService(serviceName, cancellationToken: cancellationToken);
        var serviceUri = serviceUris?.Count > 0 ? serviceUris[0] : default;
        if (serviceUri is null)
        {
            scopedLogger.LogWarning("Failed to find Daybreak API service by name {serviceName}", serviceName);
            return default;
        }

        return new DaybreakAPIContext(serviceUri, guildWarsProcess);
    }

    public Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken) =>
        Task.Factory.StartNew(() => this.InjectWithStub(guildWarsCreatedContext), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);

    public Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken) => Task.CompletedTask;

    private void InjectWithStub(GuildWarsCreatedContext context)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var dllName = 
            Path.GetFullPath(
                Path.Combine(
                    PathUtils.GetRootFolder(),
                    DaybreakApiName));

        if (!File.Exists(dllName))
        {
            this.notificationService.NotifyError(
                "Daybreak API Failure",
                "Failed to inject with stub. Daybreak API dll not found");
            scopedLogger.LogError("Failed to inject with stub. Daybreak API DLL not found at {dllPath}", dllName);
            return;
        }

        scopedLogger.LogInformation("Injecting {dllName} into {processId}", dllName, context.ApplicationLauncherContext.Process.Id);
        if (!this.stubInjector.Inject(context.ApplicationLauncherContext.Process, dllName, out var port))
        {
            this.notificationService.NotifyError(
                "Daybreak API Failure",
                "Failed to inject with stub. Injection failed");
            scopedLogger.LogError("Failed to inject with stub. Injection failed");
            return;
        }

        if (port <= 0)
        {
            this.notificationService.NotifyError(
                "Daybreak API Failure",
                "Failed to start API. Non-success exit code");
            scopedLogger.LogError($"Failed to start API. Exit code {port}");
            return;
        }

        this.notificationService.NotifyInformation(
            "Injected Daybreak API",
            $"Daybreak API started on port {port}");
    }
}
