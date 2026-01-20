using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Guildwars;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "propertyType")]
[JsonDerivedType(typeof(BaneProperty), "Bane")]
[JsonDerivedType(typeof(RequirementProperty), "Requirement")]
[JsonDerivedType(typeof(DamageProperty), "Damage")]
[JsonDerivedType(typeof(ArmorProperty), "Armor")]
[JsonDerivedType(typeof(EnergyProperty), "Energy")]
[JsonDerivedType(typeof(SuffixProperty), "Suffix")]
[JsonDerivedType(typeof(PrefixProperty), "Prefix")]
[JsonDerivedType(typeof(InscriptionProperty), "Inscription")]
[JsonDerivedType(typeof(UpgradesRuneProperty), "UpgradesRune")]
[JsonDerivedType(typeof(AppliesToRuneProperty), "AppliesToRune")]
[JsonDerivedType(typeof(OfTheProfessionProperty), "OfTheProfession")]
[JsonDerivedType(typeof(IncreasedSaleValueProperty), "IncreasedSaleValue")]
[JsonDerivedType(typeof(HighlySalvageableProperty), "HighlySalvageable")]
[JsonDerivedType(typeof(CustomizedProperty), "Customized")]
[JsonDerivedType(typeof(DamageTypeProperty), "DamageType")]
[JsonDerivedType(typeof(DamagePlusProperty), "DamagePlus")]
[JsonDerivedType(typeof(DamagePlusVsHexedProperty), "DamagePlusVsHexed")]
[JsonDerivedType(typeof(DamagePlusWhileEnchantedProperty), "DamagePlusWhileEnchanted")]
[JsonDerivedType(typeof(DamagePlusWhileUpProperty), "DamagePlusWhileUp")]
[JsonDerivedType(typeof(DamagePlusWhileDownProperty), "DamagePlusWhileDown")]
[JsonDerivedType(typeof(DamagePlusWhileHexedProperty), "DamagePlusWhileHexed")]
[JsonDerivedType(typeof(DamagePlusWhileInStanceProperty), "DamagePlusWhileInStance")]
[JsonDerivedType(typeof(DamagePlusVsSpeciesProperty), "DamagePlusVsSpecies")]
[JsonDerivedType(typeof(HalvesCastingTimeGeneralProperty), "HalvesCastingTimeGeneral")]
[JsonDerivedType(typeof(HalvesCastingTimeAttributeProperty), "HalvesCastingTimeAttribute")]
[JsonDerivedType(typeof(HalvesCastingTimeItemAttributeProperty), "HalvesCastingTimeItemAttribute")]
[JsonDerivedType(typeof(HalvesSkillRechargeGeneralProperty), "HalvesSkillRechargeGeneral")]
[JsonDerivedType(typeof(HalvesSkillRechargeAttributeProperty), "HalvesSkillRechargeAttribute")]
[JsonDerivedType(typeof(HalvesSkillRechargeItemAttributeProperty), "HalvesSkillRechargeItemAttribute")]
[JsonDerivedType(typeof(EnergyPlusProperty), "EnergyPlus")]
[JsonDerivedType(typeof(EnergyPlusWhileEnchantedProperty), "EnergyPlusWhileEnchanted")]
[JsonDerivedType(typeof(EnergyPlusWhileHexedProperty), "EnergyPlusWhileHexed")]
[JsonDerivedType(typeof(EnergyPlusWhileDownProperty), "EnergyPlusWhileDown")]
[JsonDerivedType(typeof(EnergyMinusProperty), "EnergyMinus")]
[JsonDerivedType(typeof(EnergyDegenProperty), "EnergyDegen")]
[JsonDerivedType(typeof(EnergyRegenProperty), "EnergyRegen")]
[JsonDerivedType(typeof(EnergyGainOnHitProperty), "EnergyGainOnHit")]
[JsonDerivedType(typeof(ArmorPlusProperty), "ArmorPlus")]
[JsonDerivedType(typeof(ArmorPlusVsDamageProperty), "ArmorPlusVsDamage")]
[JsonDerivedType(typeof(ArmorPlusVsPhysical), "ArmorPlusVsPhysical")]
[JsonDerivedType(typeof(ArmorPlusVsElemental), "ArmorPlusVsElemental")]
[JsonDerivedType(typeof(ArmorPlusVsSpeciesProperty), "ArmorPlusVsSpecies")]
[JsonDerivedType(typeof(ArmorPlusWhileAttackingProperty), "ArmorPlusWhileAttacking")]
[JsonDerivedType(typeof(ArmorPlusWhileCastingProperty), "ArmorPlusWhileCasting")]
[JsonDerivedType(typeof(ArmorPlusWhileEnchantedProperty), "ArmorPlusWhileEnchanted")]
[JsonDerivedType(typeof(ArmorPlusWhileHexedProperty), "ArmorPlusWhileHexed")]
[JsonDerivedType(typeof(ArmorPlusWhileDownProperty), "ArmorPlusWhileDown")]
[JsonDerivedType(typeof(ArmorMinusWhileAttackingProperty), "ArmorMinusWhileAttacking")]
[JsonDerivedType(typeof(ArmorPenetrationProperty), "ArmorPenetration")]
[JsonDerivedType(typeof(HealthPlusProperty), "HealthPlus")]
[JsonDerivedType(typeof(HealthPlusWhileHexedProperty), "HealthPlusWhileHexed")]
[JsonDerivedType(typeof(HealthPlusWhileDownProperty), "HealthPlusWhileDown")]
[JsonDerivedType(typeof(HealthPlusStance), "HealthPlusStance")]
[JsonDerivedType(typeof(HealthMinusProperty), "HealthMinus")]
[JsonDerivedType(typeof(HealthDegenProperty), "HealthDegen")]
[JsonDerivedType(typeof(HealthStealOnHitProperty), "HealthStealOnHit")]
[JsonDerivedType(typeof(ReceiveLessDamageProperty), "ReceiveLessDamage")]
[JsonDerivedType(typeof(ReceiveLessPhysDamageWhileEnchantedProperty), "ReceiveLessPhysDamageWhileEnchanted")]
[JsonDerivedType(typeof(ReceiveLessPhysDamageWhileHexedProperty), "ReceiveLessPhysDamageWhileHexed")]
[JsonDerivedType(typeof(ReceiveLessPhysDamageWhileStanceProperty), "ReceiveLessPhysDamageWhileStance")]
[JsonDerivedType(typeof(AttributePlusOneProperty), "AttributePlusOne")]
[JsonDerivedType(typeof(AttributePlusOneItemProperty), "AttributePlusOneItem")]
[JsonDerivedType(typeof(ReduceConditionDurationProperty), "ReduceConditionDuration")]
[JsonDerivedType(typeof(ReduceConditionTupleDurationProperty), "ReduceConditionTupleDuration")]
[JsonDerivedType(typeof(IncreaseEnchantmentDurationProperty), "IncreaseEnchantmentDuration")]
[JsonDerivedType(typeof(IncreaseConditionDurationProperty), "IncreaseConditionDuration")]
[JsonDerivedType(typeof(UnknownUpgradeProperty), "Unknown")]
public abstract class ItemProperty
{
    public static ImmutableArray<ItemProperty> ParseItemModifiers(ImmutableArray<ItemModifier> modifiers)
    {
        return [.. modifiers.Select(
            m => m.Identifier switch
            {
                ItemModifierIdentifier.BaneSpecies                                  => new BaneProperty { BaneSpecies = (ItemBaneSpecies)m.Argument1 },
                ItemModifierIdentifier.Attribute                                    => new RequirementProperty { Attribute = ParseAttributeName(m.Argument1), Requirement = (int)m.Argument2 },
                ItemModifierIdentifier.Damage                                       => new DamageProperty { MinDamage = (int)m.Argument2, MaxDamage = (int)m.Argument1 },
                ItemModifierIdentifier.Armor1                                       => new ArmorProperty { Armor =(int)m.Argument1 },
                ItemModifierIdentifier.Armor2                                       => new ArmorProperty { Armor = (int)m.Argument1 },
                ItemModifierIdentifier.Energy                                       => new EnergyProperty { Energy = (int)m.Argument1 },
                ItemModifierIdentifier.Upgrade1                                     => ParseUpgradeProperty(m),
                ItemModifierIdentifier.Upgrade2                                     => ParseUpgradeProperty(m),
                ItemModifierIdentifier.OfTheProfession                              => ParseOfTheProfessionProperty(m),
                ItemModifierIdentifier.DamageType                                   => new DamageTypeProperty { DamageType = (DamageType)m.Argument1 },

                ItemModifierIdentifier.IncreasedSaleValue                           => new IncreasedSaleValueProperty { SaleValueBonus = (int)m.Argument1 },
                ItemModifierIdentifier.HighlySalvageable                            => new HighlySalvageableProperty { SalvageBonus = (int)m.Argument1 },

                ItemModifierIdentifier.DamagePlusCustomized when m.Argument1 is 120 => new CustomizedProperty(),

                ItemModifierIdentifier.DamagePlus                                   => new DamagePlusProperty { Percentage = (int)m.Argument2 },
                ItemModifierIdentifier.DamagePlusVsHexed                            => new DamagePlusVsHexedProperty { Percentage = (int)m.Argument2 },
                ItemModifierIdentifier.DamagePlusEnchanted                          => new DamagePlusWhileEnchantedProperty { Percentage = (int)m.Argument2 },
                ItemModifierIdentifier.DamagePlusWhileUp                            => new DamagePlusWhileUpProperty { Percentage = (int)m.Argument2 },
                ItemModifierIdentifier.DamagePlusWhileDown                          => new DamagePlusWhileDownProperty { Percentage = (int)m.Argument2 },
                ItemModifierIdentifier.DamagePlusHexed                              => new DamagePlusWhileHexedProperty { Percentage = (int)m.Argument2 },
                ItemModifierIdentifier.DamagePlusStance                             => new DamagePlusWhileInStanceProperty { Percentage = (int)m.Argument2 },
                ItemModifierIdentifier.DamagePlusVsSpecies                          => new DamagePlusVsSpeciesProperty { Percentage = (int)m.Argument1 },

                ItemModifierIdentifier.HalvesCastingTimeGeneral                     => new HalvesCastingTimeGeneralProperty { Chance = (int)m.Argument1 },
                ItemModifierIdentifier.HalvesCastingTimeAttribute                   => new HalvesCastingTimeAttributeProperty { Chance = (int)m.Argument1, Attribute = ParseAttributeName(m.Argument2) },
                ItemModifierIdentifier.HalvesCastingTimeItemAttribute               => new HalvesCastingTimeItemAttributeProperty { Chance = (int)m.Argument1 },

                ItemModifierIdentifier.HalvesSkillRechargeGeneral                   => new HalvesSkillRechargeGeneralProperty { Chance = (int)m.Argument1 },
                ItemModifierIdentifier.HalvesSkillRechargeAttribute                 => new HalvesSkillRechargeAttributeProperty { Chance = (int)m.Argument1, Attribute = ParseAttributeName(m.Argument2) },
                ItemModifierIdentifier.HalvesSkillRechargeItemAttribute             => new HalvesSkillRechargeItemAttributeProperty { Chance = (int)m.Argument1 },

                ItemModifierIdentifier.EnergyPlus                                   => new EnergyPlusProperty { Energy = (int)m.Argument2 },
                ItemModifierIdentifier.EnergyPlusEnchanted                          => new EnergyPlusWhileEnchantedProperty { Energy = (int)m.Argument2 },
                ItemModifierIdentifier.EnergyPlusHexed                              => new EnergyPlusWhileHexedProperty { Energy = (int)m.Argument2 },
                ItemModifierIdentifier.EnergyPlusWhileDown                          => new EnergyPlusWhileDownProperty { Energy = (int)m.Argument2, HealthThreshold = (int)m.Argument1 },

                ItemModifierIdentifier.EnergyMinus                                  => new EnergyMinusProperty { Energy = (int)m.Argument2 },
                ItemModifierIdentifier.EnergyDegen                                  => new EnergyDegenProperty { EnergyDegen = (int)m.Argument2 },
                ItemModifierIdentifier.EnergyRegen                                  => new EnergyRegenProperty { EnergyRegen = (int)m.Argument2 },
                ItemModifierIdentifier.EnergyGainOnHit                              => new EnergyGainOnHitProperty { EnergyGain = (int)m.Argument2 },

                ItemModifierIdentifier.ArmorPlus                                    => new ArmorPlusProperty { Armor = (int)m.Argument2 },
                ItemModifierIdentifier.ArmorPlusVsDamage                            => new ArmorPlusVsDamageProperty { Armor = (int)m.Argument2, DamageType = (DamageType)m.Argument1 },
                ItemModifierIdentifier.ArmorPlusVsPhysical                          => new ArmorPlusVsPhysical { Armor = (int)m.Argument2 },
                ItemModifierIdentifier.ArmorPlusVsPhysical2                         => new ArmorPlusVsPhysical { Armor = (int)m.Argument2 },
                ItemModifierIdentifier.ArmorPlusVsElemental                         => new ArmorPlusVsElemental { Armor = (int)m.Argument2 },
                ItemModifierIdentifier.ArmorPlusVsSpecies                           => new ArmorPlusVsSpeciesProperty { Armor = (int)m.Argument2, Species = (ItemBaneSpecies)m.Argument1 },
                ItemModifierIdentifier.ArmorPlusAttacking                           => new ArmorPlusWhileAttackingProperty { Armor = (int)m.Argument2 },
                ItemModifierIdentifier.ArmorPlusCasting                             => new ArmorPlusWhileCastingProperty { Armor = (int)m.Argument2 },
                ItemModifierIdentifier.ArmorPlusEnchanted                           => new ArmorPlusWhileEnchantedProperty { Armor = (int)m.Argument2 },
                ItemModifierIdentifier.ArmorPlusHexed                               => new ArmorPlusWhileHexedProperty { Armor = (int)m.Argument2 },
                ItemModifierIdentifier.ArmorPlusWhileDown                           => new ArmorPlusWhileDownProperty { Armor = (int)m.Argument2 },

                ItemModifierIdentifier.ArmorMinusAttacking                          => new ArmorMinusWhileAttackingProperty { Armor = (int)m.Argument2 },

                ItemModifierIdentifier.ArmorPenetration                             => new ArmorPenetrationProperty { ArmorPenetration = (int)m.Argument2, Chance = (int)m.Argument1 },

                ItemModifierIdentifier.HealthPlus                                   => new HealthPlusProperty { Health = (int)m.Argument2 },
                ItemModifierIdentifier.HealthPlus2                                  => new HealthPlusProperty { Health = (int)m.Argument1 },
                ItemModifierIdentifier.HealthPlusHexed                              => new HealthPlusWhileHexedProperty { Health = (int)m.Argument1 },
                ItemModifierIdentifier.HealthPlusWhileDown                          => new HealthPlusWhileDownProperty { Health = (int)m.Argument2, HealthThreshold = (int)m.Argument1 },
                ItemModifierIdentifier.HealthPlusStance                             => new HealthPlusStance { Health = (int)m.Argument1 },
                ItemModifierIdentifier.HealthMinus                                  => new HealthMinusProperty { Health = (int)m.Argument2 },
                ItemModifierIdentifier.HealthDegen                                  => new HealthDegenProperty { HealthDegen = (int)m.Argument2 },
                ItemModifierIdentifier.HealthStealOnHit                             => new HealthStealOnHitProperty { HealthSteal = (int)m.Argument1 },

                ItemModifierIdentifier.ReceiveLessDamage                            => new ReceiveLessDamageProperty { LessDamage = (int)m.Argument2, Chance = (int)m.Argument1 },
                ItemModifierIdentifier.ReceiveLessPhysDamageEnchanted               => new ReceiveLessPhysDamageWhileEnchantedProperty { LessDamage = (int)m.Argument2 },
                ItemModifierIdentifier.ReceiveLessPhysDamageHexed                   => new ReceiveLessPhysDamageWhileHexedProperty { LessDamage = (int)m.Argument2 },
                ItemModifierIdentifier.ReceiveLessPhysDamageStance                  => new ReceiveLessPhysDamageWhileStanceProperty { LessDamage = (int)m.Argument2 },

                ItemModifierIdentifier.AttributePlusOne                             => new AttributePlusOneProperty { Attribute = ParseAttributeName(m.Argument1), Chance = (int)m.Argument2 },
                ItemModifierIdentifier.AttributePlusOneItem                         => new AttributePlusOneItemProperty { Chance = (int)m.Argument1 },

                ItemModifierIdentifier.ReduceConditionDuration                      => new ReduceConditionDurationProperty { Condition = (ConditionType)m.Argument1 },
                ItemModifierIdentifier.ReduceConditionTupleDuration                 => new ReduceConditionTupleDurationProperty { Condition1 = (ConditionType)m.Argument1, Condition2 = (ConditionType)m.Argument2 },

                ItemModifierIdentifier.IncreaseEnchantmentDuration                  => new IncreaseEnchantmentDurationProperty { Percentage = (int)m.Argument2 },
                ItemModifierIdentifier.IncreaseConditionDuration                    => new IncreaseConditionDurationProperty { Condition = (SkillIdToCondition)m.UpgradeId },
                _ => default
            })
            .OfType<ItemProperty>()];
    }

