using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Guildwars;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(BaneProperty), "Bane")]
[JsonDerivedType(typeof(RequirementProperty), "Requirement")]
[JsonDerivedType(typeof(DamageProperty), "Damage")]
[JsonDerivedType(typeof(ArmorProperty), "Armor")]
[JsonDerivedType(typeof(EnergyProperty), "Energy")]
[JsonDerivedType(typeof(SuffixProperty), "Suffix")]
[JsonDerivedType(typeof(PrefixProperty), "Prefix")]
[JsonDerivedType(typeof(InscriptionProperty), "Inscription")]
[JsonDerivedType(typeof(HalvesSkillRecharge), "HalvesSkillRecharge")]
[JsonDerivedType(typeof(HalvesCastingTime), "HalvesCastingTime")]
[JsonDerivedType(typeof(OfTheProfessionProperty), "OfTheProfession")]
[JsonDerivedType(typeof(UnknownUpgradeProperty), "Unknown")]
public abstract class ItemProperty
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
    public required Attribute Attribute { get; init; }

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

public sealed class HalvesSkillRecharge : ItemProperty
{
    [JsonPropertyName("chance")]
    public required int Chance { get; init; }
}

public sealed class HalvesCastingTime : ItemProperty
{
    [JsonPropertyName("chance")]
    public required int Chance { get; init; }
}

public sealed class OfTheProfessionProperty : ItemProperty
{
    [JsonPropertyName("profession")]
    public required Profession Profession { get; init; }

    [JsonPropertyName("attribute")]
    public required Attribute Attribute { get; init; }

    [JsonPropertyName("value")]
    public required int Value { get; init; }
}

public sealed class UnknownUpgradeProperty : ItemProperty
{
}
