using Daybreak.Shared.Services.ApplicationLauncher;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions.Core;

namespace Daybreak.Windows.Services.ApplicationLauncher;

/// <summary>
/// Windows-specific implementation that waits for the Guild Wars window to be fully ready.
/// Uses Win32 window enumeration to detect when the game window is shown and responsive.
/// </summary>
internal sealed class GuildWarsReadyChecker(
    ILogger<GuildWarsReadyChecker> logger
) : IGuildWarsReadyChecker
{
    private readonly ILogger<GuildWarsReadyChecker> logger = logger.ThrowIfNull();

    public async Task<bool> WaitForReady(Process process, TimeSpan timeout, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var sw = Stopwatch.StartNew();

        while (sw.Elapsed < timeout)
        {
            await Task.Delay(500, cancellationToken);

            if (process.HasExited)
            {
                scopedLogger.LogError("Guild Wars process exited before the main window was shown. Process ID: {ProcessId}", process.Id);
                return false;
            }

            if (process.MainWindowHandle == IntPtr.Zero)
            {
                continue;
            }

            return true;
        }

        scopedLogger.LogError("Timed out waiting for Guild Wars to be ready");
        return false;
    }
}
