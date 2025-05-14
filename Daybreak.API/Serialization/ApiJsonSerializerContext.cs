using System.Text.Json.Serialization;

namespace Daybreak.API.Serialization;

[JsonSerializable(typeof(string))]
public partial class ApiJsonSerializerContext : JsonSerializerContext
{
}
