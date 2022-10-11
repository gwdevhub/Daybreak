using System.Runtime.InteropServices;

namespace Daybreak.Scanner;

internal static class NativeMethods
{
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern IntPtr GetModuleHandle([MarshalAs(UnmanagedType.LPWStr)] string lpModuleName);
    [DllImport("dbghelp.dll", SetLastError = true)]
    public static extern IntPtr ImageNtHeader(IntPtr ImageBase);
}
