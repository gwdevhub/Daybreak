using Daybreak.Shared.Services.Themes;
using System.Diagnostics;

namespace Daybreak.Linux.Services.Themes;

/// <summary>
/// Linux implementation of ISystemThemeDetector.
/// Checks the XDG Desktop Portal color-scheme (works on Hyprland, Sway, GNOME, KDE, etc.),
/// falls back to gsettings for GNOME-based desktops, then GTK_THEME env var.
/// </summary>
internal sealed class SystemThemeDetector : ISystemThemeDetector
{
    public bool IsLightTheme()
    {
        if (TryReadXdgPortalColorScheme(out var portalResult))
        {
            return portalResult;
        }

        if (TryReadGsettingsColorScheme(out var gsettingsResult))
        {
            return gsettingsResult;
        }

        var gtkThemeEnv = Environment.GetEnvironmentVariable("GTK_THEME");
        if (!string.IsNullOrWhiteSpace(gtkThemeEnv))
        {
            return !gtkThemeEnv.Contains("dark", StringComparison.OrdinalIgnoreCase);
        }

        return false; // Default to dark
    }

    /// <summary>
    /// Queries org.freedesktop.appearance color-scheme via the XDG Desktop Portal D-Bus interface.
    /// Returns: 0 = no preference, 1 = prefer dark, 2 = prefer light.
    /// Requires xdg-desktop-portal (xdg-desktop-portal-hyprland, -gnome, -kde, -wlr, etc.)
    /// </summary>
    private static bool TryReadXdgPortalColorScheme(out bool isLight)
    {
        isLight = false;
        try
        {
            var output = RunCommand("busctl", "--user call org.freedesktop.portal.Desktop " +
                "/org/freedesktop/portal/desktop org.freedesktop.portal.Settings " +
                "Read ss org.freedesktop.appearance color-scheme");
            if (output is null)
            {
                return false;
            }

            // busctl output looks like: v u 1
            // where the last number is: 0=no preference, 1=dark, 2=light
            if (output.Contains('2'))
            {
                isLight = true;
            }
            else if (output.Contains('1'))
            {
                isLight = false;
            }
            else
            {
                // 0 = no preference, treat as light
                isLight = true;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool TryReadGsettingsColorScheme(out bool isLight)
    {
        isLight = false;
        try
        {
            // GNOME 42+ color-scheme setting
            var colorScheme = RunCommand("gsettings", "get org.gnome.desktop.interface color-scheme");
            if (colorScheme is not null)
            {
                isLight = !colorScheme.Contains("dark", StringComparison.OrdinalIgnoreCase);
                return true;
            }

            // Older GTK3 theme name fallback
            var gtkTheme = RunCommand("gsettings", "get org.gnome.desktop.interface gtk-theme");
            if (gtkTheme is not null)
            {
                isLight = !gtkTheme.Contains("dark", StringComparison.OrdinalIgnoreCase);
                return true;
            }
        }
        catch
        {
        }

        return false;
    }

    private static string? RunCommand(string command, string arguments)
    {
        try
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var output = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit(TimeSpan.FromSeconds(2));
            return string.IsNullOrWhiteSpace(output) ? null : output;
        }
        catch
        {
            return null;
        }
    }
}
