using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Guildwars;

[JsonConverter(typeof(JsonStringEnumConverter<DamageType>))]
public enum DamageType
{
    Blunt,
    Piercing,
    Slashing,
    Cold,
    Lighting,
    Fire,
    Chaos,
    Dark,
    Holy,
    Nature,
    Sacrifice,
    Earth,
    Generic,
    DarkDupe,
    Count,
    None = 0xff
}
