namespace Daybreak.Shared.Services.UMod;

/// <summary>
/// Resolves native file paths to the format expected by mod loaders running inside the game process.
/// On Windows, paths are returned as-is. On Linux, paths are converted to Wine-compatible paths.
/// </summary>
public interface IModPathResolver
{
    /// <summary>
    /// Converts a native file path to the format expected by the mod loader.
    /// </summary>
    string ResolveForModLoader(string nativePath);
}
