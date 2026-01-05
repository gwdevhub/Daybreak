namespace Daybreak.Shared.Models.Api;

public sealed record BagEntry(string BagType, List<ItemEntry> Items)
{
}
