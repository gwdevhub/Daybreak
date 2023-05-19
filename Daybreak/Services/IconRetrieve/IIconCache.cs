using Daybreak.Models.Guildwars;
using System;
using System.Threading.Tasks;

namespace Daybreak.Services.IconRetrieve;

public interface IIconCache
{
    Task<string?> GetIconUri(Skill skill);
    Task<string?> GetIconUri(ItemBase itemBase);
}
