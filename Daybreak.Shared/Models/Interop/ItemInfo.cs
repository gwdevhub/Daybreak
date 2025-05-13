using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct ItemInfo
{
    [FieldOffset(0x0000)]
    public readonly uint Id;

    [FieldOffset(0x0004)]
    public readonly uint EntityId;

    [FieldOffset(0x000C)]
    public readonly uint ContainingBagAddress; // Used only to determine which bag the item belongs to

    [FieldOffset(0x0010)]
    public readonly uint ModifierArrayAddress;

    [FieldOffset(0x0014)]
    public readonly uint ModifierCount;

    [FieldOffset(0x0018)]
    public readonly GuildwarsPointer<string> CustomizedPtr;

    [FieldOffset(0x0020)]
    public readonly byte Type;

    [FieldOffset(0x0024)]
    public readonly uint Value;

    [FieldOffset(0x002C)]
    public readonly uint ModelId;

    [FieldOffset(0x0030)]
    public readonly GuildwarsPointer<string> InfoPtr;

    [FieldOffset(0x003C)]
    public readonly GuildwarsPointer<string> ItemNamePtr;

    [FieldOffset(0x004C)]
    public readonly ushort Quantity;

    [FieldOffset(0x0050)]
    public readonly byte Slot;
}
