using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct InventoryContext
{
    [FieldOffset(0x0004)]
    public readonly GuildwarsPointer<BagInfo> Backpack;

    [FieldOffset(0x0008)]
    public readonly GuildwarsPointer<BagInfo> BeltPouch;

    [FieldOffset(0x000C)]
    public readonly GuildwarsPointer<BagInfo> Bag1;

    [FieldOffset(0x0010)]
    public readonly GuildwarsPointer<BagInfo> Bag2;

    [FieldOffset(0x0014)]
    public readonly GuildwarsPointer<BagInfo> EquipmentPack;

    [FieldOffset(0x0018)]
    public readonly GuildwarsPointer<BagInfo> MaterialStorage;

    [FieldOffset(0x001C)]
    public readonly GuildwarsPointer<BagInfo> UnclaimedItems;

    [FieldOffset(0x0020)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
    public readonly GuildwarsPointer<BagInfo>[] StoragePanes;

    [FieldOffset(0x0058)]
    public readonly GuildwarsPointer<BagInfo> EquippedItems;
}
