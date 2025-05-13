using Daybreak.Models.Guildwars;
using Daybreak.Models.Trade;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.TradeChat;

public interface IPriceHistoryService
{
    Task<IEnumerable<TraderQuote>> GetPriceHistory(ItemBase itemBase, CancellationToken cancellationToken, DateTime? from = default, DateTime? to = default);
}
