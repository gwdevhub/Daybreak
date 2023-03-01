using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct UserContext
{
    public const int BaseOffset = 0x0074;

    [FieldOffset(0x0000)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x28)]
    public readonly byte[] PlayerNameBytes;

    [FieldOffset(0x01B0)]
    public readonly int Language;

    [FieldOffset(0x0230)]
    public readonly uint PlayerNumber;

    [FieldOffset(0x0344)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x80)]
    public readonly byte[] PlayerEmailBytes;
}
