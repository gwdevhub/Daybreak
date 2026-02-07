using PeNet;

namespace Daybreak.Utils;

/// <summary>
/// Cross-platform PE version reader using PeNet.
/// </summary>
internal static class PeVersionReader
{
    /// <summary>
    /// Reads the ProductVersion string from a PE file's version resource.
    /// Returns null if the file doesn't exist or has no version info.
    /// </summary>
    public static string? GetProductVersion(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            var peFile = new PeFile(filePath);
            return peFile.Resources?.VsVersionInfo?.StringFileInfo?.StringTable
                ?.FirstOrDefault()?.ProductVersion;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Reads the FileVersion string from a PE file's version resource.
    /// Returns null if the file doesn't exist or has no version info.
    /// </summary>
    public static string? GetFileVersion(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            var peFile = new PeFile(filePath);
            return peFile.Resources?.VsVersionInfo?.StringFileInfo?.StringTable
                ?.FirstOrDefault()?.FileVersion;
        }
        catch
        {
            return null;
        }
    }
}
