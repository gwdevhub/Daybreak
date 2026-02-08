using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Extensions.Core;

namespace Daybreak.Services.Initialization;

internal class StartupActionsProducer(IServiceCollection services, ILogger<StartupActionsProducer> logger)
    : IStartupActionProducer
{
    private readonly ILogger<StartupActionsProducer> logger = logger;
    private readonly IServiceCollection services = services;

    public void RegisterAction<T>() where T : StartupActionBase
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        this.services.AddSingleton<StartupActionBase, T>();
        scopedLogger.LogDebug("Registered {StartupAction.Name}", typeof(T).Name);
    }
}
