using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Shared.Services.IconRetrieve;

public interface IIconCache
{
    Task<string?> GetIconUri(Skill skill, bool prefHighQuality = true);
    Task<string?> GetIconUri(ItemBase itemBase);
}
