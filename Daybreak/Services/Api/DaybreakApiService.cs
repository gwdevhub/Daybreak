using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.Api;
using Daybreak.Shared.Services.Injection;
using Daybreak.Shared.Services.MDns;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions.Core;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Api;
public sealed class DaybreakApiService(
    IAttachedApiAccessor attachedApiAccessor,
    IMDnsService mdnsService,
    IStubInjector stubInjector,
    INotificationService notificationService,
    IHttpClient<ScopedApiContext> scopedApiClient,
    ILiveUpdateableOptions<FocusViewOptions> liveUpdateableOptions,
    ILogger<DaybreakApiService> logger,
    ILogger<ScopedApiContext> scopedApiLogger)
    : IDaybreakApiService
{
    private const string DaybreakApiName = "Daybreak.API.dll";
    private const string ProcessIdPlaceholder = "{PID}";
    private const string DaybreakApiServiceName = $"daybreak-api-{ProcessIdPlaceholder}";
    private const int MDNSRetryCount = 3;

    private static readonly TimeSpan MDNSTimeout = TimeSpan.FromSeconds(5);

    private readonly IAttachedApiAccessor attachedApiAccessor = attachedApiAccessor.ThrowIfNull();
    private readonly IMDnsService mdnsService = mdnsService.ThrowIfNull();
    private readonly IStubInjector stubInjector = stubInjector.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IHttpClient<ScopedApiContext> scopedApiClient = scopedApiClient.ThrowIfNull();
    private readonly ILiveUpdateableOptions<FocusViewOptions> liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
    private readonly ILogger<DaybreakApiService> logger = logger.ThrowIfNull();
    private readonly ILogger<ScopedApiContext> scopedApiLogger = scopedApiLogger.ThrowIfNull();

    public string Name { get; } = "Daybreak API";

    public bool IsEnabled
    {
        get => this.liveUpdateableOptions.Value.Enabled;
        set
        {
            this.liveUpdateableOptions.Value.Enabled = value;
            this.liveUpdateableOptions.UpdateOption();
        }
    }

    public bool IsInstalled { get; } = true;

    public IEnumerable<string> GetCustomArguments() => [];

    public async Task<ScopedApiContext?> AttachDaybreakApiContext(GuildWarsApplicationLaunchContext launchContext, CancellationToken cancellationToken)
    {
        var apiContext = await this.GetDaybreakApiContext(launchContext.GuildWarsProcess, cancellationToken);
        if (this.attachedApiAccessor is AttachedApiAccessor accessor)
        {
            accessor.LaunchContext = launchContext;
            accessor.ApiContext = apiContext;
        }

        return apiContext;
    }

    public async Task<ScopedApiContext?> GetDaybreakApiContext(Process guildWarsProcess, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (guildWarsProcess.HasExited)
        {
            scopedLogger.LogWarning("Guild Wars process has exited");
            return default;
        }

        var serviceName = DaybreakApiServiceName.Replace(ProcessIdPlaceholder, guildWarsProcess.Id.ToString());
        IReadOnlyList<Uri>? serviceUris = default;
        for (var i = 0; i < MDNSRetryCount; i++)
        {
            using var cts = new CancellationTokenSource(MDNSTimeout);
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
            scopedLogger.LogInformation("{attempt}/{maxAttempts} Searching for Daybreak API service with name {serviceName}", i, MDNSRetryCount, serviceName);
            try
            {
                serviceUris = await this.mdnsService.FindLocalService(serviceName, cancellationToken: linkedCts.Token);
                break;
            }
            catch(Exception e)
            {
                scopedLogger.LogError(e, "{attempt}/{maxAttempts} Failed to find Daybreak API service with name {serviceName}", i, MDNSRetryCount, serviceName);
            }
        }

        var serviceUri = serviceUris?.Count > 0 ? serviceUris[0] : default;
        if (serviceUri is null)
        {
            scopedLogger.LogWarning("Failed to find Daybreak API service by name {serviceName}", serviceName);
            return default;
        }

        var apiContext = new DaybreakAPIContext(serviceUri, guildWarsProcess);
        return new ScopedApiContext(this.scopedApiLogger, this.scopedApiClient, apiContext);
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