    private static OfTheProfessionProperty ParseOfTheProfessionProperty(ItemModifier m)
    {
        if (!Attribute.TryParse((int)m.Argument1, out var attribute) ||
            attribute == Attribute.None)
        {
            return new OfTheProfessionProperty { Profession = Profession.None.Name ?? string.Empty, Attribute = Attribute.None.Name ?? string.Empty, Value = 0 };
        }

        return new OfTheProfessionProperty { Profession = (attribute.Profession ?? Profession.None).Name ?? string.Empty, Attribute = attribute.Name ?? string.Empty, Value = (int)m.Argument2 };
    }

    private static ItemProperty ParseUpgradeProperty(ItemModifier m)
    {
        if (!ItemUpgrade.TryParse((int)m.UpgradeId, out var upgrade) ||
            upgrade == ItemUpgrade.Unknown)
        {
            return new UnknownUpgradeProperty { RawModifier = m };
        }

        return upgrade.Type switch
        {
            ItemUpgradeType.Suffix => new SuffixProperty { Upgrade = upgrade },
            ItemUpgradeType.Prefix => new PrefixProperty { Upgrade = upgrade },
            ItemUpgradeType.Inscription => new InscriptionProperty { Upgrade = upgrade },
            ItemUpgradeType.UpgradeRune => new UpgradesRuneProperty { Upgrade = upgrade },
            ItemUpgradeType.AppliesToRune => new AppliesToRuneProperty { Upgrade = upgrade },
            _ => new UnknownUpgradeProperty { RawModifier = m }
        };
    }

