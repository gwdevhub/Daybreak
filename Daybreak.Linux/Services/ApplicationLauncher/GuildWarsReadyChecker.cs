using Daybreak.Shared.Services.ApplicationLauncher;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Extensions.Core;

namespace Daybreak.Linux.Services.ApplicationLauncher;

/// <summary>
/// Linux-specific implementation that skips Win32 window enumeration.
/// Under Wine, we cannot enumerate windows from the Linux host process,
/// so we just verify the process is alive and return immediately.
/// </summary>
internal sealed class GuildWarsReadyChecker(
    ILogger<GuildWarsReadyChecker> logger
) : IGuildWarsReadyChecker
{
    private readonly ILogger<GuildWarsReadyChecker> logger = logger;

    public Task<bool> WaitForReady(Process process, TimeSpan timeout, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();

        if (process.HasExited)
        {
            scopedLogger.LogError("Guild Wars process has already exited. Process ID: {ProcessId}", process.Id);
            return Task.FromResult(false);
        }

        scopedLogger.LogInformation("Guild Wars process is running (PID {ProcessId}). Skipping window readiness check on Linux", process.Id);
        return Task.FromResult(true);
    }
}
