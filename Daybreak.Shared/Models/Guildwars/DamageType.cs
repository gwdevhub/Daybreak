using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Guildwars;

[JsonConverter(typeof(JsonStringEnumConverter<DamageType>))]
public enum DamageType
{
    Blunt,
    Piercing,
    Slashing,
    Icy,
    Shocking,
    Fiery,
    Chaotic,
    Unholy,
    Holy,
    Wooden,
    Sacrificial,
    Ebon,
    Magical,
    UnholyDupe,
    Count,
    None = 0xff
}
