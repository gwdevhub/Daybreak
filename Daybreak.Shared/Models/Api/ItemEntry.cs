using Daybreak.Shared.Models.Guildwars;

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
    uint[] Modifiers,
    ItemProperty[] Properties)
{
}
