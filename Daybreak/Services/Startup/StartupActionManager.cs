using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Startup;
using Microsoft.Extensions.Logging;
using Slim;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.Startup;

internal sealed class StartupActionManager : IStartupActionProducer, IApplicationLifetimeService
{
    private readonly IServiceManager serviceManager;
    private readonly ILogger<StartupActionManager> logger;

    public StartupActionManager(
        IServiceManager serviceManager,
        ILogger<StartupActionManager> logger)
    {
        this.serviceManager = serviceManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public void OnClosing()
    {
    }

    public void OnStartup()
    {
        var asyncTasks = new List<Task>();
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        foreach(var action in this.serviceManager.GetServicesOfType<StartupActionBase>())
        {
            action.ExecuteOnStartup();
            asyncTasks.Add(Task.Run(() => action.ExecuteOnStartupAsync(cts.Token)));
        }

        try
        {
            Task.WaitAll(asyncTasks.ToArray());
        }
        catch(Exception e)
        {
            this.logger.LogError(e, "Encountered an exception while processing startup actions");
        }
    }

    public void RegisterAction<T>() where T : StartupActionBase
    {
        this.serviceManager.RegisterScoped<T>();
    }
}
