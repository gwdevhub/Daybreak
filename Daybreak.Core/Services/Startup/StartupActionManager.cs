using Daybreak.Shared.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;

namespace Daybreak.Services.Startup;

internal sealed class StartupActionManager(
    IEnumerable<StartupActionBase> startupActions,
    ILogger<StartupActionManager> logger) : IHostedService
{
    private readonly IEnumerable<StartupActionBase> startupActions = startupActions.ThrowIfNull();
    private readonly ILogger<StartupActionManager> logger = logger.ThrowIfNull();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() =>
        {
            var asyncTasks = new List<Task>();
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            foreach (var action in this.startupActions)
            {
                action.ExecuteOnStartup();
                asyncTasks.Add(Task.Run(() => action.ExecuteOnStartupAsync(cts.Token)));
            }

            try
            {
                Task.WaitAll([.. asyncTasks]);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Encountered an exception while processing startup actions");
            }
        }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
