using Daybreak.Shared.Services.Window;
using Daybreak.Shared.Utils;
using Daybreak.Windows.Utils;
using Photino.Blazor;
using System.Core.Extensions;
using System.Runtime.Versioning;

namespace Daybreak.Windows.Services.Window;

[SupportedOSPlatform("windows")]
internal sealed class WindowManipulationService : IWindowManipulationService
{
    private readonly PhotinoBlazorApp photinoApp;

    public WindowManipulationService(PhotinoBlazorApp photinoApp)
    {
        this.photinoApp = photinoApp.ThrowIfNull();
    }

    public void DragWindow()
    {
        var windowHandle = this.photinoApp.MainWindow.WindowHandle;
        if (windowHandle == IntPtr.Zero)
        {
            return;
        }

        NativeMethods.ReleaseCapture();
        NativeMethods.PostMessage(
            windowHandle,
            NativeMethods.WM_SYSCOMMAND,
            (IntPtr)(NativeMethods.SC_MOVE | NativeMethods.HTCAPTION),
            IntPtr.Zero
        );
    }

    public void ResizeWindow(ResizeDirection direction)
    {
        var windowHandle = this.photinoApp.MainWindow.WindowHandle;
        if (windowHandle == IntPtr.Zero)
        {
            return;
        }

        NativeMethods.ReleaseCapture();
        NativeMethods.PostMessage(
            windowHandle,
            NativeMethods.WM_NCLBUTTONDOWN,
            (IntPtr)(int)direction,
            IntPtr.Zero
        );
    }
}
