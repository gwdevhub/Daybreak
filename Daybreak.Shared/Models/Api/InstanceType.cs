using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Api;

[JsonConverter(typeof(JsonStringEnumConverter<InstanceType>))]
public enum InstanceType
{
    Outpost,
    Explorable,
    Loading,
    Undefined
}
