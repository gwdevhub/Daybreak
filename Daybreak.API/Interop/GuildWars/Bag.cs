using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

public enum BagType
{
    None,
    Inventory,
    Equipped,
    NotCollected,
    Storage,
    MaterialsStorage
}

[StructLayout(LayoutKind.Explicit, Pack = 1)]
[GWCAEquivalent("Bag")]
public readonly unsafe struct Bag
{
    [FieldOffset(0x0000)]
    public readonly BagType Type;
    [FieldOffset(0x0004)]
    public readonly uint Index;
    [FieldOffset(0x000C)]
    public readonly uint ContainerItem;
    [FieldOffset(0x0010)]
    public readonly uint ItemsCount;
    [FieldOffset(0x0014)]
    public readonly Bag* Bags;
    [FieldOffset(0x0018)]
    public readonly GuildWarsArray<WrappedPointer<Item>> Items;
}
