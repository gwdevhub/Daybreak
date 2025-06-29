namespace Daybreak.Shared.Models.Api;
public sealed record BuildEntry(int Primary, int Secondary, List<AttributeEntry> Attributes, List<uint> Skills)
{
}
