namespace Daybreak.Shared.Models.Guildwars;

public readonly struct ItemModifier
{
    public uint Modifier { get; init; }
    public uint Flags => (this.Modifier >> 30) & 0xC;
    public ItemModifierIdentifier Identifier => (ItemModifierIdentifier)((this.Modifier >> 20) & 0x3FF);
    public ItemModifierParam Param => (ItemModifierParam)((this.Modifier >> 16) & 0xF);
    public uint UpgradeId => this.Modifier & 0xFFFF;
    public uint Argument1 => (this.Modifier >> 8) & 0xFF;
    public uint Argument2 => this.Modifier & 0xFF;

    public static implicit operator uint(ItemModifier modifier) => modifier.Modifier;
    public static implicit operator ItemModifier(uint modifier) => new(){ Modifier = modifier };
}

public enum ItemModifierIdentifier : uint
{
    BaneSpecies                         = 0x008,    // Dmg increased vs species (arg1)
    Attribute                           = 0x279,    // Requires points in attribute (arg2) (arg1)
    Damage                              = 0x27A,    // Min damage - Max damage (arg2) (arg1)
    Armor1                              = 0x27B,    // Armor value (arg1)
    Armor2                              = 0x23C,    // Armor value (arg1)
    Energy                              = 0x27C,    // Energy value (arg1)
    Upgrade1                            = 0x21E,    // Suffix/Prefix/Inscription upgrade (upgrade id)
    Upgrade2                            = 0x240,    // Suffix/Prefix/Inscription upgrade (upgrade id)
    OfTheProfession                     = 0x28A,    // +5 to primary of profession (arg2) (arg1)
    DamageType                          = 0x24B,    // Damage type (arg1)

    IncreasedSaleValue                  = 0x25F,    // Increased sale value (arg1)
    HighlySalvageable                   = 0x260,    // Highly salvageable (arg1)

    DamagePlusCustomized                = 0x249,    // 120 for +20% damage (Customized) (arg2)

    DamagePlus                          = 0x223,    // +20% damage (arg2)
    DamagePlusVsHexed                   = 0x225,    // +15% damage vs hexed (arg2)
    DamagePlusEnchanted                 = 0x226,    // +15% damage while enchanted (arg2)
    DamagePlusWhileUp                   = 0x227,    // +10% damage while above 50% HP (arg2)
    DamagePlusWhileDown                 = 0x228,    // +10% damage while below 50% HP (arg2)
    DamagePlusHexed                     = 0x229,    // +15% damage while hexed (arg2)
    DamagePlusStance                    = 0x22A,    // +15% damage while in stance (arg2)
    DamagePlusVsSpecies                 = 0x224,    // +20% damage vs species (arg1) (species given by BaneSpecies property)

    HalvesCastingTimeGeneral            = 0x220,    // 10% chance to halve casting time for any skill (arg1)
    HalvesCastingTimeAttribute          = 0x221,    // 20% chance to halve casting time for attribute skill (arg1) (arg2)
    HalvesCastingTimeItemAttribute      = 0x280,    // 10% chance to halve casting time for attribute of equipped item (arg1)

    HalvesSkillRechargeGeneral          = 0x23A,    // 10% skill recharge for any skill (arg1)
    HalvesSkillRechargeAttribute        = 0x239,    // 20% skill recharge for attribute skill (arg1) (arg2)
    HalvesSkillRechargeItemAttribute    = 0x282,    // 10% skill recharge for attribute of equipped item (arg1)

    EnergyPlus                          = 0x22D,    // +15 energy (arg2)
    EnergyPlusEnchanted                 = 0x22F,    // +10 energy while enchanted (arg2)
    EnergyPlusHexed                     = 0x232,    // +10 energy while hexed (arg2)
    EnergyPlusWhileDown                 = 0x231,    // +10 energy while below 50% HP (arg2) (arg1)

    EnergyMinus                         = 0x20B,    // -5 energy (arg2)
    EnergyDegen                         = 0x20C,    // -1 energy regen (arg2)
    EnergyRegen                         = 0x262,    // 1 energy regen (arg2)
    EnergyGainOnHit                     = 0x251,    // Gain 1 energy on hit (arg2)

    ArmorPlus                           = 0x210,    // +5 armor (arg2)
    ArmorPlusVsDamage                   = 0x211,    // +10 armor vs damage type attacks (arg2) (arg1)
    ArmorPlusVsPhysical                 = 0x215,    // +10 armor vs physical attacks (arg2)
    ArmorPlusVsPhysical2                = 0x216,    // +10 armor vs physical attacks (arg2)
    ArmorPlusVsElemental                = 0x212,    // +7 armor vs elemental attacks (arg2)
    ArmorPlusVsSpecies                  = 0x214,    // +10 armor vs species attacks (arg2) (arg1)
    ArmorPlusAttacking                  = 0x217,    // +5 armor while attacking (arg2)
    ArmorPlusCasting                    = 0x218,    // +5 armor while casting (arg2)
    ArmorPlusEnchanted                  = 0x219,    // +5 armor while enchanted (arg2)
    ArmorPlusHexed                      = 0x21A,    // +5 armor while hexed (arg2)
    ArmorPlusWhileDown                  = 0x21B,    // +5 armor while < 50% HP (arg2)

    ArmorMinusAttacking                 = 0x201,    // -5 armor while attacking (arg2)

    ArmorPenetration                    = 0x23F,    // 20% armor penetration (20% chance) (arg2) (arg1)

    HealthPlus                          = 0x289,    // +60 hp (arg2)
    HealthPlus2                         = 0x234,    // +30 health (arg1)
    HealthPlusWhileDown                 = 0x230,    // +20 health while below 50% HP (arg2) (arg1)
    HealthPlusHexed                     = 0x237,    // +60 health while hexed (arg1)
    HealthPlusStance                    = 0x238,    // +60 health while in stance (arg1)
    HealthMinus                         = 0x20D,    // -20 health (arg2)
    HealthDegen                         = 0x20E,    // -1 health regen (arg2)
    HealthStealOnHit                    = 0x252,    // Steal 5 health on hit (arg1)

    ReceiveLessDamage                   = 0x207,    // Receive 5 less damage (20%) (arg2) (arg1)
    ReceiveLessPhysDamageEnchanted      = 0x208,    // Receive 2 less physical damage while enchanted (arg2)
    ReceiveLessPhysDamageHexed          = 0x209,    // Receive 3 less physical damage while hexed (arg2)
    ReceiveLessPhysDamageStance         = 0x20A,    // Receive 2 less physical damage while in stance (arg2)

    AttributePlusOne                    = 0x241,    // +1 to attribute (20% chance) (arg1) (arg2)
    AttributePlusOneItem                = 0x283,    // +1 to attribute of equipped item (20% chance) (arg1)

    ReduceConditionDuration             = 0x285,    // Reduce condition duration by 20% (arg1)
    ReduceConditionTupleDuration        = 0x277,    // Reduce condition1 and condition2 duration by 20% (arg1) (arg2)

    IncreaseEnchantmentDuration         = 0x22B,    // Increase enchantment duration by 20% (arg2)
    IncreaseConditionDuration           = 0x246,    // Increase condition duration by 33% (upgradeId)
}

// Parameter values for ItemModifier. Description only currently known.
public enum ItemModifierParam : uint
{
    LabelInName = 0x0,
    Description = 0x8
}
