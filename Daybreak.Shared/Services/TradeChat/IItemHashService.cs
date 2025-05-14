using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Shared.Services.TradeChat;

public interface IItemHashService
{
    string? ComputeHash(ItemBase itemBase);
}
