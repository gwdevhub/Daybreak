using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daybreak.Shared.Converters;

public sealed class HexIntConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var hexString = reader.GetString();
            if (hexString is not null && hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                return Convert.ToInt32(hexString[2..], 16);
            }
        }

        return reader.GetInt32();
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteStringValue($"0x{value:X4}");
    }
}
