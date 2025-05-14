using Daybreak.Shared.Models.Guildwars;
using Newtonsoft.Json;
using System;

namespace Daybreak.Shared.Converters;
public sealed class MapJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => true;

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (!objectType.IsAssignableTo(typeof(Map)))
        {
            return default;
        }

        switch (reader.TokenType)
        {
            case JsonToken.String:
                var name = reader.ReadAsString();
                if (name is null ||
                    !Map.TryParse(name, out var namedMap))
                {
                    return default;
                }

                return namedMap;
            case JsonToken.Integer:
                var id = reader.Value as long?;
                if (id is not long ||
                    !Map.TryParse((int)id.Value, out var parsedMap))
                {
                    return default;
                }

                return parsedMap;

            default:
                return default;
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not Map map)
        {
            return;
        }

        writer.WriteValue(map.Id);
    }
}
