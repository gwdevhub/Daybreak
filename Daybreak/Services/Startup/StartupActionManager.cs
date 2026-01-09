using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Startup;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Slim;
using System.Core.Extensions;

namespace Daybreak.Services.Startup;

internal sealed class StartupActionManager(
    IServiceManager serviceManager,
    ILogger<StartupActionManager> logger) : IStartupActionProducer, IHostedService
{
    private readonly IServiceManager serviceManager = serviceManager.ThrowIfNull();
    private readonly ILogger<StartupActionManager> logger = logger.ThrowIfNull();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() =>
        {
            var asyncTasks = new List<Task>();
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            foreach (var action in this.serviceManager.GetServicesOfType<StartupActionBase>())
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

    public void RegisterAction<T>() where T : StartupActionBase
    {
        this.serviceManager.RegisterScoped<T>();
    }
}
