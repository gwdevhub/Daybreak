using Daybreak.Shared.Models.Trade;

namespace Daybreak.Shared.Services.TradeChat;

public interface ITraderQuoteService
{
    Task<IEnumerable<TraderQuote>> GetBuyQuotes(CancellationToken cancellationToken);

    Task<IEnumerable<TraderQuote>> GetSellQuotes(CancellationToken cancellationToken);
}
