using Daybreak.Shared.Services.Api;

namespace Daybreak.Windows.Services.Api;

/// <summary>
/// Windows implementation of <see cref="IPidProvider"/>.
/// On Windows, the reported PID is the actual system PID, so this is a pass-through.
/// </summary>
public sealed class PidProvider : IPidProvider
{
    /// <inheritdoc />
    public int ResolveSystemPid(int reportedPid, string executableName)
    {
        // On Windows, the reported PID is the actual system PID
        return reportedPid;
    }
}
