using Daybreak.Shared.Models.Guildwars;
using System.Threading.Tasks;

namespace Daybreak.Shared.Services.IconRetrieve;

public interface IIconCache
{
    Task<string?> GetIconUri(Skill skill, bool prefHighQuality = true);
    Task<string?> GetIconUri(ItemBase itemBase);
}
