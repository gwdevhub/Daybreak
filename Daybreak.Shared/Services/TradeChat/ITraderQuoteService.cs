using Daybreak.Shared.Models.Trade;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Shared.Services.TradeChat;

public interface ITraderQuoteService
{
    Task<IEnumerable<TraderQuote>> GetBuyQuotes(CancellationToken cancellationToken);

    Task<IEnumerable<TraderQuote>> GetSellQuotes(CancellationToken cancellationToken);
}
