using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Daybreak.Models.Guildwars;

public sealed class PartyCompositionMetadataEntry
{
    public required PartyCompositionMemberType Type { get; init; }

    public required int Index { get; init; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? HeroId { get; init; }
}

[JsonConverter(typeof(StringEnumConverter))]
public enum PartyCompositionMemberType
{
    Unknown,
    MainPlayer,
    Player,
    Hero,
    Henchman
}
