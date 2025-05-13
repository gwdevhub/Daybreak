using Daybreak.Models.Guildwars;

namespace Daybreak.Services.TradeChat;

public interface IItemHashService
{
    string? ComputeHash(ItemBase itemBase);
}