    private static string ParseAttributeName(uint arg)
    {
        return Attribute.TryParse((int)arg, out var attribute)
            ? attribute.Name ?? string.Empty
            : arg is 45 ? "Any Casting Primary" : Attribute.None.Name ?? string.Empty;
    }
}

public sealed class UnknownUpgradeProperty : ItemProperty
{
    [JsonPropertyName("rawModifier")]
    public required ItemModifier RawModifier { get; init; }
}

public sealed class BaneProperty : ItemProperty
{
    [JsonPropertyName("baneSpecies")]
    public required ItemBaneSpecies BaneSpecies { get; init; }
}

public sealed class RequirementProperty : ItemProperty
{
    [JsonPropertyName("attribute")]
    public required string Attribute { get; init; }

    [JsonPropertyName("requirement")]
    public required int Requirement { get; init; }
}

public sealed class DamageProperty : ItemProperty
{
    [JsonPropertyName("minDamage")]
    public required int MinDamage { get; init; }

    [JsonPropertyName("maxDamage")]
    public required int MaxDamage { get; init; }
}

public sealed class ArmorProperty : ItemProperty
{
    [JsonPropertyName("armor")]
    public required int Armor { get; init; }
}

public sealed class EnergyProperty : ItemProperty
{
    [JsonPropertyName("energy")]
    public required int Energy { get; init; }
}

