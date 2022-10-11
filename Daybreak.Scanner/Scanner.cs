using Daybreak.Scanner.System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Daybreak.Scanner;

public sealed class Scanner
{
    public static void Scan(string moduleName)
    {
        var process = Process.GetProcessesByName(moduleName).First();
        var hwnd = NativeMethods.GetModuleHandle(null);
        var headersPtr = NativeMethods.ImageNtHeader(hwnd);
        var headers = Marshal.PtrToStructure<ImageNtHeaders32>(headersPtr);
    }
}
