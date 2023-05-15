using Daybreak.Models.Guildwars;
using System;
using System.Threading.Tasks;

namespace Daybreak.Services.IconRetrieve;

public interface IIconCache
{
    Task<Uri?> GetIconUri(Skill skill);
    Task<Uri?> GetIconUri(ItemBase itemBase);
}
