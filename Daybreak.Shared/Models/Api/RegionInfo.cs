using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Api;

[JsonConverter(typeof(JsonStringEnumConverter<RegionInfo>))]
public enum RegionInfo
{
    Kryta,
    Maguuma,
    Ascalon,
    NorthernShiverpeaks,
    HeroesAscent,
    CrystalDesert,
    FissureOfWoe,
    Presearing,
    Kaineng,
    Kurzick,
    Luxon,
    ShingJea,
    Kourna,
    Vaabi,
    Desolation,
    Istan,
    DomainOfAnguish,
    TarnishedCoast,
    DepthsOfTyria,
    FarShiverpeaks,
    CharrHomelands,
    BattleIslands,
    TheBattleOfJahai,
    TheFlightNorth,
    TheTenguAccords,
    TheRiseOfTheWhiteMantle,
    Swat,
    DevRegion,
    Unknown
}
