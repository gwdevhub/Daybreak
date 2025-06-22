using Daybreak.API.Interop.GuildWars;
using Daybreak.Shared.Models.Api;
using ZLinq;

namespace Daybreak.API.Extensions;

public static class AttributeContextExtensions
{
    public static List<AttributeEntry> GetAttributeEntryList(this AttributeContext[] attributes)
    {
        return attributes
                .AsValueEnumerable()
                .Take(45) // There are only 45 maximum attributes in game
                .Where(a => a.LevelBase > 0 && a.Level >= a.LevelBase) // Select only attributes with points in them
                .Select(a => new AttributeEntry(a.Id, a.LevelBase, a.Level)).ToList();
    }
}
