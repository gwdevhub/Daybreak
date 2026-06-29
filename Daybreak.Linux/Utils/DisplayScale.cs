using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Daybreak.Linux.Utils;

/// <summary>
/// Detects the effective desktop UI scale factor on Linux so the Photino webview
/// can be zoomed to match HiDPI monitors.
/// </summary>
/// <remarks>
/// <para>
/// This is needed because Daybreak forces the GTK X11 backend (see
/// <c>Launcher.ForceX11Backend</c>): under XWayland, GTK/GDK and therefore
/// Photino's built-in <c>gdk_monitor_get_scale_factor</c> based auto-zoom always
/// report a scale of 1, so a fractional Wayland scale (e.g. 1.5) is invisible and
/// the UI renders too small.
/// </para>
/// <para>
/// Detection is layered, first match wins, so it degrades safely across
/// environments:
/// <list type="number">
///   <item><c>GDK_SCALE</c> * <c>GDK_DPI_SCALE</c> environment override (any session).</item>
///   <item>Wayland session: the focused monitor's scale from the compositor
///   (hyprctl / swaymsg / wlr-randr / kscreen-doctor). Shelling out keeps the
///   detection out-of-process so a misbehaving tool cannot crash the launcher.</item>
///   <item>X11 session: <c>Xft.dpi</c> / 96 (set by KDE/GNOME when scaling on X11).</item>
///   <item>Fallback: 1.0 (current behaviour, no regression).</item>
/// </list>
/// </para>
/// </remarks>
internal static class DisplayScale
{
    private const double MinScale = 0.5;
    private const double MaxScale = 4.0;
    private const int ProcessTimeoutMs = 2000;

    /// <summary>
    /// Returns the effective UI scale (e.g. 1.0, 1.5, 2.0), clamped to a sane range.
    /// Returns 1.0 if nothing could be determined.
    /// </summary>
    public static double GetEffectiveScale(ILogger logger)
    {
        if (TryGetEnvScale(out var envScale))
        {
            logger.LogDebug("Display scale {Scale} from GDK_SCALE/GDK_DPI_SCALE", envScale);
            return Clamp(envScale);
        }

        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WAYLAND_DISPLAY")))
        {
            if (TryGetWaylandScale(logger, out var wlScale))
            {
                logger.LogDebug("Display scale {Scale} from Wayland compositor", wlScale);
                return Clamp(wlScale);
            }
        }
        else if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DISPLAY")))
        {
            if (TryGetXftDpiScale(logger, out var x11Scale))
            {
                logger.LogDebug("Display scale {Scale} from Xft.dpi", x11Scale);
                return Clamp(x11Scale);
            }
        }

        logger.LogDebug("No display scale detected; defaulting to 1.0");
        return 1.0;
    }

    private static bool TryGetEnvScale(out double scale)
    {
        scale = 1.0;
        var gdkScale = Environment.GetEnvironmentVariable("GDK_SCALE");
        var gdkDpiScale = Environment.GetEnvironmentVariable("GDK_DPI_SCALE");
        var any = false;

        if (double.TryParse(gdkScale, NumberStyles.Float, CultureInfo.InvariantCulture, out var gs) && gs > 0)
        {
            scale *= gs;
            any = true;
        }

        if (double.TryParse(gdkDpiScale, NumberStyles.Float, CultureInfo.InvariantCulture, out var gd) && gd > 0)
        {
            scale *= gd;
            any = true;
        }

        return any;
    }

    private static bool TryGetWaylandScale(ILogger logger, out double scale)
    {
        scale = 1.0;

        // hyprctl and swaymsg both emit a JSON array of monitors/outputs with
        // "focused" and "scale" fields, so they share a parser. The focused
        // output is where a new window spawns, which is exactly what we want.
        if (TryRun("hyprctl", "monitors -j", logger, out var hypr) &&
            TryParseFocusedScale(hypr, out scale))
        {
            return true;
        }

        if (TryRun("swaymsg", "-t get_outputs -r", logger, out var sway) &&
            TryParseFocusedScale(sway, out scale))
        {
            return true;
        }

        // wlr-randr (generic wlroots) and kscreen-doctor (KDE) have no "focused"
        // flag; fall back to the first scaled output reported.
        if (TryRun("wlr-randr", "--json", logger, out var wlr) &&
            TryParseFirstScale(wlr, "scale", out scale))
        {
            return true;
        }

        if (TryRun("kscreen-doctor", "-j", logger, out var kscreen) &&
            TryParseKScreenScale(kscreen, out scale))
        {
            return true;
        }

        return false;
    }

