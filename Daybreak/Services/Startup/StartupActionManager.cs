using Daybreak.Services.Startup.Actions;
using Slim;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.Startup;

public sealed class StartupActionManager : IStartupActionProducer, IApplicationLifetimeService
{
    private readonly IServiceManager serviceManager;

    public StartupActionManager(
        IServiceManager serviceManager)
    {
        this.serviceManager = serviceManager.ThrowIfNull();
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

        Task.WaitAll(asyncTasks.ToArray());
    }

    public void RegisterAction<T>() where T : StartupActionBase
    {
        this.serviceManager.RegisterScoped<T>();
    }
}
