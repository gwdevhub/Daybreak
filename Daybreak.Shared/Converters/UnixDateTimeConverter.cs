using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daybreak.Shared.Converters;

public sealed class UnixDateTimeConverter : JsonConverter<DateTime>
{
    private static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number => UnixEpoch.AddSeconds(reader.GetInt64()),
            JsonTokenType.String when long.TryParse(reader.GetString(), out var seconds) => UnixEpoch.AddSeconds(seconds),
            _ => throw new JsonException($"Unable to convert {reader.TokenType} to Unix DateTime")
        };
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var unixTime = (long)(value.ToUniversalTime() - UnixEpoch).TotalSeconds;
        writer.WriteNumberValue(unixTime);
    }
}
