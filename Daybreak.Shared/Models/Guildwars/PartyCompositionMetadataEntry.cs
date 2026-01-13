using Daybreak.Shared.Models.Api;
using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Guildwars;

public sealed class PartyCompositionMetadataEntry
{
    public required PartyCompositionMemberType Type { get; init; }

    public required int Index { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? HeroId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HeroBehavior? Behavior { get; init; }
}

[JsonConverter(typeof(JsonStringEnumConverter<PartyCompositionMemberType>))]
public enum PartyCompositionMemberType
{
    Unknown,
    MainPlayer,
    Player,
    Hero,
    Henchman
}
