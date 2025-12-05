using Daybreak.Shared.Models;
using Newtonsoft.Json;

namespace Daybreak.Shared.Converters;
public sealed class ThemeJsonConverter : JsonConverter<Theme>
{
    public override Theme? ReadJson(JsonReader reader, Type objectType, Theme? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType is JsonToken.String)
        {
            var themeName = reader.Value?.ToString();
            if (themeName is not null)
            {
                return Theme.Themes.FirstOrDefault(t => t.Name.Equals(themeName, StringComparison.OrdinalIgnoreCase));
            }
        }

        throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing theme. Expected a string representing the theme name.");
    }

    public override void WriteJson(JsonWriter writer, Theme? value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.Name ?? string.Empty);
    }
}
