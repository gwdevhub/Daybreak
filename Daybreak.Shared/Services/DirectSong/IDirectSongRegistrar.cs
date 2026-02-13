using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Services.Downloads;

namespace Daybreak.Shared.Services.DirectSong;

/// <summary>
/// Handles DirectSong registration including registry entries, DLL overrides, and platform-specific file management.
/// On Windows this runs the exe natively; on Linux it sets up Wine registry and DLL overrides.
/// </summary>
public interface IDirectSongRegistrar
{
    /// <summary>
    /// Registers the DirectSong directory in the registry.
    /// On Windows: Runs the registry editor executable.
    /// On Linux: Sets the MusicPath registry entry via Wine.
    /// </summary>
    Task<bool> RegisterDirectSongDirectory(string installationDirectory, string registryEditorName, CancellationToken cancellationToken);

    /// <summary>
    /// Sets up DLL overrides required for DirectSong.
    /// On Windows: No-op (returns true).
    /// On Linux: Sets wmvcore and wmasf to native,builtin.
    /// </summary>
    Task<bool> SetupDllOverrides(CancellationToken cancellationToken);

    /// <summary>
    /// Checks if the gstreamer library with libav plugin is available.
    /// On Windows: Always returns true.
    /// On Linux: Checks for gstreamer1.0-libav or equivalent.
    /// </summary>
    /// <returns>True if available, false if missing (user needs to install it).</returns>
    bool IsGStreamerAvailable();

    /// <summary>
    /// Gets a user-friendly message about how to install gstreamer.
    /// </summary>
    string GetGStreamerInstallInstructions();

    /// <summary>
    /// Checks if platform-specific files are installed.
    /// On Windows: Checks for WMVCORE.DLL and ds_GuildWars.dll in the installation directory.
    /// On Linux: Checks for WMVCORE.DLL and WMASF.DLL in the Linux subdirectory + ds_GuildWars.dll.
    /// </summary>
    bool ArePlatformFilesInstalled(string installationDirectory);

    /// <summary>
    /// Downloads and sets up platform-specific files.
    /// On Windows: No-op (files come from DirectSong.7z).
    /// On Linux: Downloads WMVCORE.DLL and WMASF.DLL from GitHub to a Linux subdirectory.
    /// </summary>
    Task<bool> SetupPlatformFiles(string installationDirectory, IDownloadService downloadService, IProgress<ProgressUpdate> progress, CancellationToken cancellationToken);

    /// <summary>
    /// Copies DirectSong DLLs to the Guild Wars directory.
    /// On Windows: Copies WMVCORE.DLL and ds_GuildWars.dll.
    /// On Linux: Copies WMVCORE.DLL, WMASF.DLL from Linux subdir + ds_GuildWars.dll.
    /// </summary>
    void CopyFilesToGuildWars(string installationDirectory, string gwDirectory);

    /// <summary>
    /// Removes DirectSong DLLs from the Guild Wars directory.
    /// On Windows: Removes WMVCORE.DLL and ds_GuildWars.dll.
    /// On Linux: Removes WMVCORE.DLL, WMASF.DLL, and ds_GuildWars.dll.
    /// </summary>
    void RemoveFilesFromGuildWars(string gwDirectory);
}