    private static bool TryParseFocusedScale(string json, out double scale)
    {
        scale = 1.0;
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.ValueKind != JsonValueKind.Array)
            {
                return false;
            }

            double? firstScale = null;
            foreach (var output in doc.RootElement.EnumerateArray())
            {
                if (!output.TryGetProperty("scale", out var scaleProp) ||
                    scaleProp.ValueKind != JsonValueKind.Number)
                {
                    continue;
                }

                var value = scaleProp.GetDouble();
                firstScale ??= value;

                if (output.TryGetProperty("focused", out var focused) &&
                    focused.ValueKind is JsonValueKind.True)
                {
                    scale = value;
                    return value > 0;
                }
            }

            if (firstScale is > 0)
            {
                scale = firstScale.Value;
                return true;
            }
        }
        catch (JsonException)
        {
        }

        return false;
    }

    private static bool TryParseFirstScale(string json, string property, out double scale)
    {
        scale = 1.0;
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.ValueKind != JsonValueKind.Array)
            {
                return false;
            }

            foreach (var output in doc.RootElement.EnumerateArray())
            {
                if (output.TryGetProperty(property, out var scaleProp) &&
                    scaleProp.ValueKind == JsonValueKind.Number)
                {
                    var value = scaleProp.GetDouble();
                    if (value > 0)
                    {
                        scale = value;
                        return true;
                    }
                }
            }
        }
        catch (JsonException)
        {
        }

        return false;
    }

    private static bool TryParseKScreenScale(string json, out double scale)
    {
        scale = 1.0;
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("outputs", out var outputs) ||
                outputs.ValueKind != JsonValueKind.Array)
            {
                return false;
            }

            double? firstScale = null;
            foreach (var output in outputs.EnumerateArray())
            {
                if (!output.TryGetProperty("scale", out var scaleProp) ||
                    scaleProp.ValueKind != JsonValueKind.Number)
                {
                    continue;
                }

                var value = scaleProp.GetDouble();
                firstScale ??= value;

                if (output.TryGetProperty("enabled", out var enabled) &&
                    enabled.ValueKind is JsonValueKind.True &&
                    value > 0)
                {
                    scale = value;
                    return true;
                }
            }

            if (firstScale is > 0)
            {
                scale = firstScale.Value;
                return true;
            }
        }
        catch (JsonException)
        {
        }

        return false;
    }

    private static bool TryGetXftDpiScale(ILogger logger, out double scale)
    {
        scale = 1.0;
        if (!TryRun("xrdb", "-query", logger, out var output))
        {
            return false;
        }

        foreach (var line in output.Split('\n'))
        {
            if (!line.StartsWith("Xft.dpi:", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var value = line["Xft.dpi:".Length..].Trim();
            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var dpi) && dpi > 0)
            {
                scale = dpi / 96.0;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Runs a process and returns its stdout, or false if the executable is
    /// missing, times out, or exits non-zero.
    /// </summary>
    private static bool TryRun(string fileName, string arguments, ILogger logger, out string output)
    {
        output = string.Empty;
        try
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                },
            };

            if (!process.Start())
            {
                return false;
            }

            var stdout = process.StandardOutput.ReadToEnd();
            if (!process.WaitForExit(ProcessTimeoutMs))
            {
                try
                {
                    process.Kill(entireProcessTree: true);
                }
                catch
                {
                    // Best effort; the process will be reaped by the OS.
                }

                return false;
            }

            if (process.ExitCode != 0)
            {
                return false;
            }

            output = stdout;
            return !string.IsNullOrWhiteSpace(output);
        }
        catch (Exception e) when (e is System.ComponentModel.Win32Exception or InvalidOperationException)
        {
            // Tool not installed / not on PATH — expected, try the next source.
            logger.LogDebug("Display scale tool '{Tool}' unavailable: {Message}", fileName, e.Message);
            return false;
        }
    }

    private static double Clamp(double scale) => Math.Clamp(scale, MinScale, MaxScale);
}
