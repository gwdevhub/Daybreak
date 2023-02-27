using System.Runtime.InteropServices;

namespace Daybreak.Models.Guildwars;

[StructLayout(LayoutKind.Explicit)]
public readonly struct CharContext
{
    public const int BaseOffset = 0x0074;

    [FieldOffset(0x0000)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x28)]
    public readonly byte[] PlayerNameBytes;
    
    [FieldOffset(0x01B0)]
    public readonly int Language;
    
    [FieldOffset(0x0344)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x80)]
    public readonly byte[] PlayerEmailBytes;
}
