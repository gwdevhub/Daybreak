using Daybreak.Models.Guildwars;
using System.Linq;
using System.Text;

namespace Daybreak.Services.TradeChat;

public sealed class ItemHashService : IItemHashService
{
    public string? ComputeHash(ItemBase itemBase)
    {
        if (itemBase.Modifiers is null ||
            itemBase.Modifiers.Count() == 0)
        {
            return default;
        }

        var sb = new StringBuilder();
        foreach(var modifier in itemBase.Modifiers)
        {
            sb.Append(modifier.Modifier.ToString("X"));
        }

        return sb.ToString();
    }
}
