using Daybreak.Shared.Models.Guildwars;
using Newtonsoft.Json;
using System;

namespace Daybreak.Shared.Converters;
public sealed class QuestJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => true;

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (!objectType.IsAssignableTo(typeof(Quest)))
        {
            return default;
        }

        switch (reader.TokenType)
        {
            case JsonToken.String:
                var name = reader.ReadAsString();
                if (name is null ||
                    !Quest.TryParse(name, out var namedQuest))
                {
                    return default;
                }

                return namedQuest;
            case JsonToken.Integer:
                var id = reader.Value as long?;
                if (id is not long ||
                    !Quest.TryParse((int)id.Value, out var parsedQuest))
                {
                    return default;
                }

                return parsedQuest;

            default:
                return default;
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not Quest quest)
        {
            return;
        }

        writer.WriteValue(quest.Id);
    }
}
