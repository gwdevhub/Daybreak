using Daybreak.Models.Guildwars;
using System.Collections.Generic;

namespace Daybreak.Services.PriceChecker.CheckerModules;
internal interface IIdentifierModule
{
    IEnumerable<ItemBase> IdentifyItems(IBagContent item);
}
