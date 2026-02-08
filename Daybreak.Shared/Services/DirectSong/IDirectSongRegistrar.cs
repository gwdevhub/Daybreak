namespace Daybreak.Shared.Services.DirectSong;

/// <summary>
/// Runs the DirectSong registry editor executable to register the DirectSong installation directory.
/// On Windows this runs the exe natively; on Linux it runs through Wine.
/// </summary>
public interface IDirectSongRegistrar
{
    Task<bool> RegisterDirectSongDirectory(string installationDirectory, string registryEditorName, CancellationToken cancellationToken);
}
