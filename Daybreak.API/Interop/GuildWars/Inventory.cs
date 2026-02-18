using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[GWCAEquivalent("Inventory")]
public readonly unsafe struct Inventory
{
    public readonly Bag* UnusedBag;
    public readonly Bag* Backpack;
    public readonly Bag* BeltPouch;
    public readonly Bag* Bag1;
    public readonly Bag* Bag2;
    public readonly Bag* EquipmentPack;
    public readonly Bag* MaterialStorage;
    public readonly Bag* UnclaimedItems;
    public readonly Bag* Storage1;
    public readonly Bag* Storage2;
    public readonly Bag* Storage3;
    public readonly Bag* Storage4;
    public readonly Bag* Storage5;
    public readonly Bag* Storage6;
    public readonly Bag* Storage7;
    public readonly Bag* Storage8;
    public readonly Bag* Storage9;
    public readonly Bag* Storage10;
    public readonly Bag* Storage11;
    public readonly Bag* Storage12;
    public readonly Bag* Storage13;
    public readonly Bag* Storage14;
    public readonly Bag* EquippedItems;
    public readonly Item* Bundle;
    public readonly uint StoragePanesUnlocked;

    public readonly Bag*[] Bags => [
        this.Backpack,
        this.BeltPouch,
        this.Bag1,
        this.Bag2,
        this.EquipmentPack,
        this.MaterialStorage,
        this.UnclaimedItems,
        this.Storage1,
        this.Storage2,
        this.Storage3,
        this.Storage4,
        this.Storage5,
        this.Storage6,
        this.Storage7,
        this.Storage8,
        this.Storage9,
        this.Storage10,
        this.Storage11,
        this.Storage12,
        this.Storage13,
        this.Storage14,
        this.EquippedItems
    ];
}
