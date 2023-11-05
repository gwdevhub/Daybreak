using Daybreak.Models.GWCA;
using Daybreak.Services.Injection;
using Daybreak.Services.Notifications;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.GWCA;

internal sealed class GWCAInjector : IGWCAInjector
{
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

    public Task OnGuildWarsStarting(Process process, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task OnGuildWarsStartingDisabled(Process process, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task OnGuildWarsCreated(Process process, CancellationToken cancellationToken)
    {
        if (!await this.injector.Inject(process, ModulePath, cancellationToken))
        {
            this.notificationService.NotifyError(
                    title: "Unable to inject GWCA into Guild Wars process",
                    description: "Daybreak integration with the Guild Wars process will be affected. Some Daybreak functionality might not work");
        }
    }

    public async Task OnGuildWarsStarted(Process process, CancellationToken cancellationToken)
    {
        if (await this.gwcaClient.Connect(process, cancellationToken) is not ConnectionContext connectionContext)
        {
            var maybeConnectionContext = await this.gwcaClient.Connect(process, cancellationToken);
            if (maybeConnectionContext is not ConnectionContext newContext)
            {
                this.notificationService.NotifyError(
                    title: "Unable to inject GWCA into Guild Wars process",
                    description: "Daybreak integration with the Guild Wars process will be affected. Some Daybreak functionality might not work");
                return;
            }

            connectionContext = newContext;
        }

        if (await this.gwcaClient.CheckAlive(connectionContext, cancellationToken) is false)
        {
            this.notificationService.NotifyError(
                    title: "Unable to inject GWCA into Guild Wars process",
                    description: "Daybreak integration with the Guild Wars process will be affected. Some Daybreak functionality might not work");
            return;
        }

        this.notificationService.NotifyInformation(
            title: "GWCA integration injected",
            description: $"Daybreak GWCA integration has started successfully on http://localhost:{connectionContext.Port}");
    }
}
