using Daybreak.Models.Guildwars;
using Newtonsoft.Json;
using System;

namespace Daybreak.Converters;
public sealed class ItemBaseJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => true;

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (!objectType.IsAssignableTo(typeof(ItemBase)))
        {
            return default;
        }

        switch (reader.TokenType)
        {
            case JsonToken.String:
                var name = reader.ReadAsString();
                if (name is not string ||
                    !ItemBase.TryParse(name, out var namedItem))
                {
                    return default;
                }

                return namedItem;
            case JsonToken.Integer:
                var id = reader.Value as long?;
                if (id is not long ||
                    !ItemBase.TryParse((int)id.Value, null, out var parsedItem))
                {
                    return default;
                }

                return parsedItem;

            default:
                return default;
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not ItemBase itemBase)
        {
            return;
        }

        writer.WriteValue(itemBase.Id);
    }
}
