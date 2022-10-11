using System.Runtime.InteropServices;

namespace Daybreak.Scanner.System;

[StructLayout(LayoutKind.Sequential)]
public struct ImageDataDirectory
{
    public uint VirtualAddress;
    public uint Size;
}
