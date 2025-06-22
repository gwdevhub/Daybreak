using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Api;

[JsonConverter(typeof(JsonStringEnumConverter<HeroBehavior>))]
public enum HeroBehavior
{
    Fight,
    Guard,
    AvoidCombat,
    Undefined
}
