using Daybreak.Shared.Services.ApplicationArguments;
using Daybreak.Shared.Services.ApplicationArguments.ArgumentHandling;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;

namespace Daybreak.Services.ApplicationArguments;

internal sealed class ApplicationArgumentService(IEnumerable<IArgumentHandler> argumentHandlers, ILogger<ApplicationArgumentService> logger) : IApplicationArgumentService
{
    private readonly ILogger<ApplicationArgumentService> logger = logger.ThrowIfNull();
    private readonly IEnumerable<IArgumentHandler> argumentHandlers = argumentHandlers.ThrowIfNull();

    public void HandleArguments(string[] args)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var handlers = this.argumentHandlers.ToList();
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
}
