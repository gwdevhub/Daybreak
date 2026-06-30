using Daybreak.Linux.Services.Wine;
using Daybreak.Shared.Services.Api;
using System.Globalization;

namespace Daybreak.Linux.Services.Api;

/// <summary>
/// Linux implementation of <see cref="IPidProvider"/>.
/// On Linux, the reported PID is a Wine-internal PID. The most reliable way to map an
/// API instance to its Linux process is via the TCP port it listens on: we resolve the
/// owning Linux PID from the listening socket. This disambiguates multiple concurrent
/// Guild Wars instances, which a Wine-PID-by-name lookup cannot do.
/// </summary>
public sealed class PidProvider(IWinePidMapper winePidMapper) : IPidProvider
{
    private readonly IWinePidMapper winePidMapper = winePidMapper;

    /// <inheritdoc />
    public int ResolveSystemPid(int reportedPid, string executableName, int? port = null)
    {
        if (port is not null && TryGetProcessIdForTcpListener(port.Value, executableName) is { } listenerPid)
        {
            return listenerPid;
        }

        // Fallback: the reported PID is a Wine PID. Convert to Linux system PID by name.
        var linuxPid = this.winePidMapper.WinePidToLinuxPid(reportedPid, executableName);
        return linuxPid ?? reportedPid;
    }

    /// <summary>
    /// Resolves the Linux PID that owns the listening socket on <paramref name="port"/>.
    /// Under Wine the listening socket fd is shared between the game process and the
    /// shared <c>wineserver</c> process, so candidate processes are filtered by
    /// <paramref name="executableName"/> to select the game process and exclude wineserver.
    /// </summary>
    private static int? TryGetProcessIdForTcpListener(int port, string executableName)
    {
        try
        {
            var socketInodes = GetTcpListenerSocketInodes("/proc/net/tcp", port)
                .Concat(GetTcpListenerSocketInodes("/proc/net/tcp6", port))
                .ToHashSet(StringComparer.Ordinal);
            if (socketInodes.Count == 0)
            {
                return null;
            }

            foreach (var procDir in Directory.EnumerateDirectories("/proc"))
            {
                if (!int.TryParse(Path.GetFileName(procDir), out var pid))
                {
                    continue;
                }

                // Only consider processes whose command line references the target
                // executable. This excludes wineserver (which shares the socket fd)
                // and limits the fd scan to the handful of Guild Wars processes.
                if (!ProcessReferencesExecutable(procDir, executableName))
                {
                    continue;
                }

                if (ProcessOwnsSocket(procDir, socketInodes))
                {
                    return pid;
                }
            }
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }

        return null;
    }

    private static bool ProcessReferencesExecutable(string procDir, string executableName)
    {
        try
        {
            var cmdline = File.ReadAllText(Path.Combine(procDir, "cmdline"));
            return cmdline.Contains(executableName, StringComparison.OrdinalIgnoreCase);
        }
        catch (IOException)
        {
            return false;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
    }

    private static bool ProcessOwnsSocket(string procDir, HashSet<string> socketInodes)
    {
        var fdDir = Path.Combine(procDir, "fd");
        if (!Directory.Exists(fdDir))
        {
            return false;
        }

        try
        {
            foreach (var fd in Directory.EnumerateFiles(fdDir))
            {
                string? linkTarget;
                try
                {
                    linkTarget = new FileInfo(fd).LinkTarget;
                }
                catch (IOException)
                {
                    continue;
                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }

                if (linkTarget is not null &&
                    TryParseSocketInode(linkTarget, out var inode) &&
                    socketInodes.Contains(inode))
                {
                    return true;
                }
            }
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }

        return false;
    }

    private static IEnumerable<string> GetTcpListenerSocketInodes(string procNetPath, int port)
    {
        if (!File.Exists(procNetPath))
        {
            yield break;
        }

        foreach (var line in File.ReadLines(procNetPath).Skip(1))
        {
            var columns = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (columns.Length < 10 ||
                !IsListeningSocket(columns[3]) ||
                !TryParsePort(columns[1], out var socketPort) ||
                socketPort != port)
            {
                continue;
            }

            yield return columns[9];
        }
    }

    private static bool TryParsePort(string localAddress, out int port)
    {
        port = default;
        var separatorIndex = localAddress.LastIndexOf(':');
        if (separatorIndex < 0 ||
            separatorIndex == localAddress.Length - 1 ||
            !int.TryParse(localAddress[(separatorIndex + 1)..], NumberStyles.HexNumber, null, out port))
        {
            return false;
        }

        return true;
    }

    private static bool IsListeningSocket(string state) => state.Equals("0A", StringComparison.OrdinalIgnoreCase);

    private static bool TryParseSocketInode(string linkTarget, out string inode)
    {
        inode = string.Empty;
        const string socketPrefix = "socket:[";
        if (!linkTarget.StartsWith(socketPrefix, StringComparison.Ordinal) || !linkTarget.EndsWith(']'))
        {
            return false;
        }

        inode = linkTarget[socketPrefix.Length..^1];
        return inode.Length > 0;
    }
}
