using Daybreak.Shared.Services.Injection;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions.Core;

namespace Daybreak.Services.Injection;
internal sealed class StubInjector(
    IDaybreakInjector daybreakInjector,
    ILogger<StubInjector> logger) : IStubInjector
{
    private readonly IDaybreakInjector daybreakInjector = daybreakInjector.ThrowIfNull();
    private readonly ILogger<StubInjector> logger = logger;

    public async Task<int> Inject(Process target, string dllPath, string entryPoint, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var result = await this.daybreakInjector.InjectStub(target.Id, dllPath, entryPoint, cancellationToken);
        if (result < 0)
        {
            scopedLogger.LogError("Failed to inject DLL into process {ProcessId} with error code {ErrorCode}", target.Id, result);
            return (int)result;
        }

        scopedLogger.LogInformation("Successfully injected DLL into process {ProcessId}. Stub response: {ExitCode}", target.Id, result);
        return (int)result;
    }
}