public sealed class SuffixProperty : ItemProperty
{
    [JsonPropertyName("upgrade")]
    public required ItemUpgrade Upgrade { get; init; }
}

public sealed class PrefixProperty : ItemProperty
{
    [JsonPropertyName("upgrade")]
    public required ItemUpgrade Upgrade { get; init; }
}

public sealed class InscriptionProperty : ItemProperty
{
    [JsonPropertyName("upgrade")]
    public required ItemUpgrade Upgrade { get; init; }
}

public sealed class IncreasedSaleValueProperty : ItemProperty
{
    [JsonPropertyName("saleValueBonus")]
    public required int SaleValueBonus { get; init; }
}

public sealed class HighlySalvageableProperty : ItemProperty
{
    [JsonPropertyName("salvageBonus")]
    public required int SalvageBonus { get; init; }
}

public sealed class UpgradesRuneProperty : ItemProperty
{
    [JsonPropertyName("upgrade")]
    public required ItemUpgrade Upgrade { get; init; }
}

public sealed class AppliesToRuneProperty : ItemProperty
{
    [JsonPropertyName("upgrade")]
    public required ItemUpgrade Upgrade { get; init; }
}

public sealed class OfTheProfessionProperty : ItemProperty
{
    [JsonPropertyName("profession")]
    public required string Profession { get; init; }

