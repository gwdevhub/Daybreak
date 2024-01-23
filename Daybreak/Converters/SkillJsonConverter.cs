using Daybreak.Models.Guildwars;
using Newtonsoft.Json;
using System;

namespace Daybreak.Converters;
public sealed class SkillJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => true;

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (!objectType.IsAssignableTo(typeof(Skill)))
        {
            return default;
        }

        switch (reader.TokenType)
        {
            case JsonToken.String:
                var name = reader.ReadAsString();
                if (name is null ||
                    !Skill.TryParse(name, out var namedSkill))
                {
                    return default;
                }

                return namedSkill;
            case JsonToken.Integer:
                var id = reader.Value as long?;
                if (id is not long ||
                    !Skill.TryParse((int)id.Value, out var parsedSkill))
                {
                    return default;
                }

                return parsedSkill;

            default:
                return default;
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not Skill skill)
        {
            return;
        }

        writer.WriteValue(skill.Id);
    }
}
