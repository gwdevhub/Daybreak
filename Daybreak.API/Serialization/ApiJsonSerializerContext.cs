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
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(Task<IResult>))]
[JsonSerializable(typeof(Task))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(JsonElement))]
[JsonSerializable(typeof(HealthCheckResponse))]
[JsonSerializable(typeof(HealthCheckEntryResponse))]
[JsonSerializable(typeof(HookState))]
[JsonSerializable(typeof(ArraySegment<HookState>))]
[JsonSerializable(typeof(AddressState))]
[JsonSerializable(typeof(ArraySegment<AddressState>))]
[JsonSerializable(typeof(MainPlayerState))]
[JsonSerializable(typeof(QuestLogInformation))]
[JsonSerializable(typeof(QuestInformation))]
[JsonSerializable(typeof(MainPlayerInformation))]
[JsonSerializable(typeof(CharacterSelectInformation))]
[JsonSerializable(typeof(CharacterSelectEntry))]
[JsonSerializable(typeof(BuildEntry))]
[JsonSerializable(typeof(List<BuildEntry>))]
[JsonSerializable(typeof(PartyLoadoutEntry))]
[JsonSerializable(typeof(List<PartyLoadout>))]
[JsonSerializable(typeof(InstanceInfo))]
[JsonSerializable(typeof(TitleInfo))]
public partial class ApiJsonSerializerContext : JsonSerializerContext
{
}
