﻿using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Shared.Models.Trade;

public sealed class TraderQuote
{
    public ItemBase? Item { get; set; }

    public int Price { get; set; }

    public DateTime? Timestamp { get; set; }
}
