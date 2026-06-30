using Daybreak.Shared.Models;
using System.Diagnostics;

namespace Daybreak.Injector;

/// <summary>
/// Resolves a Wine-internal process id from a Guild Wars executable path.
///
/// On Linux multiple Guild Wars instances (each in its own install directory) run under
/// the same Wine prefix. Matching by executable name alone cannot tell them apart, so we
/// enumerate the running processes and compare each one's full image path — which the
/// caller already knows from the Linux side — to find the unique owning Wine process.
///
/// This runs inside Wine, where <see cref="Process.Id"/> is the Wine pid, so the matched
/// id is exactly what the stub/winapi injectors expect.
/// </summary>
public static class ProcessResolver
{
    private const string GuildWarsProcessName = "Gw";

    public static InjectorResponses.ResolveResult Resolve(string executablePath, out int processId)
    {
        processId = 0;
        var target = NormalizePath(executablePath);

        foreach (var process in Process.GetProcessesByName(GuildWarsProcessName))
        {
            try
            {
                if (TryGetImagePath(process.Id) is { } imagePath &&
                    string.Equals(NormalizePath(imagePath), target, StringComparison.Ordinal))
                {
                    processId = process.Id;
                    return InjectorResponses.ResolveResult.Success;
                }
            }
            finally
            {
                process.Dispose();
            }
        }

        return InjectorResponses.ResolveResult.ProcessNotFound;
    }

    private static string? TryGetImagePath(int processId)
    {
        var handle = NativeMethods.OpenProcess(
            NativeMethods.ProcessAccessFlags.QueryLimitedInformation, false, (uint)processId);
        if (handle is 0)
        {
            return null;
        }

        try
        {
            var buffer = new char[1024];
            var size = (uint)buffer.Length;
            if (!NativeMethods.QueryFullProcessImageName(handle, 0, buffer, ref size) || size is 0)
            {
                return null;
            }

            return new string(buffer, 0, (int)size);
        }
        finally
        {
            NativeMethods.CloseHandle(handle);
        }
    }

    /// <summary>
    /// Normalizes a Windows/Wine path for comparison: unifies separators, trims a trailing
    /// null/whitespace, and lower-cases (Windows paths are case-insensitive).
    /// </summary>
    private static string NormalizePath(string path) =>
        path.Trim().TrimEnd('\0').Replace('/', '\\').ToLowerInvariant();
}
