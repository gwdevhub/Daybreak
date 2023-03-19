using System;
using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct PathingMap
{
    [FieldOffset(0x0000)]
    public readonly uint ZPlane;

    [FieldOffset(0x0018)]
    public readonly int TrapezoidArray;

    [FieldOffset(0x001C)]
    public readonly uint TrapezoidCount;

    [FieldOffset(0x0020)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xD)]
    private readonly uint[] H0020;
}
