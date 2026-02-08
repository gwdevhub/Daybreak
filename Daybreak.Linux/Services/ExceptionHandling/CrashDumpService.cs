using Daybreak.Shared.Services.ExceptionHandling;
using Microsoft.Extensions.Logging;

namespace Daybreak.Linux.Services.ExceptionHandling;

//TODO: Implement crash dump writing for Linux. This is currently a stub that just logs that the feature is not supported.
internal sealed class CrashDumpService : ICrashDumpService
{
    private readonly ILogger<CrashDumpService> logger;

    public CrashDumpService(ILogger<CrashDumpService> logger)
    {
        this.logger = logger;
    }

    public void WriteCrashDump(string dumpFilePath)
    {
        this.logger.LogDebug("Crash dump writing is not supported on Linux");
    }
}
