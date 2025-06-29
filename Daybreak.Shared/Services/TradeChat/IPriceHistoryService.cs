using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Models.Trade;

namespace Daybreak.Shared.Services.TradeChat;

public interface IPriceHistoryService
{
    Task<IEnumerable<TraderQuote>> GetPriceHistory(ItemBase itemBase, CancellationToken cancellationToken, DateTime? from = default, DateTime? to = default);
}
