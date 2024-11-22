using Daybreak.Configuration.Options;
using Daybreak.Models.GWCA;
using Daybreak.Models.Mods;
using Daybreak.Services.Injection;
using Daybreak.Services.Notifications;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.GWCA;

internal sealed class GWCAInjector : IGWCAInjector
{
    private const int MaxRetries = 10;
    private const string ModuleSubPath = "GWCA/Daybreak.GWCA.dll";

    private readonly INotificationService notificationService;
    private readonly IGWCAClient gwcaClient;
    private readonly IProcessInjector injector;
    private readonly ILiveUpdateableOptions<FocusViewOptions> liveOptions;
    private readonly ILogger<GWCAInjector> logger;

    public bool IsEnabled
    {
        get => this.liveOptions.Value.Enabled;
        set
        {
            this.liveOptions.Value.Enabled = value;
            this.liveOptions.UpdateOption();
        }
    }
    public bool IsInstalled { get; } = true;
    public string Name { get; } = "GWCA";

    public GWCAInjector(
        INotificationService notificationService,
        IGWCAClient client,
        IProcessInjector processInjector,
        ILiveUpdateableOptions<FocusViewOptions> liveOptions,
        ILogger<GWCAInjector> logger)
    {
        this.notificationService = notificationService.ThrowIfNull();
        this.gwcaClient = client.ThrowIfNull();
        this.injector = processInjector.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public IEnumerable<string> GetCustomArguments() => [];

    public Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken)
    {
        var modulePath = PathUtils.GetAbsolutePathFromRoot(ModuleSubPath);
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.OnGuildWarsCreated), guildWarsCreatedContext.ApplicationLauncherContext.ExecutablePath);
        if (!await this.injector.Inject(guildWarsCreatedContext.ApplicationLauncherContext.Process, modulePath, cancellationToken))
        {
            scopedLogger.LogError("Unable to inject GWCA plugin into Guild Wars. Check above error messages for details");
            this.notificationService.NotifyError(
                    title: "Unable to inject GWCA into Guild Wars process",
                    description: "Daybreak integration with the Guild Wars process will be affected. Some Daybreak functionality might not work");
        }
    }

    public async Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.OnGuildWarsStarted), guildWarsStartedContext.ApplicationLauncherContext.ExecutablePath);
        ConnectionContext? connectionContext = default;
        for(var i = 0; i < MaxRetries; i++)
        {
            if (await this.gwcaClient.Connect(guildWarsStartedContext.ApplicationLauncherContext.ProcessId, cancellationToken) is ConnectionContext newContext)
            {
                connectionContext = newContext;
                break;
            }

            await Task.Delay(1000, cancellationToken);
        }

        if (connectionContext is null)
        {
            scopedLogger.LogError("Unable to create connection context. Unable to connect to created process");
            this.notificationService.NotifyError(
                        title: "Unable to inject GWCA into Guild Wars process",
                        description: "Daybreak integration with the Guild Wars process will be affected. Some Daybreak functionality might not work");
            return;
        }

        if (await this.gwcaClient.CheckAlive(connectionContext.Value, cancellationToken) is false)
        {
            scopedLogger.LogError("No response from GWCA plugin when calling CheckAlive");
            this.notificationService.NotifyError(
                    title: "Unable to inject GWCA into Guild Wars process",
                    description: "Daybreak integration with the Guild Wars process will be affected. Some Daybreak functionality might not work");
            return;
        }

        this.notificationService.NotifyInformation(
            title: "GWCA integration injected",
            description: $"Daybreak GWCA integration has started successfully on http://localhost:{connectionContext.Value.Port}");
    }
}
