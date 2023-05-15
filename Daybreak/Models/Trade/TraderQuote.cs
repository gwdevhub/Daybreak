using Daybreak.Models.Guildwars;

namespace Daybreak.Models.Trade;

public sealed class TraderQuote
{
    public ItemBase Item { get; set; }

    public int Price { get; set; }
}
