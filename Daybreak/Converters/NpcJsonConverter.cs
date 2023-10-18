using Daybreak.Models.Guildwars;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Daybreak.Converters;
public sealed class NpcJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => true;

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (!objectType.IsAssignableTo(typeof(Npc)))
        {
            return default;
        }

        switch (reader.TokenType)
        {
            case JsonToken.String:
                var name = reader.ReadAsString();
                if (name is null ||
                    !Npc.TryParse(name, out var namedNpc))
                {
                    return default;
                }

                return namedNpc;
            case JsonToken.Integer:
                var id = reader.ReadAsInt32();
                if (id is not int ||
                    !Npc.TryParse(id.Value, out var parsedNpc))
                {
                    return default;
                }

                return parsedNpc;

            default:
                return default;
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not Npc npc)
        {
            return;
        }

        writer.WriteValue(npc.Ids.FirstOrDefault());
    }
}
