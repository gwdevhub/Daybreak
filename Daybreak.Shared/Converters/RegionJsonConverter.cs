﻿using Daybreak.Shared.Models.Guildwars;
using Newtonsoft.Json;

namespace Daybreak.Shared.Converters;
public sealed class RegionJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => true;

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (!objectType.IsAssignableTo(typeof(Region)))
        {
            return default;
        }

        switch (reader.TokenType)
        {
            case JsonToken.String:
                var name = reader.ReadAsString();
                if (name is null ||
                    !Region.TryParse(name, out var namedRegion))
                {
                    return default;
                }

                return namedRegion;
            case JsonToken.Integer:
                var id = reader.Value as long?;
                if (id is not long ||
                    !Region.TryParse((int)id.Value, out var parsedRegion))
                {
                    return default;
                }

                return parsedRegion;

            default:
                return default;
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not Region region)
        {
            return;
        }

        writer.WriteValue(region.Id);
    }
}
