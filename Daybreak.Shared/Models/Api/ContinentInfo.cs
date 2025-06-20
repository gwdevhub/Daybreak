using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Api;

[JsonConverter(typeof(JsonStringEnumConverter<ContinentInfo>))]
public enum ContinentInfo
{
    Kryta,
    DevContinent,
    Cantha,
    BattleIsles,
    Elona,
    RealmOfTorment,
    Unknown
}
