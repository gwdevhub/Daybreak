using Daybreak.Shared.Services.Window;
using Daybreak.Shared.Utils;
using Daybreak.Windows.Utils;
using System.Runtime.Versioning;

namespace Daybreak.Windows.Services.Window;

[SupportedOSPlatform("windows")]
internal sealed class WindowManipulationService : IWindowManipulationService
{
    public void DragWindow(nint windowHandle)
    {
        NativeMethods.ReleaseCapture();
        NativeMethods.PostMessage(
            windowHandle,
            NativeMethods.WM_SYSCOMMAND,
            (IntPtr)(NativeMethods.SC_MOVE | NativeMethods.HTCAPTION),
            IntPtr.Zero
        );
    }

    public void ResizeWindow(nint windowHandle, ResizeDirection direction)
    {
        NativeMethods.ReleaseCapture();
        NativeMethods.PostMessage(
            windowHandle,
            NativeMethods.WM_NCLBUTTONDOWN,
            (IntPtr)(int)direction,
            IntPtr.Zero
        );
    }
}
