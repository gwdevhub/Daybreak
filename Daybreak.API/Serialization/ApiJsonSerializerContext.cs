using Daybreak.API.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daybreak.API.Serialization;

[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(byte[]))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(uint))]
[JsonSerializable(typeof(nuint))]
[JsonSerializable(typeof(HealthCheckResponse))]
[JsonSerializable(typeof(HealthCheckEntryResponse))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(HookState))]
[JsonSerializable(typeof(List<HookState>))]
[JsonSerializable(typeof(AddressState))]
[JsonSerializable(typeof(List<AddressState>))]
[JsonSerializable(typeof(GameState))]
[JsonSerializable(typeof(JsonElement))]
[JsonSerializable(typeof(Task<IResult>))]
public partial class ApiJsonSerializerContext : JsonSerializerContext
{
}
