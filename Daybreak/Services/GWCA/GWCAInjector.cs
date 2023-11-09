using Daybreak.Models;
using Daybreak.Models.GWCA;
using Daybreak.Services.Injection;
using Daybreak.Services.Notifications;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.GWCA;

internal sealed class GWCAInjector : IGWCAInjector
{
    private const int MaxRetries = 10;
    private const string ModulePath = "GWCA/Daybreak.GWCA.dll";

    private readonly INotificationService notificationService;
    private readonly IGWCAClient gwcaClient;
    private readonly IProcessInjector injector;

    public bool IsEnabled { get; set; } = true;
    public bool IsInstalled { get; } = true;
    public string Name { get; } = "GWCA";

    public GWCAInjector(
        INotificationService notificationService,
        IGWCAClient client,
        IProcessInjector processInjector)
    {
        this.notificationService = notificationService.ThrowIfNull();
        this.gwcaClient = client.ThrowIfNull();
        this.injector = processInjector.ThrowIfNull();
    }

    public IEnumerable<string> GetCustomArguments() => Enumerable.Empty<string>();

    public Task OnGuildWarsStarting(ApplicationLauncherContext applicationLauncherContext, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task OnGuildWarsStartingDisabled(ApplicationLauncherContext applicationLauncherContext, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task OnGuildWarsCreated(ApplicationLauncherContext applicationLauncherContext, CancellationToken cancellationToken)
    {
        if (!await this.injector.Inject(applicationLauncherContext.Process, ModulePath, cancellationToken))
        {
            this.notificationService.NotifyError(
                    title: "Unable to inject GWCA into Guild Wars process",
                    description: "Daybreak integration with the Guild Wars process will be affected. Some Daybreak functionality might not work");
        }
    }

    public async Task OnGuildWarsStarted(ApplicationLauncherContext applicationLauncherContext, CancellationToken cancellationToken)
    {
        ConnectionContext? connectionContext = default;
        for(var i = 0; i < MaxRetries; i++)
        {
            if (await this.gwcaClient.Connect(applicationLauncherContext.Process, cancellationToken) is ConnectionContext newContext)
            {
                connectionContext = newContext;
                break;
            }

            await Task.Delay(1000, cancellationToken);
        }

        if (connectionContext is null)
        {
            this.notificationService.NotifyError(
                        title: "Unable to inject GWCA into Guild Wars process",
                        description: "Daybreak integration with the Guild Wars process will be affected. Some Daybreak functionality might not work");
            return;
        }

        if (await this.gwcaClient.CheckAlive(connectionContext.Value, cancellationToken) is false)
        {
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
