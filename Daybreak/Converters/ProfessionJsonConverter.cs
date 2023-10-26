using Daybreak.Models.Guildwars;
using Newtonsoft.Json;
using System;

namespace Daybreak.Converters;
public sealed class ProfessionJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => true;

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (!objectType.IsAssignableTo(typeof(Profession)))
        {
            return default;
        }

        switch (reader.TokenType)
        {
            case JsonToken.String:
                var name = reader.ReadAsString();
                if (name is null ||
                    !Profession.TryParse(name, out var namedProfession))
                {
                    return default;
                }

                return namedProfession;
            case JsonToken.Integer:
                var id = reader.Value as long?;
                if (id is not long ||
                    !Profession.TryParse((int)id.Value, out var parsedProfession))
                {
                    return default;
                }

                return parsedProfession;

            default:
                return default;
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not Profession profession)
        {
            return;
        }

        writer.WriteValue(profession.Id);
    }
}
