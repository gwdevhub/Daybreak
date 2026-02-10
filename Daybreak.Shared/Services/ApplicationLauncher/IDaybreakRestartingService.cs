namespace Daybreak.Shared.Services.ApplicationLauncher;

/// <summary>
/// Platform-specific service for restarting the Daybreak application.
/// </summary>
public interface IDaybreakRestartingService
{
    /// <summary>
    /// Restarts Daybreak, choosing the appropriate privilege level based on current state.
    /// </summary>
    void RestartDaybreak();

    /// <summary>
    /// Restarts Daybreak with elevated privileges.
    /// </summary>
    void RestartDaybreakAsAdmin();

    /// <summary>
    /// Restarts Daybreak as a normal user (without elevation).
    /// </summary>
    void RestartDaybreakAsNormalUser();
}