    [JsonPropertyName("attribute")]
    public required string Attribute { get; init; }

    [JsonPropertyName("value")]
    public required int Value { get; init; }
}

public sealed class CustomizedProperty : ItemProperty
{
}

public sealed class DamageTypeProperty : ItemProperty
{
    [JsonPropertyName("damageType")]
    public required DamageType DamageType { get; init; }
}

public sealed class DamagePlusProperty : ItemProperty
{
    [JsonPropertyName("percentage")]
    public required int Percentage { get; init; }
}

public sealed class DamagePlusVsHexedProperty : ItemProperty
{
    [JsonPropertyName("percentage")]
    public required int Percentage { get; init; }
}

public sealed class DamagePlusWhileEnchantedProperty : ItemProperty
{
    [JsonPropertyName("percentage")]
    public required int Percentage { get; init; }
}

public sealed class DamagePlusWhileUpProperty : ItemProperty
{
    [JsonPropertyName("percentage")]
    public required int Percentage { get; init; }
}

public sealed class DamagePlusWhileDownProperty : ItemProperty
{
    [JsonPropertyName("percentage")]
    public required int Percentage { get; init; }
}

public sealed class DamagePlusWhileHexedProperty : ItemProperty
{
    [JsonPropertyName("percentage")]
    public required int Percentage { get; init; }
}

