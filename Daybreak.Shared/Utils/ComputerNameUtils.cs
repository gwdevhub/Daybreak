using System.Text;

namespace Daybreak.Shared.Utils;

/// <summary>
/// Helpers for producing Windows compatible computer names.
/// </summary>
public static class ComputerNameUtils
{
    /// <summary>
    /// Maximum length of a Windows NetBIOS computer name, matching the Win32 MAX_COMPUTERNAME_LENGTH constant.
    /// </summary>
    public const int MaxComputerNameLength = 15;

    /// <summary>
    /// Name used when no valid computer name can be derived from the host.
    /// </summary>
    public const string FallbackComputerName = "DAYBREAK";

    /// <summary>
    /// Converts an arbitrary host name into a name that Win32 GetComputerName can return.
    /// </summary>
    /// <remarks>
    /// GetComputerNameW is documented to fill a buffer of MAX_COMPUTERNAME_LENGTH + 1 characters, so callers
    /// size their buffers accordingly. When the configured name is longer, the call fails with
    /// ERROR_BUFFER_OVERFLOW and leaves the caller's buffer untouched, which applications such as GWToolbox
    /// observe as an empty computer name. Wine's own wineboot upper-cases and truncates the Linux host name
    /// for the same reason, so this method reproduces that behaviour.
    /// </remarks>
    /// <param name="name">The host name to sanitize.</param>
    /// <returns>An upper-case, at most <see cref="MaxComputerNameLength"/> characters long, non-empty computer name.</returns>
    public static string SanitizeComputerName(string? name)
    {
        if (name is null)
        {
            return FallbackComputerName;
        }

        var builder = new StringBuilder(MaxComputerNameLength);
        foreach (var c in name)
        {
            if (builder.Length >= MaxComputerNameLength)
            {
                break;
            }

            if (char.IsAsciiLetterOrDigit(c))
            {
                builder.Append(char.ToUpperInvariant(c));
            }
            else if (c is '-' or '_')
            {
                builder.Append(c);
            }
        }

        // A computer name may not start or end with a separator.
        while (builder.Length > 0 && builder[^1] is '-' or '_')
        {
            builder.Length--;
        }

        var sanitized = builder.ToString().TrimStart('-', '_');
        return sanitized.Length is 0 ? FallbackComputerName : sanitized;
    }
}
