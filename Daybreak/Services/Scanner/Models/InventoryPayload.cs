using System.Collections.Generic;

namespace Daybreak.Services.Scanner.Models;

internal class InventoryPayload
{
    public uint GoldInStorage { get; set; }
    public uint GoldOnCharacter { get; set; }
    public BagPayload? Backpack { get; set; }
    public BagPayload? BeltPouch { get; set; }
    public BagPayload? EquipmentPack { get; set; }
    public BagPayload? MaterialStorage { get; set; }
    public BagPayload? UnclaimedItems { get; set; }
    public BagPayload? EquippedItems { get; set; }
    public List<BagPayload>? Bags { get; set; }
    public List<BagPayload>? StoragePanes { get; set; }
}
