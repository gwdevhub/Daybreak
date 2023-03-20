using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
public readonly struct UserContext
{
    public const int BaseOffset = 0x0074;

    [FieldOffset(0x0000)]
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x28)]
    public readonly string PlayerName;

    [FieldOffset(0x0124)]
    public readonly uint MapId;

    [FieldOffset(0x01B0)]
    public readonly int Language;

    [FieldOffset(0x0230)]
    public readonly uint PlayerNumber;

    // The following 3 fields have to be offset like this due to a bug in CLR when reading x32 mapped memory from x64 processes.
    [FieldOffset(0x0344)]
    public readonly char PlayerEmailFirstChar;

    [FieldOffset(0x346)]
    public readonly char PlayerEmailSecondChar;

    [FieldOffset(0x348)]
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x7E)]
    public readonly string PlayerEmailRemaining;
}
