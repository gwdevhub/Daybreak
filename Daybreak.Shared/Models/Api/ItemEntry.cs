using Daybreak.Shared.Converters;
using Daybreak.Shared.Models.Guildwars;
using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Api;

public sealed record ItemEntry(
    uint ItemModelId,
    string EncodedName,
    string DecodedName,
    string EncodedSingleName,
    string DecodedSingleName,
    string EncodedCompleteName,
    string DecodedCompleteName,
    string ItemType,
    bool Inscribable,
    int Quantity,
    [property: JsonConverter(typeof(HexUIntArrayJsonConverter))] uint[] Modifiers,
    ItemProperty[] Properties)
{
}
