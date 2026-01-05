namespace Daybreak.Shared.Models.Api;

public sealed record ItemEntry(
    uint ItemModelId,
    string EncodedName,
    string DecodedName,
    string EncodedSingleName,
    string DecodedSingleName,
    string EncodedCompleteName,
    string DecodedCompleteName,
    int Quantity,
    uint[] Modifiers)
{
}
