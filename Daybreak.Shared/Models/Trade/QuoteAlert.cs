namespace Daybreak.Shared.Models.Trade;
public sealed class QuoteAlert : ITradeAlert
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public bool Enabled { get; set; }

    public int ItemId { get; set; }
    public TraderQuoteType TraderQuoteType { get; set; }

    public int UpperPriceTarget { get; set; }
    public bool UpperPriceTargetEnabled { get; set; }

    public int LowerPriceTarget { get; set; }
    public bool LowerPriceTargetEnabled { get; set; }
}
