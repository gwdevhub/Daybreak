﻿using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Startup;
using Microsoft.Extensions.Logging;
using Slim;
using System.Core.Extensions;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.Startup;

internal sealed class StartupActionManager(
    IServiceManager serviceManager,
    ILogger<StartupActionManager> logger) : IStartupActionProducer, IApplicationLifetimeService
{
    private readonly IServiceManager serviceManager = serviceManager.ThrowIfNull();
    private readonly ILogger<StartupActionManager> logger = logger.ThrowIfNull();

    public void OnClosing()
    {
    }

    public void OnStartup()
    {
        Task.Factory.StartNew(() =>
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
        }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    public void RegisterAction<T>() where T : StartupActionBase
    {
        this.serviceManager.RegisterScoped<T>();
    }
}
