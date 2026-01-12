using Daybreak.Shared.Models.Guildwars;
using System.Collections.Immutable;
using Attribute = Daybreak.Shared.Models.Guildwars.Attribute;

namespace Daybreak.Shared.Services.Items;

public sealed class ItemModifierParser : IItemModifierParser
{
    public ImmutableArray<ItemProperty> ParseItemModifiers(ImmutableArray<ItemModifier> modifiers)
    {
        return [.. modifiers.Select(
            m => m.Identifier switch
            {
                ItemModifierIdentifier.BaneSpecies          => new BaneProperty { RawModifier = m, BaneSpecies = (ItemBaneSpecies)m.Argument1 },
                ItemModifierIdentifier.Attribute            => ParseRequirementProperty(m),
                ItemModifierIdentifier.Damage               => new DamageProperty { RawModifier = m, MinDamage = (int)m.Argument2, MaxDamage = (int)m.Argument1 },
                ItemModifierIdentifier.Armor1               => new ArmorProperty { RawModifier = m,  Armor =(int)m.Argument2 },
                ItemModifierIdentifier.Armor2               => new ArmorProperty { RawModifier = m,  Armor = (int)m.Argument2 },
                ItemModifierIdentifier.Energy               => new EnergyProperty { RawModifier = m, Energy = (int)m.Argument1 },
                ItemModifierIdentifier.Upgrade1             => ParseUpgradeProperty(m),
                ItemModifierIdentifier.Upgrade2             => ParseUpgradeProperty(m),
                ItemModifierIdentifier.HalvesSkillRecharge  => new HalvesSkillRecharge { RawModifier = m, Chance =(int)m.Argument1 },
                ItemModifierIdentifier.HalvesCastingTime    => new HalvesCastingTime { RawModifier = m, Chance = (int)m.Argument1 },
                ItemModifierIdentifier.OfTheProfession      => ParseOfTheProfessionProperty(m),
                _ => default
            })
            .OfType<ItemProperty>()];
    }

    private static OfTheProfessionProperty ParseOfTheProfessionProperty(ItemModifier m)
    {
        if (!Attribute.TryParse((int)m.Argument1, out var attribute) ||
            attribute == Attribute.None)
        {
            return new OfTheProfessionProperty { RawModifier = m, Profession = Profession.None, Attribute = Attribute.None, Value = 0 };
        }

        return new OfTheProfessionProperty { RawModifier = m, Profession = attribute.Profession ?? Profession.None, Attribute = attribute, Value = (int)m.Argument2 };
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
            ItemUpgradeType.Suffix => new SuffixProperty { RawModifier = m, Upgrade = upgrade },
            ItemUpgradeType.Prefix => new PrefixProperty { RawModifier = m, Upgrade = upgrade },
            ItemUpgradeType.Inscription => new InscriptionProperty { RawModifier = m, Upgrade = upgrade },
            _ => new UnknownUpgradeProperty { RawModifier = m }
        };
    }

    private static RequirementProperty ParseRequirementProperty(ItemModifier m)
    {
        return Attribute.TryParse((int)m.Argument1, out var attribute)
            ? new RequirementProperty { RawModifier = m, Attribute = attribute, Requirement = (int)m.Argument2 }
            : new RequirementProperty { RawModifier = m, Attribute = Attribute.None, Requirement = (int)m.Argument2 };
    }
}
