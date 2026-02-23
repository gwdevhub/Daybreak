using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

public enum ItemType : byte
{
    Salvage,
    Axe = 2,
    Bag,
    Boots,
    Bow,
    Bundle,
    Chestpiece,
    Rune_Mod,
    Usable,
    Dye,
    Materials_Zcoins,
    Offhand,
    Gloves,
    Hammer = 15,
    Headpiece,
    CC_Shards,
    Key,
    Leggings,
    Gold_Coin,
    Quest_Item,
    Wand,
    Shield = 24,
    Staff = 26,
    Sword,
    Kit = 29,
    Trophy,
    Scroll,
    Daggers,
    Present,
    Minipet,
    Scythe,
    Spear,
    Storybook = 43,
    Costume,
    Costume_Headpiece,
    Unknown = 0xff
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[GWCAEquivalent("ItemModifier")]
public readonly struct ItemModifier(uint mod)
{
    public static readonly ItemModifier Zero = new();

    public readonly uint Mod = mod;

    public uint Identifier => this.Mod >> 16;
    public uint Arg1 => (this.Mod & 0x0000FF00) >> 8;
    public uint Arg2 => this.Mod & 0x000000FF;
    public uint Arg => this.Mod & 0x0000FFFF;
}

[StructLayout(LayoutKind.Explicit, Pack = 1)]
[GWCAEquivalent("Item")]
public readonly unsafe struct Item
{
    [FieldOffset(0x0000)]
    public readonly uint ItemId;

    [FieldOffset(0x0004)]
    public readonly uint AgentId;

    [FieldOffset(0x0008)]
    public readonly Bag* BagEquipped;

    [FieldOffset(0x000C)]
    public readonly Bag* Bag;

    [FieldOffset(0x0010)]
    public readonly ItemModifier* Modifiers;

    [FieldOffset(0x0014)]
    public readonly uint ModifiersCount;

    [FieldOffset(0x001C)]
    public readonly uint ModelFileId;

    [FieldOffset(0x0020)]
    public readonly ItemType Type;

    [FieldOffset(0x0024)]
    public readonly ushort Value;

    [FieldOffset(0x0028)]
    public readonly uint Interaction;

    [FieldOffset(0x002C)]
    public readonly uint ModelId;

    [FieldOffset(0x0030)]
    public readonly char* InfoString;

    [FieldOffset(0x0034)]
    public readonly char* NameEncoded;

    [FieldOffset(0x0038)]
    public readonly char* CompleteNameEncoded;

    [FieldOffset(0x003C)]
    public readonly char* SingleItemName;

    [FieldOffset(0x0048)]
    public readonly ushort ItemFormula;

    [FieldOffset(0x004A)]
    public readonly byte IsMaterialSalvageable;

    [FieldOffset(0x004C)]
    public readonly ushort Quantity;

    [FieldOffset(0x004E)]
    public readonly byte Equipped;

    [FieldOffset(0x004F)]
    public readonly byte Profession;

    [FieldOffset(0x0050)]
    public readonly uint Slot;

    public readonly bool Stackable => (this.Interaction & 0x80000) != 0;

    public readonly bool Inscribable => (this.Interaction & 0x08000000) != 0;
}
