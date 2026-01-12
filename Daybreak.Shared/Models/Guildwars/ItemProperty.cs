namespace Daybreak.Shared.Models.Guildwars;

public abstract record ItemProperty(ItemModifier RawModifier, string Name)
{
}

public sealed record BaneProperty(ItemModifier RawModifier, ItemBaneSpecies BaneSpecies) : ItemProperty(RawModifier, "Bane")
{
}

public sealed record RequirementProperty(ItemModifier RawModifier, Attribute Attribute, int Requirement) : ItemProperty(RawModifier, "Requirement")
{
}

public sealed record DamageProperty(ItemModifier RawModifier, int MinDamage, int MaxDamage) : ItemProperty(RawModifier, "Damage")
{
}

public sealed record ArmorProperty(ItemModifier RawModifier, int Armor) : ItemProperty(RawModifier, "Armor")
{
}

public sealed record EnergyProperty(ItemModifier RawModifier, int Energy) : ItemProperty(RawModifier, "Energy")
{
}

public sealed record SuffixProperty(ItemModifier RawModifier, ItemUpgrade Upgrade) : ItemProperty(RawModifier, "Suffix")
{
}

public sealed record PrefixProperty(ItemModifier RawModifier, ItemUpgrade Upgrade) : ItemProperty(RawModifier, "Prefix")
{
}

public sealed record InscriptionProperty(ItemModifier RawModifier, ItemUpgrade Upgrade) : ItemProperty(RawModifier, "Inscription")
{
}

public sealed record HalvesSkillRecharge(ItemModifier ItemModifier, int Chance) : ItemProperty(ItemModifier, "HalvesSkillRecharge")
{
}

public sealed record HalvesCastingTime(ItemModifier ItemModifier, int Chance) : ItemProperty(ItemModifier, "HalvesCastingTime")
{
}

public sealed record OfTheProfessionProperty(ItemModifier RawModifier, Profession Profession, Attribute Attribute, int Value) : ItemProperty(RawModifier, "OfTheProfession")
{
}

public sealed record UnknownUpgradeProperty(ItemModifier RawModifier) : ItemProperty(RawModifier, "Unknown")
{
}
