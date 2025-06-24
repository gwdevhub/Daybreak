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
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Api;
public sealed class DaybreakApiService(
    IAttachedApiAccessor attachedApiAccessor,
    IMDomainRegistrar mDomainRegistrar,
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
    private const string ServiceSubType = "daybreak-api";

    private readonly IAttachedApiAccessor attachedApiAccessor = attachedApiAccessor.ThrowIfNull();
    private readonly IMDomainRegistrar mDomainRegistrar = mDomainRegistrar.ThrowIfNull();
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

    public Task<ScopedApiContext?> AttachDaybreakApiContext(GuildWarsApplicationLaunchContext launchContext, ScopedApiContext apiContext, CancellationToken _)
    {
        if (this.attachedApiAccessor is AttachedApiAccessor accessor)
        {
            accessor.LaunchContext = launchContext;
            accessor.ApiContext = apiContext;
        }

        return Task.FromResult<ScopedApiContext?>(apiContext);
    }

    public async Task<ScopedApiContext?> AttachDaybreakApiContext(GuildWarsApplicationLaunchContext launchContext, CancellationToken cancellationToken)
    {
        var apiContext = await this.GetDaybreakApiContext(launchContext.GuildWarsProcess, cancellationToken);
        if (apiContext is not null)
        {
            return await this.AttachDaybreakApiContext(launchContext, apiContext, cancellationToken);
        }
        else
        {
            return apiContext;
        }
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
        var serviceUris = this.mDomainRegistrar.Resolve(serviceName);
        var serviceUri = serviceUris?.Count > 0 ? serviceUris[0] : default;
        if (serviceUri is null)
        {
            scopedLogger.LogWarning("Failed to find Daybreak API service by name {serviceName}", serviceName);
            return default;
        }

        var apiContext = new DaybreakAPIContext(serviceUri);
        var scopedApiContext = new ScopedApiContext(this.scopedApiLogger, this.scopedApiClient, apiContext);
        if (await scopedApiContext.IsAvailable(cancellationToken))
        {
            return scopedApiContext;
        }

        return default;
    }

    public async Task<ScopedApiContext?> FindDaybreakApiContextByCredentials(LoginCredentials loginCredentials, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var serviceUris = this.mDomainRegistrar.QueryByServiceName(n => n.StartsWith(ServiceSubType));
        foreach(var uri in serviceUris ?? [])
        {
            var apiContext = new DaybreakAPIContext(uri);
            var scopedApiContext = new ScopedApiContext(this.scopedApiLogger, this.scopedApiClient, apiContext);

            var response = await scopedApiContext.GetLoginInfo(cancellationToken);
            if (loginCredentials.Username == response?.Email)
            {
                return scopedApiContext;
            }
        }

        scopedLogger.LogWarning("Failed to find Daybreak API service by credentials for user {username}", loginCredentials?.Username ?? string.Empty);
        return default;
    }

    public void RequestInstancesAnnouncement()
    {
        this.mDomainRegistrar.QueryAllServices();
    }

    public Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken) =>
        Task.Factory.StartNew(() => this.InjectWithStub(guildWarsCreatedContext.ApplicationLauncherContext), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);

    public Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task<bool> ShouldRunAgain(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken)
    {
        if (!guildWarsRunningContext.LoadedModules.Contains(DaybreakApiName) &&
            this.IsEnabled)
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    public Task OnGuildWarsRunning(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken)
        => Task.Factory.StartNew(() => this.InjectWithStub(guildWarsRunningContext.ApplicationLauncherContext), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);

    private void InjectWithStub(ApplicationLauncherContext context)
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
                "Failed to inject with stub. Daybreak API dll not found",
                expirationTime: DateTime.UtcNow + TimeSpan.FromSeconds(10));
            scopedLogger.LogError("Failed to inject with stub. Daybreak API DLL not found at {dllPath}", dllName);
            return;
        }

        scopedLogger.LogInformation("Injecting {dllName} into {processId}", dllName, context.Process.Id);
        if (!this.stubInjector.Inject(context.Process, dllName, out var port))
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
