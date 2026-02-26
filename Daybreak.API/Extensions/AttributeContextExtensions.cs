using Daybreak.API.Interop.GuildWars;
using Daybreak.Shared.Models.Api;

namespace Daybreak.API.Extensions;

public static class AttributeContextExtensions
{
    public static IEnumerable<AttributeEntry> GetAttributeEntryList(this AttributeStructArray54 attributes)
    {
        foreach (var attribute in attributes)
        {            
            if (attribute.LevelBase > 0 && attribute.Level >= attribute.LevelBase)
            {
                yield return new AttributeEntry((uint)attribute.Id, attribute.LevelBase, attribute.Level);
            }
        }
    }
}
