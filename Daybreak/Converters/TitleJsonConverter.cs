using Daybreak.Models.Guildwars;
using Newtonsoft.Json;
using System;

namespace Daybreak.Converters;
public sealed class TitleJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => true;

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (!objectType.IsAssignableTo(typeof(Title)))
        {
            return default;
        }

        switch (reader.TokenType)
        {
            case JsonToken.String:
                var name = reader.ReadAsString();
                if (name is null ||
                    !Title.TryParse(name, out var namedTitle))
                {
                    return default;
                }

                return namedTitle;
            case JsonToken.Integer:
                var id = reader.ReadAsInt32();
                if (id is not int ||
                    !Title.TryParse(id.Value, out var parsedTitle))
                {
                    return default;
                }

                return parsedTitle;

            default:
                return default;
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not Title title)
        {
            return;
        }

        writer.WriteValue(title.Id);
    }
}
