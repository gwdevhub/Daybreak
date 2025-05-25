using Daybreak.API.Models;
using Daybreak.Shared.Models.Api;
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
[JsonSerializable(typeof(ArraySegment<HookState>))]
[JsonSerializable(typeof(AddressState))]
[JsonSerializable(typeof(ArraySegment<AddressState>))]
[JsonSerializable(typeof(MainPlayerState))]
[JsonSerializable(typeof(JsonElement))]
[JsonSerializable(typeof(Task<IResult>))]
[JsonSerializable(typeof(Task))]
public partial class ApiJsonSerializerContext : JsonSerializerContext
{
}
