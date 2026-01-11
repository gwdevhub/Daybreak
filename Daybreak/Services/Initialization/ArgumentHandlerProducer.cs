using Daybreak.Shared.Services.ApplicationArguments.ArgumentHandling;
using Daybreak.Shared.Services.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Extensions.Core;

namespace Daybreak.Services.Initialization;

public sealed class ArgumentHandlerProducer(IServiceCollection services, ILogger<ArgumentHandlerProducer> logger)
    : IArgumentHandlerProducer
{
    private readonly IServiceCollection services = services;
    private readonly ILogger<ArgumentHandlerProducer> logger = logger;

    public void RegisterArgumentHandler<T>() where
        T : class, IArgumentHandler
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        this.services.AddSingleton<IArgumentHandler, T>();
        scopedLogger.LogDebug("Registered {ArgumentHandler.Name}", typeof(T).Name);
    }
}
