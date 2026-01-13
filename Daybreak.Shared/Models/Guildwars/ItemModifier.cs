namespace Daybreak.Shared.Models.Guildwars;

public readonly struct ItemModifier
{
    public uint Modifier { get; init; }
    public ItemModifierIdentifier Identifier => (ItemModifierIdentifier)(this.Modifier >> 20);
    public ItemModifierParam Param => (ItemModifierParam)((this.Modifier & 0x000F0000) >> 16);
    public uint UpgradeId => this.Modifier & 0x0000FFFF;
    public uint Argument1 => (this.Modifier & 0x0000FF00) >> 8;
    public uint Argument2 => this.Modifier & 0x000000FF;

    public static implicit operator uint(ItemModifier modifier) => modifier.Modifier;
    public static implicit operator ItemModifier(uint modifier) => new(){ Modifier = modifier };
}

public enum ItemModifierIdentifier : uint
{
    BaneSpecies                         = 0x808,
    Attribute                           = 0x279,
    Damage                              = 0xA7A,
    Armor1                              = 0xA7B,
    Armor2                              = 0xA3C,
    Energy                              = 0x67C,
    Upgrade1                            = 0x21E,
    Upgrade2                            = 0x240,
    OfTheProfession                     = 0x28A,
    DamageType                          = 0x24B,

    DamagePlusCustomized                = 0xA49,    // 120 for +20% damage (Customized) (arg2)

    DamagePlus                          = 0x223,    // +20% damage (arg2)
    DamagePlusVsHexed                   = 0x225,    // +15% damage vs hexed (arg2)
    DamagePlusEnchanted                 = 0x226,    // +15% damage while enchanted (arg2)
    DamagePlusWhileUp                   = 0x227,    // +10% damage while above 50% HP (arg2)
    DamagePlusWhileDown                 = 0x228,    // +10% damage while below 50% HP (arg2)
    DamagePlusHexed                     = 0x229,    // +15% damage while hexed (arg2)
    DamagePlusStance                    = 0x22A,    // +15% damage while in stance (arg2)

    HalvesCastingTimeGeneral            = 0x220,    // 10 for 10% chance to halve casting time for any skill (arg1)
    HalvesCastingTimeAttribute          = 0x221,    // 20 for 20% chance to halve casting time for attribute skill (arg1) (arg2)
    HalvesCastingTimeItemAttribute      = 0x280,    // 10 for 10% chance to halve casting time for attribute of equipped item (arg1)

    HalvesSkillRechargeGeneral          = 0x23A,    // 10 for 10% skill recharge for any skill (arg1)
    HalvesSkillRechargeAttribute        = 0x282,    // 20 for 20% skill recharge for attribute skill (arg1) (arg2)
    HalvesSkillRechargeItemAttribute    = 0x282,    // 10 for 10% skill recharge for attribute of equipped item (arg1)

    EnergyPlus                          = 0x22D,    // 15 for +15 energy (arg2)
    EnergyPlusEnchanted                 = 0x22F,    // 10 for +10 energy while enchanted (arg2)
    EnergyPlusHexed                     = 0x232,    // 10 for +10 energy while hexed (arg2)

    EnergyMinus                         = 0x20B,    // 5 for -5 energy (arg2)
    EnergyDegen                         = 0x20C,    // 1 for -1 energy regen (arg2)

    ArmorPlus                           = 0x210,    // +5 armor (arg2)
    ArmorPlusVsDamage                   = 0x211,    // +10 armor vs damage type attacks (arg2) (arg1)
    ArmorPlusVsDamage2                  = 0xA11,    // +10 armor vs damage type attacks (arg2) (arg1)
    ArmorPlusVsSpecies                  = 0x214,    // +10 armor vs species attacks (arg2) (arg1)
    ArmorPlusAttacking                  = 0x217,    // +5 armor while attacking (arg2)
    ArmorPlusCasting                    = 0x218,    // +5 armor while casting (arg2)
    ArmorPlusEnchanted                  = 0x219,    // +5 armor while enchanted (arg2)
    ArmorPlusHexed                      = 0x21A,    // +5 armor while hexed (arg2)
    ArmorPlusWhileDown                  = 0x21B,    // +5 armor while < 50% HP (arg2)

    ArmorMinusAttacking                 = 0x201,    // -5 armor while attacking (arg2)

    HealthPlusWhileDown                 = 0x230,    // 20 for +20 health while below 50% HP (arg2) (arg1)
    HealthMinus                         = 0x20D,    // 20 for -20 health (arg2)

    ReceiveLessDamage                   = 0x207,    // Receive 5 less damage (20%) (arg2) (arg1)
    ReceiveLessPhysDamageEnchanted      = 0x208,    // Receive 2 less physical damage while enchanted (arg2)
    ReceiveLessPhysDamageHexed          = 0x209,    // Receive 3 less physical damage while hexed (arg2)
    ReceiveLessPhysDamageStance         = 0x20A,    // Receive 2 less physical damage while in stance (arg2)

    AttributePlusOne                    = 0x241,    // +1 to attribute (20% chance) (arg1) (arg2)
    AttributePlusOneItem                = 0x283,    // +1 to attribute of equipped item (20% chance) (arg1)

    ReduceConditionDuration             = 0x285,    // Reduce condition duration by 20% (arg1)
}

// Parameter values for ItemModifier. Description only currently known.
public enum ItemModifierParam : uint
{
    LabelInName = 0x0,
    Description = 0x8
}
