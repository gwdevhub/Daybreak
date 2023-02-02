using Daybreak.Scanner.System;
using Daybreak.Scanner.Utils;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Daybreak.Scanner;

public sealed class Scanner
{
    public static void Scan(string moduleName)
    {
        var proc = Process.GetProcessesByName(moduleName).FirstOrDefault();
        var mem = new GWCAMemory(proc);
        GWMemory.FindAddressesIfNeeded(mem);
    }
}
