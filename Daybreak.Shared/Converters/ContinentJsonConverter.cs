using Daybreak.Shared.Models.Guildwars;
using Newtonsoft.Json;
using System;

namespace Daybreak.Shared.Converters;
public sealed class ContinentJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => true;

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (!objectType.IsAssignableTo(typeof(Continent)))
        {
            return default;
        }

        switch (reader.TokenType)
        {
            case JsonToken.String:
                var name = reader.ReadAsString();
                if (name is null ||
                    !Continent.TryParse(name, out var namedContinent))
                {
                    return default;
                }

                return namedContinent;
            case JsonToken.Integer:
                var id = reader.Value as long?;
                if (id is not long ||
                    !Continent.TryParse((int)id.Value, out var parsedContinent))
                {
                    return default;
                }

                return parsedContinent;

            default:
                return default;
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not Continent continent)
        {
            return;
        }

        writer.WriteValue(continent.Id);
    }
}
