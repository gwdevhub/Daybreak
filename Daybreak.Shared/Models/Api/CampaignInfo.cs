using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Api;

[JsonConverter(typeof(JsonStringEnumConverter<CampaignInfo>))]
public enum CampaignInfo
{
    Core,
    Prophecies,
    Factions,
    Nightfall,
    EyeOfTheNorth,
    BonusMissionPack,
    Unknown
}
