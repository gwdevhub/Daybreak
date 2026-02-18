using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
[GWCAEquivalent("ItemContext")]
public readonly struct ItemContext
{
    [FieldOffset(0x0024)]
    public readonly GuildWarsArray<WrappedPointer<Bag>> Bags;

    [FieldOffset(0x00B8)]
    public readonly GuildWarsArray<WrappedPointer<Item>> Items;

    [FieldOffset(0x00F8)]
    public readonly WrappedPointer<Inventory> Inventory;
}
