using System.IO;
using System.Reflection;

namespace Daybreak.Shared.Utils;
public static class PathUtils
{
    private const string WineDrivePrefix = "Z:";

    private static readonly Lazy<string> RootPath = new(() => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException("Unable to obtain application root path"));

    public static string GetRootFolder()
    {
        return RootPath.Value;
    }

    public static string GetAbsolutePathFromRoot(params string[] subPaths)
    {
        var paths = subPaths.Prepend(GetRootFolder()).ToArray();
        return Path.GetFullPath(Path.Combine(paths));
    }

    /// <summary>
    /// Converts a Linux path to a Wine-compatible path using the Z: drive.
    /// Wine exposes the entire Linux filesystem through Z:/
    /// </summary>
    /// <param name="linuxPath">The native Linux path (e.g., /mnt/games/Gw.exe)</param>
    /// <returns>The Wine-compatible path (e.g., Z:\mnt\games\Gw.exe)</returns>
    public static string ToWinePath(string linuxPath)
    {
        // Normalize the path and convert forward slashes to backslashes
        var normalizedPath = Path.GetFullPath(linuxPath);
        var windowsPath = normalizedPath.Replace('/', '\\');
        return $"{WineDrivePrefix}{windowsPath}";
    }
}
