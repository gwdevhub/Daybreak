using System.Diagnostics;
using Daybreak.Shared.Services.ApplicationLauncher;

namespace Daybreak.Windows.Services.ApplicationLauncher;

/// <summary>
/// Windows-specific Steam service. Detects the running Steam client by process name.
/// </summary>
public sealed class SteamService : ISteamService
{
    private const string SteamProcessName = "steam";

    public bool IsSteamLoginSupported => true;

    public bool IsSteamRunning()
    {
        return Process.GetProcessesByName(SteamProcessName).Length > 0;
    }
}
