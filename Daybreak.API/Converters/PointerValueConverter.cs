using Daybreak.API.Interop;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daybreak.API.Converters;

public sealed class PointerValueConverter : JsonConverter<PointerValue>
{
    public override PointerValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var s = reader.GetString() ?? throw new JsonException("Expected string");
        return PointerValue.Parse(s);
    }

    public override void Write(Utf8JsonWriter writer, PointerValue value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
