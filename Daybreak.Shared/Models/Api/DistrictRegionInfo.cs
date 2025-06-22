using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Api;

[JsonConverter(typeof(JsonStringEnumConverter<DistrictRegionInfo>))]
public enum DistrictRegionInfo
{
    International = -2,
    America = 0,
    Korea,
    Europe,
    China,
    Japan,
    Unknown = 0xff
}
