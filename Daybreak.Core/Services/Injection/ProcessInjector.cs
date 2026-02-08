using Daybreak.Shared.Services.Injection;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Extensions.Core;

namespace Daybreak.Services.Injection;
internal sealed class ProcessInjector(
    IDaybreakInjector daybreakInjector,
    ILogger<ProcessInjector> logger) : IProcessInjector
{
    private readonly IDaybreakInjector daybreakInjector = daybreakInjector.ThrowIfNull();
    private readonly ILogger<ProcessInjector> logger = logger.ThrowIfNull();

    public async Task<bool> Inject(Process process, string pathToDll, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var result = await this.daybreakInjector.InjectWinApi(process.Id, pathToDll, cancellationToken);
        if (result < 0)
        {
            scopedLogger.LogError("Failed to inject DLL into process {ProcessId} with error code {ErrorCode}", process.Id, result);
            return false;
        }

        return true;
    }
}
