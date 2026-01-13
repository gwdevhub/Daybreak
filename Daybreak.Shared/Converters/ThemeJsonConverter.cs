using Daybreak.Shared.Models.Themes;
using Daybreak.Shared.Services.Themes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daybreak.Shared.Converters;
public sealed class ThemeJsonConverter : JsonConverter<Theme>
{
    public override Theme? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => reader.GetString() is string themeName
                ? themeName is GameScreenshotsTheme.ThemeName
                    ? Theme.Themes.FirstOrDefault(t => t is GameScreenshotsTheme)
                    : Theme.Themes.FirstOrDefault(t => t.Name.Equals(themeName, StringComparison.OrdinalIgnoreCase))
                : default,
            _ => default
        };
    }

    public override void Write(Utf8JsonWriter writer, Theme value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Name);
    }
}