public sealed class DamagePlusWhileInStanceProperty : ItemProperty
{
    [JsonPropertyName("percentage")]
    public required int Percentage { get; init; }
}

public sealed class DamagePlusVsSpeciesProperty : ItemProperty
{
    [JsonPropertyName("percentage")]
    public required int Percentage { get; init; }
}

public sealed class HalvesCastingTimeGeneralProperty : ItemProperty
{
    [JsonPropertyName("chance")]
    public required int Chance { get; init; }
}

public sealed class HalvesCastingTimeAttributeProperty : ItemProperty
{
    [JsonPropertyName("attribute")]
    public required string Attribute { get; init; }

    [JsonPropertyName("chance")]
    public required int Chance { get; init; }
}

public sealed class HalvesCastingTimeItemAttributeProperty : ItemProperty
{
    [JsonPropertyName("chance")]
    public required int Chance { get; init; }
}

public sealed class HalvesSkillRechargeGeneralProperty : ItemProperty
{
    [JsonPropertyName("chance")]
    public required int Chance { get; init; }
}

public sealed class HalvesSkillRechargeAttributeProperty : ItemProperty
{
    [JsonPropertyName("attribute")]
    public required string Attribute { get; init; }

    [JsonPropertyName("chance")]
    public required int Chance { get; init; }
}

