using System;
using System.Runtime.InteropServices;

namespace Pepa.Wpf.Utilities
{
    static class NativeMethods
    {
        public const int WM_SYSCOMMAND = 0x112;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    }
}
