using System.Runtime.InteropServices;

namespace Daybreak.Shared.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct ItemContext
{
    [FieldOffset(0x0024)]
    public readonly GuildwarsPointerArray<BagInfo> Bags;

    [FieldOffset(0x00B8)]
    public readonly GuildwarsPointerArray<ItemInfo> Items;

    [FieldOffset(0x00F8)]
    public readonly GuildwarsPointer<InventoryContext> Inventory;
}
