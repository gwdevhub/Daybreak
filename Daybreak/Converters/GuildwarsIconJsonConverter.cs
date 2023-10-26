using Daybreak.Models.Guildwars;
using Newtonsoft.Json;
using System;

namespace Daybreak.Converters;
public sealed class GuildwarsIconJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => true;

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (!objectType.IsAssignableTo(typeof(GuildwarsIcon)))
        {
            return default;
        }

        switch (reader.TokenType)
        {
            case JsonToken.String:
                var name = reader.ReadAsString();
                if (name is null ||
                    !GuildwarsIcon.TryParse(name, out var namedGuildwarsIcon))
                {
                    return default;
                }

                return namedGuildwarsIcon;
            case JsonToken.Integer:
                var id = reader.Value as long?;
                if (id is not long ||
                    !GuildwarsIcon.TryParse((int)id.Value, out var parsedGuildwarsIcon))
                {
                    return default;
                }

                return parsedGuildwarsIcon;

            default:
                return default;
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not GuildwarsIcon guildwarsIcon)
        {
            return;
        }

        writer.WriteValue(guildwarsIcon.Id);
    }
}
