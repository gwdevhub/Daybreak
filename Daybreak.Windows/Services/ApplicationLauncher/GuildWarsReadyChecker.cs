using Daybreak.Shared.Services.ApplicationLauncher;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions.Core;
using System.Runtime.InteropServices;
using System.Text;
using static Daybreak.Shared.Utils.NativeMethods;

namespace Daybreak.Windows.Services.ApplicationLauncher;

/// <summary>
/// Windows-specific implementation that waits for the Guild Wars window to be fully ready.
/// Uses Win32 window enumeration to detect when the game window is shown and responsive.
/// </summary>
internal sealed class GuildWarsReadyChecker(
    ILogger<GuildWarsReadyChecker> logger
) : IGuildWarsReadyChecker
{
    private const double LaunchMemoryThreshold = 200000000;

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

            var windows = GetRootWindowsOfProcess(process.Id)
                .Select(root => (root, GetChildWindows(root)))
                .SelectMany(tuple =>
                {
                    tuple.Item2.Add(tuple.root);
                    return tuple.Item2;
                })
                .Select(GetWindowTitle).ToList();

            /*
             * Detect when the game window has shown. Because both the updater and the game window are called Guild Wars,
             * we need to look at the other windows created by the process. Especially, we need to detect the input windows
             * to check when the game is ready to accept input
             */
            if (!windows.Contains("Guild Wars Reforged"))
            {
                continue;
            }

            var virtualMemory = process.VirtualMemorySize64;
            if (virtualMemory < LaunchMemoryThreshold)
            {
                continue;
            }

            return true;
        }

        scopedLogger.LogError("Timed out waiting for Guild Wars to be ready");
        return false;
    }

    private static List<IntPtr> GetRootWindowsOfProcess(int pid)
    {
        var rootWindows = GetChildWindows(IntPtr.Zero);
        var dsProcRootWindows = new List<IntPtr>();
        foreach (IntPtr hWnd in rootWindows)
        {
            _ = GetWindowThreadProcessId(hWnd, out var lpdwProcessId);
            if (lpdwProcessId == pid)
                dsProcRootWindows.Add(hWnd);
        }

        return dsProcRootWindows;
    }

    private static List<IntPtr> GetChildWindows(IntPtr parent)
    {
        var result = new List<IntPtr>();
        var listHandle = GCHandle.Alloc(result);
        try
        {
            var childProc = new Win32Callback(EnumWindow);
            EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
        }
        finally
        {
            if (listHandle.IsAllocated)
                listHandle.Free();
        }

        return result;
    }

    private static string GetWindowTitle(IntPtr hwnd)
    {
        var titleLength = GetWindowTextLength(hwnd);
        var titleBuffer = new StringBuilder(titleLength);
        _ = GetWindowText(hwnd, titleBuffer, titleLength + 1);
        return titleBuffer.ToString();
    }

    private static bool EnumWindow(IntPtr handle, IntPtr pointer)
    {
        var gch = GCHandle.FromIntPtr(pointer);
        if (gch.Target is not List<IntPtr> list)
        {
            return false;
        }

        list.Add(handle);
        return true;
    }
}
