using Daybreak.Shared.Services.ApplicationArguments;
using Daybreak.Shared.Services.ApplicationArguments.ArgumentHandling;
using Microsoft.Extensions.Logging;
using Slim;
using System.Core.Extensions;
using System.Extensions;

namespace Daybreak.Services.ApplicationArguments;

internal sealed class ApplicationArgumentService(IServiceManager serviceManager, ILogger<ApplicationArgumentService> logger) : IApplicationArgumentService
{
    private readonly IServiceManager serviceManager = serviceManager.ThrowIfNull();
    private readonly ILogger<ApplicationArgumentService> logger = logger.ThrowIfNull();

    public void HandleArguments(string[] args)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.HandleArguments), string.Empty);
        var handlers = this.serviceManager.GetServicesOfType<IArgumentHandler>().ToList();
        var argQueue = new Queue<string>(args);
        while (argQueue.TryDequeue(out var arg))
        {
            var handler = handlers.FirstOrDefault(h => h.Identifier == arg);
            if (handler is null)
            {
                continue;
            }

            var argsToDequeue = handler.ExpectedArgumentCount;
            var handlerArgs = new List<string>(argsToDequeue);
            for(var i = 0; i < argsToDequeue; i++)
            {
                if (!argQueue.TryDequeue(out var handlerArg))
                {
                    break;
                }

                handlerArgs.Add(handlerArg);
            }

            if (handlerArgs.Count != handler.ExpectedArgumentCount)
            {
                scopedLogger.LogError($"[{handler.Identifier}] Could not get all required arguments for handler");
                continue;
            }

            handler.HandleArguments([.. handlerArgs]);
        }
    }

    public void RegisterArgumentHandler<T>()
        where T : class, IArgumentHandler
    {
        this.serviceManager.RegisterScoped<T>();
    }
}
