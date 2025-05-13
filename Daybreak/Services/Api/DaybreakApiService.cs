using Daybreak.Models.Mods;
using Daybreak.Services.Injection;
using Daybreak.Services.Notifications;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Extensions.Core;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Api;
public sealed class DaybreakApiService(
    IStubInjector stubInjector,
    INotificationService notificationService,
    ILogger<DaybreakApiService> logger)
    : IDaybreakApiService
{
    private const string DaybreakApiName = "Daybreak.API.dll";

    private readonly IStubInjector stubInjector = stubInjector.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly ILogger<DaybreakApiService> logger = logger.ThrowIfNull();

    public string Name { get; } = "Daybreak API";
    public bool IsEnabled { get; set; } = true;
    public bool IsInstalled { get; } = true;

    public IEnumerable<string> GetCustomArguments() => [];

    public Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken) =>
        Task.Factory.StartNew(() => this.InjectWithStub(guildWarsStartedContext), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);

    public Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken) => Task.CompletedTask;

    private void InjectWithStub(GuildWarsStartedContext context)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var dllName = 
            Path.GetFullPath(
                Path.Combine(
                    PathUtils.GetRootFolder(),
                    DaybreakApiName));

        if (!File.Exists(dllName))
        {
            scopedLogger.LogError("Failed to inject with stub. Daybreak API DLL not found at {dllPath}", dllName);
            return;
        }

        scopedLogger.LogInformation("Injecting {dllName} into {processId}", dllName, context.ApplicationLauncherContext.Process.Id);
        if (!this.stubInjector.Inject(context.ApplicationLauncherContext.Process, dllName))
        {
            scopedLogger.LogError("Failed to inject with stub. Injection failed");
            return;
        }
    }
}
