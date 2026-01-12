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
                ItemModifierIdentifier.BaneSpecies          => new BaneProperty(m, (ItemBaneSpecies)m.Argument1),
                ItemModifierIdentifier.Attribute            => ParseRequirementProperty(m),
                ItemModifierIdentifier.Damage               => new DamageProperty(m, (int)m.Argument2, (int)m.Argument1),
                ItemModifierIdentifier.Armor1               => new ArmorProperty(m, (int)m.Argument2),
                ItemModifierIdentifier.Armor2               => new ArmorProperty(m, (int)m.Argument2),
                ItemModifierIdentifier.Energy               => new EnergyProperty(m, (int)m.Argument1),
                ItemModifierIdentifier.Upgrade1             => ParseUpgradeProperty(m),
                ItemModifierIdentifier.Upgrade2             => ParseUpgradeProperty(m),
                ItemModifierIdentifier.HalvesSkillRecharge  => new HalvesSkillRecharge(m, (int)m.Argument1),
                ItemModifierIdentifier.HalvesCastingTime    => new HalvesCastingTime(m, (int)m.Argument1),
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
            return new OfTheProfessionProperty(m, Profession.None, Attribute.None, 0);
        }

        return new OfTheProfessionProperty(m, attribute.Profession ?? Profession.None, attribute, (int)m.Argument2);
    }

    private static ItemProperty ParseUpgradeProperty(ItemModifier m)
    {
        if (!ItemUpgrade.TryParse((int)m.UpgradeId, out var upgrade) ||
            upgrade == ItemUpgrade.Unknown)
        {
            return new UnknownUpgradeProperty(m);
        }

        return upgrade.Type switch
        {
            ItemUpgradeType.Suffix => new SuffixProperty(m, upgrade),
            ItemUpgradeType.Prefix => new PrefixProperty(m, upgrade),
            ItemUpgradeType.Inscription => new InscriptionProperty(m, upgrade),
            _ => new UnknownUpgradeProperty(m)
        };
    }

    private static RequirementProperty ParseRequirementProperty(ItemModifier m)
    {
        return Attribute.TryParse((int)m.Argument1, out var attribute)
            ? new RequirementProperty(m, attribute, (int)m.Argument2)
            : new RequirementProperty(m, Attribute.None, (int)m.Argument2);
    }
}
