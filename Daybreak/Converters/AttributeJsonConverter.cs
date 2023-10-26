using Daybreak.Models.Guildwars;
using Newtonsoft.Json;
using System;

namespace Daybreak.Converters;
public sealed class AttributeJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => true;

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (!objectType.IsAssignableTo(typeof(Models.Guildwars.Attribute)))
        {
            return default;
        }

        switch (reader.TokenType)
        {
            case JsonToken.String:
                var name = reader.ReadAsString();
                if (name is null ||
                    !Models.Guildwars.Attribute.TryParse(name, out var namedAttribute))
                {
                    return default;
                }

                return namedAttribute;
            case JsonToken.Integer:
                var id = reader.Value as long?;
                if (id is not long ||
                    !Models.Guildwars.Attribute.TryParse((int)id.Value, out var parsedAttribute))
                {
                    return default;
                }

                return parsedAttribute;

            default:
                return default;
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not Models.Guildwars.Attribute attribute)
        {
            return;
        }

        writer.WriteValue(attribute.Id);
    }
}
