namespace Daybreak.Shared.Models.Guildwars;

/// <summary>
/// Parsed metadata from a party loadout template (extended template, type 1).
/// </summary>
public readonly struct PartyLoadoutTemplateMetadata(
    int version,
    int partySize,
    List<PartyLoadoutMemberMetadata> members)
{
    public readonly int Version = version;
    public readonly int PartySize = partySize;
    public readonly List<PartyLoadoutMemberMetadata> Members = members;
}

/// <summary>
/// Metadata for a single party member in a party loadout template.
/// </summary>
public readonly struct PartyLoadoutMemberMetadata(
    PartyCompositionMemberType memberType,
    int heroId,
    int behavior,
    int primaryProfessionId,
    int secondaryProfessionId,
    List<int> attributeIds,
    List<int> attributePoints,
    List<int> skillIds)
{
    public readonly PartyCompositionMemberType MemberType = memberType;
    public readonly int HeroId = heroId;
    public readonly int Behavior = behavior;
    public readonly int PrimaryProfessionId = primaryProfessionId;
    public readonly int SecondaryProfessionId = secondaryProfessionId;
    public readonly List<int> AttributeIds = attributeIds;
    public readonly List<int> AttributePoints = attributePoints;
    public readonly List<int> SkillIds = skillIds;
}
