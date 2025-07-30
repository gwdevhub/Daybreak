using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Daybreak.Services.WindowInterop
{
    public sealed class WindowInteropService : IWindowInteropService
    {
        // Win32 API constants and imports
        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int HTCAPTION = 2;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        public event EventHandler? WindowDragRequested;

        public void RequestWindowDrag()
        {
            WindowDragRequested?.Invoke(this, EventArgs.Empty);
        }

        public void RequestWindowDragWithHandle(IntPtr windowHandle)
        {
            // Release any current mouse capture
            ReleaseCapture();
            
            // Send a message to Windows that simulates dragging the title bar
            SendMessage(windowHandle, WM_NCLBUTTONDOWN, new IntPtr(HTCAPTION), IntPtr.Zero);
        }
    }
}
