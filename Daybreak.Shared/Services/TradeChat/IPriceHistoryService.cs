using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Models.Trade;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Shared.Services.TradeChat;

public interface IPriceHistoryService
{
    Task<IEnumerable<TraderQuote>> GetPriceHistory(ItemBase itemBase, CancellationToken cancellationToken, DateTime? from = default, DateTime? to = default);
}
