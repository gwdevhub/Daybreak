using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Api;

[JsonConverter(typeof(JsonStringEnumConverter<DifficultyInfo>))]
public enum DifficultyInfo
{
    Normal,
    Hard,
    Unknown
}
