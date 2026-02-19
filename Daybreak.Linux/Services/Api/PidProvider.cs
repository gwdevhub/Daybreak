using Daybreak.Linux.Services.Wine;
using Daybreak.Shared.Services.Api;

namespace Daybreak.Linux.Services.Api;

/// <summary>
/// Linux implementation of <see cref="IPidProvider"/>.
/// On Linux, the reported PID is a Wine-internal PID that must be
/// converted to the actual Linux system PID via <see cref="IWinePidMapper"/>.
/// </summary>
public sealed class PidProvider(IWinePidMapper winePidMapper) : IPidProvider
{
    private readonly IWinePidMapper winePidMapper = winePidMapper;

    /// <inheritdoc />
    public int ResolveSystemPid(int reportedPid, string executableName)
    {
        // On Linux, the reported PID is a Wine PID. Convert to Linux system PID.
        var linuxPid = this.winePidMapper.WinePidToLinuxPid(reportedPid, executableName);
        return linuxPid ?? reportedPid;
    }
}
