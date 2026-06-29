using Daybreak.Configuration.Options;
using Daybreak.Linux.Utils;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Screens;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Photino.NET;
using System.Core.Extensions;
using System.Drawing;
using System.Extensions;
using System.Extensions.Core;

namespace Daybreak.Linux.Services.Screens;

/// <summary>
/// Linux screen management backed by GDK monitor enumeration. Reports real
/// monitor geometry and tracks each monitor's integer scale factor so the window
/// spawns sized to (and is restored on) the correct monitor. Scale is persisted
/// as DPI (scale * 96) in <see cref="ScreenManagerOptions"/>, mirroring the
/// Windows implementation, so a window saved on one monitor is rescaled when
/// restored on a monitor with a different scale.
/// </summary>
/// <remarks>
/// GDK reports an integer scale factor; under X11/XWayland (Daybreak forces the
/// X11 backend) this reflects GDK_SCALE / Xft.dpi-derived scaling. True
/// fractional per-monitor scaling would require the Wayland backend.
/// </remarks>
internal sealed class ScreenManager(
    PhotinoWindow photinoWindow,
    IOptionsProvider optionsProvider,
    IOptionsMonitor<ScreenManagerOptions> liveUpdateableOptions,
    ILogger<ScreenManager> logger) : IScreenManager, IHostedService
{
    private const int DefaultDpi = 96;

    private readonly PhotinoWindow photinoWindow = photinoWindow.ThrowIfNull();
    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull();
    private readonly IOptionsMonitor<ScreenManagerOptions> liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
    private readonly ILogger<ScreenManager> logger = logger.ThrowIfNull();

    private readonly record struct MonitorInfo(Screen Screen, bool IsPrimary);

    // Enumerated once (lazily) and cached; provides real monitor geometry for
    // window placement. Monitor scale comes from DisplayScale (not GDK), because
    // under the forced X11 backend GDK always reports a scale of 1.
    private IReadOnlyList<MonitorInfo>? cachedMonitors;
    private double? cachedScale;

    public IEnumerable<Screen> Screens => this.GetMonitors().Select(static m => m.Screen);

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        this.ApplyDisplayScale();
        return Task.CompletedTask;
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        this.SaveWindowPositionAndSize();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Zooms the Photino webview to match the desktop UI scale so the interface is
    /// rendered at the correct physical size on HiDPI monitors. Photino's built-in
    /// scale detection is defeated by the forced X11 backend, so this is applied
    /// explicitly. Must run on the UI thread, hence the <see cref="PhotinoWindow.Invoke"/>.
    /// </summary>
    private void ApplyDisplayScale()
    {
        var scale = this.GetEffectiveScale();
        var zoom = (int)Math.Round(scale * 100.0);
        if (zoom == 100)
        {
            return;
        }

        this.logger.LogDebug("Applying webview zoom {Zoom}% for display scale {Scale}", zoom, scale);
        this.photinoWindow.Invoke(() => this.photinoWindow.Zoom = zoom);
    }

    private double GetEffectiveScale()
    {
        this.cachedScale ??= DisplayScale.GetEffectiveScale(this.logger);
        return this.cachedScale.Value;
    }

    public void MoveWindowToSavedPosition()
    {
        var savedPosition = this.GetSavedPosition();
        this.photinoWindow.Left = savedPosition.Left;
        this.photinoWindow.Top = savedPosition.Top;
        this.photinoWindow.Width = savedPosition.Width;
        this.photinoWindow.Height = savedPosition.Height;
    }

    public void SaveWindowPositionAndSize()
    {
        var position = new Rectangle(
            this.photinoWindow.Left,
            this.photinoWindow.Top,
            this.photinoWindow.Width,
            this.photinoWindow.Height);

        var dpi = this.GetDpiForPosition(position.Left, position.Top);

        var options = this.liveUpdateableOptions.CurrentValue;
        options.X = position.Left;
        options.Y = position.Top;
        options.Width = position.Width;
        options.Height = position.Height;
        options.DpiX = dpi;
        options.DpiY = dpi;
        this.optionsProvider.SaveOption(options);
    }

    public void ResetSavedPosition()
    {
        var options = this.liveUpdateableOptions.CurrentValue;
        options.X = 0;
        options.Y = 0;
        options.Width = 0;
        options.Height = 0;
        options.DpiX = DefaultDpi;
        options.DpiY = DefaultDpi;
        this.optionsProvider.SaveOption(options);
    }

    // TODO: Implement for Linux - this is Windows-specific
    public bool MoveGuildwarsToScreen(Screen screen)
    {
        this.logger.LogWarning("MoveGuildwarsToScreen is not implemented on Linux");
        return false;
    }

    public Rectangle GetSavedPosition()
    {
        var options = this.liveUpdateableOptions.CurrentValue;
        var savedPosition = new Rectangle(
            (int)options.X,
            (int)options.Y,
            (int)options.Width,
            (int)options.Height);

        if (savedPosition.Width is 0 || savedPosition.Height is 0)
        {
            return this.GetDefaultPosition();
        }

        // Rescale the saved geometry if the monitor under it now has a different
        // scale than when it was saved (e.g. monitor swapped, scale changed).
        var savedDpiX = options.DpiX > 0 ? options.DpiX : DefaultDpi;
        var savedDpiY = options.DpiY > 0 ? options.DpiY : DefaultDpi;
        var currentDpi = this.GetDpiForPosition(savedPosition.Left, savedPosition.Top);

        if (savedDpiX != currentDpi || savedDpiY != currentDpi)
        {
            var scaleX = (double)currentDpi / savedDpiX;
            var scaleY = (double)currentDpi / savedDpiY;
            savedPosition = new Rectangle(
                (int)(savedPosition.X * scaleX),
                (int)(savedPosition.Y * scaleY),
                (int)(savedPosition.Width * scaleX),
                (int)(savedPosition.Height * scaleY));
        }

        return this.EnsurePositionIsOnScreen(savedPosition);
    }

    private Rectangle GetDefaultPosition()
    {
        var monitor = this.GetPrimaryMonitor();
        if (monitor is not { } m || m.Screen.Size.IsEmpty)
        {
            // Reasonable fallback when GDK could not enumerate any monitor.
            return new Rectangle(0, 100, 1000, 900);
        }

        var bounds = m.Screen.Size;
        return new Rectangle(
            bounds.X + (bounds.Width / 4),
            bounds.Y + (bounds.Height / 4),
            bounds.Width / 2,
            bounds.Height / 2);
    }

    private Rectangle EnsurePositionIsOnScreen(Rectangle position)
    {
        var monitors = this.GetMonitors();
        if (monitors.Count == 0)
        {
            return position;
        }

        var centerX = position.Left + (position.Width / 2);
        var centerY = position.Top + (position.Height / 2);
        var isOnScreen = monitors.Any(m =>
            centerX >= m.Screen.Size.Left && centerX <= m.Screen.Size.Right &&
            centerY >= m.Screen.Size.Top && centerY <= m.Screen.Size.Bottom);

        if (isOnScreen)
        {
            return position;
        }

        // Off-screen (e.g. a disconnected monitor): re-home onto the primary.
        var primary = this.GetPrimaryMonitor() ?? monitors[0];
        var bounds = primary.Screen.Size;
        return new Rectangle(
            bounds.X + (bounds.Width / 4),
            bounds.Y + (bounds.Height / 4),
            Math.Min(position.Width, bounds.Width / 2),
            Math.Min(position.Height, bounds.Height / 2));
    }

    /// <summary>
    /// Returns the effective DPI (scale * 96) used for the saved-position rescale.
    /// A single effective scale is used for the whole desktop: webview zoom is one
    /// global value, and under the forced X11 backend GDK cannot report reliable
    /// per-monitor scales anyway.
    /// </summary>
    private int GetDpiForPosition(int x, int y)
    {
        return (int)Math.Round(this.GetEffectiveScale() * DefaultDpi);
    }

    private MonitorInfo? GetPrimaryMonitor()
    {
        var monitors = this.GetMonitors();
        if (monitors.Count == 0)
        {
            return null;
        }

        foreach (var m in monitors)
        {
            if (m.IsPrimary)
            {
                return m;
            }
        }

        return monitors[0];
    }

    private IReadOnlyList<MonitorInfo> GetMonitors()
    {
        if (this.cachedMonitors is { Count: > 0 })
        {
            return this.cachedMonitors;
        }

        var monitors = EnumerateMonitors(this.logger);
        if (monitors.Count > 0)
        {
            // Only cache a successful enumeration so an early (pre-GTK) call can retry.
            this.cachedMonitors = monitors;
        }

        return monitors;
    }

    private static IReadOnlyList<MonitorInfo> EnumerateMonitors(ILogger<ScreenManager> logger)
    {
        var scopedLogger = logger.CreateScopedLogger();

        // Ensure a default display exists in case monitors are queried before
        // Photino created its first window. gtk_init_check is idempotent.
        NativeMethods.gtk_init_check(nint.Zero, nint.Zero);

        var display = NativeMethods.gdk_display_get_default();
        if (display == nint.Zero)
        {
            scopedLogger.LogWarning("No default GdkDisplay available; cannot enumerate monitors");
            return [];
        }

        var count = NativeMethods.gdk_display_get_n_monitors(display);
        if (count <= 0)
        {
            scopedLogger.LogWarning("GDK reported {Count} monitors", count);
            return [];
        }

        var primary = NativeMethods.gdk_display_get_primary_monitor(display);
        var result = new List<MonitorInfo>(count);
        for (var i = 0; i < count; i++)
        {
            var monitor = NativeMethods.gdk_display_get_monitor(display, i);
            if (monitor == nint.Zero)
            {
                continue;
            }

            NativeMethods.gdk_monitor_get_geometry(monitor, out var geometry);
            var screen = new Screen(i, new Rectangle(geometry.X, geometry.Y, geometry.Width, geometry.Height));
            result.Add(new MonitorInfo(screen, monitor == primary && primary != nint.Zero));
        }

        scopedLogger.LogDebug("Enumerated {Count} monitor(s) via GDK", result.Count);
        return result;
    }
}
