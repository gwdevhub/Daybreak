using System.Runtime.InteropServices;

namespace Daybreak.Shared.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct BagInfo
{
    [FieldOffset(0x0000)]
    public readonly uint Type;

    [FieldOffset(0x0004)]
    public readonly uint Index;

    [FieldOffset(0x0008)]
    public readonly uint Id;

    [FieldOffset(0x000C)]
    public readonly uint Container;

    [FieldOffset(0x0010)]
    public readonly uint ItemsCount;

    [FieldOffset(0x0018)]
    public readonly GuildwarsPointerArray<ItemInfo> Items;
}
