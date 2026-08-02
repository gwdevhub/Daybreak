namespace Daybreak.Shared.Services.ApplicationLauncher;

/// <summary>
/// Platform-specific service for interacting with the Steam client.
/// Used to determine whether Steam is available before launching Guild Wars with Steam login.
/// </summary>
public interface ISteamService
{
    /// <summary>
    /// Returns true if Steam login is supported on the current platform.
    /// Steam login is currently only supported on Windows.
    /// </summary>
    bool IsSteamLoginSupported { get; }

    /// <summary>
    /// Returns true if the Steam client is currently running.
    /// </summary>
    bool IsSteamRunning();
}
