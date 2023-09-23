using Microsoft.Extensions.Logging;
using Reloaded.Injector;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;

namespace Daybreak.Services.Injection;
internal sealed class ProcessInjector : IProcessInjector
{
    private readonly ILogger<ProcessInjector> logger;
    
    public ProcessInjector(
        ILogger<ProcessInjector> logger)
    {
        this.logger = logger.ThrowIfNull();
    }

    public bool Inject(Process process, string pathToDll)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.Inject), pathToDll);
        scopedLogger.LogInformation("Injecting");
        var injector = new Injector(process);
        var result = injector.Inject(pathToDll);
        scopedLogger.LogInformation($"Injection finished with result {result}");
        return result != 0;
    }
}
