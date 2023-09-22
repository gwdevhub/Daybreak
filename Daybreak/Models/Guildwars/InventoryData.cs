using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;

public sealed class InventoryData
{
    public Bag? Backpack { get; init; }
    public Bag? BeltPouch { get; init; }
    public List<Bag?> Bags { get; init; } = new();
    public Bag? EquipmentPack { get; init; }
    public Bag? MaterialStorage { get; init; }
    public Bag? UnclaimedItems { get; init; }
    public List<Bag?> StoragePanes { get; init; } = new();
    public Bag? EquippedItems { get; init; }
}
