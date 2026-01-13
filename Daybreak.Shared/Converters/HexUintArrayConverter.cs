using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daybreak.Shared.Converters;

public sealed class HexUIntArrayJsonConverter : JsonConverter<uint[]>
{
    private static readonly HexUintConverter ElementConverter = new();

    public override uint[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        var list = new List<uint>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            list.Add(ElementConverter.Read(ref reader, typeof(uint), options));
        }

        return [.. list];
    }

    public override void Write(Utf8JsonWriter writer, uint[] value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
        {
            ElementConverter.Write(writer, item, options);
        }

        writer.WriteEndArray();
    }
}
