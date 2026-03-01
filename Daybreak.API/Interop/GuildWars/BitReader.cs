using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x1C)]
public readonly struct BitReader
{
    [FieldOffset(0x0000)]
    public readonly int HasError;
    [FieldOffset(0x0004)]
    public readonly int RackBitCount;
    [FieldOffset(0x0008)]
    public readonly long RackData;
    [FieldOffset(0x0010)]
    public readonly nint CurrentPtr;
    [FieldOffset(0x0014)]
    public readonly nint StartPtr;
    [FieldOffset(0x0018)]
    public readonly nint EndPtr;
}