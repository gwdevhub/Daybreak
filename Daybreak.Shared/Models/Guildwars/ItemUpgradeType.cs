using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Guildwars;

[JsonConverter(typeof(JsonStringEnumConverter<ItemUpgradeType>))]
public enum ItemUpgradeType
{
    Unknown,
    Prefix,
    Suffix,
    Inscription
}