public sealed class HalvesSkillRechargeItemAttributeProperty : ItemProperty
{
    [JsonPropertyName("chance")]
    public required int Chance { get; init; }
}

public sealed class EnergyPlusProperty : ItemProperty
{
    [JsonPropertyName("energy")]
    public required int Energy { get; init; }
}

public sealed class EnergyPlusWhileEnchantedProperty : ItemProperty
{
    [JsonPropertyName("energy")]
    public required int Energy { get; init; }
}

public sealed class EnergyPlusWhileHexedProperty : ItemProperty
{
    [JsonPropertyName("energy")]
    public required int Energy { get; init; }
}

public sealed class EnergyPlusWhileDownProperty : ItemProperty
{
    [JsonPropertyName("energy")]
    public required int Energy { get; init; }

    [JsonPropertyName("healthThreshold")]
    public required int HealthThreshold { get; init; }
}

public sealed class EnergyMinusProperty : ItemProperty
{
    [JsonPropertyName("energy")]
    public required int Energy { get; init; }
}

public sealed class EnergyDegenProperty : ItemProperty
{
    [JsonPropertyName("energyDegen")]
    public required int EnergyDegen { get; init; }
}

public sealed class EnergyRegenProperty : ItemProperty
{
    [JsonPropertyName("energyRegen")]
    public required int EnergyRegen { get; init; }
}

public sealed class EnergyGainOnHitProperty : ItemProperty
{
    [JsonPropertyName("energyGain")]
    public required int EnergyGain { get; init; }
}

public sealed class ArmorPlusProperty : ItemProperty
{
    [JsonPropertyName("armor")]
    public required int Armor { get; init; }
}

public sealed class ArmorPlusVsPhysical : ItemProperty
{
    [JsonPropertyName("armor")]
    public required int Armor { get; init; }
}

public sealed class ArmorPlusVsElemental : ItemProperty
{
    [JsonPropertyName("armor")]
    public required int Armor { get; init; }
}

public sealed class ArmorPlusVsDamageProperty : ItemProperty
{
    [JsonPropertyName("damageType")]
    public required DamageType DamageType { get; init; }

    [JsonPropertyName("armor")]
    public required int Armor { get; init; }
}

public sealed class ArmorPlusVsSpeciesProperty : ItemProperty
{
    [JsonPropertyName("species")]
    public required ItemBaneSpecies Species { get; init; }

    [JsonPropertyName("armor")]
    public required int Armor { get; init; }
}

public sealed class ArmorPlusWhileAttackingProperty : ItemProperty
{
    [JsonPropertyName("armor")]
    public required int Armor { get; init; }
}

public sealed class ArmorPlusWhileCastingProperty : ItemProperty
{
    [JsonPropertyName("armor")]
    public required int Armor { get; init; }
}

