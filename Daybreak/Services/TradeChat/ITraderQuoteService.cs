using Daybreak.Models.Guildwars;
using Daybreak.Models.Trade;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.TradeChat;

public interface ITraderQuoteService
{
    Task<IEnumerable<TraderQuote>> GetBuyQuotes(CancellationToken cancellationToken);

    Task<IEnumerable<TraderQuote>> GetSellQuotes(CancellationToken cancellationToken);
}