public sealed class ArmorPlusWhileEnchantedProperty : ItemProperty
{
    [JsonPropertyName("armor")]
    public required int Armor { get; init; }
}

public sealed class ArmorPlusWhileHexedProperty : ItemProperty
{
    [JsonPropertyName("armor")]
    public required int Armor { get; init; }
}

public sealed class ArmorPlusWhileDownProperty : ItemProperty
{
    [JsonPropertyName("armor")]
    public required int Armor { get; init; }
}

public sealed class ArmorMinusWhileAttackingProperty : ItemProperty
{
    [JsonPropertyName("armor")]
    public required int Armor { get; init; }
}

public sealed class ArmorPenetrationProperty : ItemProperty
{
    [JsonPropertyName("armorPenetration")]
    public required int ArmorPenetration { get; init; }

    [JsonPropertyName("chance")]
    public required int Chance { get; init; }
}

public sealed class HealthPlusProperty : ItemProperty
{
    [JsonPropertyName("health")]
    public required int Health { get; init; }
}

public sealed class HealthPlusWhileHexedProperty : ItemProperty
{
    [JsonPropertyName("health")]
    public required int Health { get; init; }
}

public sealed class HealthPlusWhileDownProperty : ItemProperty
{
    [JsonPropertyName("health")]
    public required int Health { get; init; }

    [JsonPropertyName("healthThreshold")]
    public required int HealthThreshold { get; init; }
}

public sealed class HealthPlusStance : ItemProperty
{
    [JsonPropertyName("health")]
    public required int Health { get; init; }

}

public sealed class HealthMinusProperty : ItemProperty
{
    [JsonPropertyName("health")]
    public required int Health { get; init; }
}

public sealed class HealthDegenProperty : ItemProperty
{
    [JsonPropertyName("healthDegen")]
    public required int HealthDegen { get; init; }
}

public sealed class HealthStealOnHitProperty : ItemProperty
{
    [JsonPropertyName("healthSteal")]
    public required int HealthSteal { get; init; }
}

public sealed class ReceiveLessDamageProperty : ItemProperty
{
    [JsonPropertyName("chance")]
    public required int Chance { get; init; }

    [JsonPropertyName("lessDamage")]
    public required int LessDamage { get; init; }
}

public sealed class ReceiveLessPhysDamageWhileEnchantedProperty : ItemProperty
{
    [JsonPropertyName("lessDamage")]
    public required int LessDamage { get; init; }
}

public sealed class ReceiveLessPhysDamageWhileHexedProperty : ItemProperty
{
    [JsonPropertyName("lessDamage")]
    public required int LessDamage { get; init; }
}

public sealed class ReceiveLessPhysDamageWhileStanceProperty : ItemProperty
{
    [JsonPropertyName("lessDamage")]
    public required int LessDamage { get; init; }
}

public sealed class AttributePlusOneProperty : ItemProperty
{
    [JsonPropertyName("chance")]
    public required int Chance { get; init; }

    [JsonPropertyName("attribute")]
    public required string Attribute { get; init; }
}

public sealed class AttributePlusOneItemProperty : ItemProperty
{
    [JsonPropertyName("chance")]
    public required int Chance { get; init; }
}

public sealed class ReduceConditionDurationProperty : ItemProperty
{
    [JsonPropertyName("conditionType")]
    public required ConditionType Condition { get; init; }
}

public sealed class ReduceConditionTupleDurationProperty : ItemProperty
{
    [JsonPropertyName("condition1Type")]
    public required ConditionType Condition1 { get; init; }

    [JsonPropertyName("condition2Type")]
    public required ConditionType Condition2 { get; init; }
}

public sealed class IncreaseEnchantmentDurationProperty : ItemProperty
{
    [JsonPropertyName("percentage")]
    public required int Percentage { get; init; }
}

public sealed class IncreaseConditionDurationProperty : ItemProperty
{
    [JsonPropertyName("conditionType")]
    public required SkillIdToCondition Condition { get; init; }
}
