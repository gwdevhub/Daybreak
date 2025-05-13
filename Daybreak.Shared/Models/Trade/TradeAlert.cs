using System;

namespace Daybreak.Models.Trade;
public sealed class TradeAlert : ITradeAlert
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public bool Enabled { get; set; }

    public string? MessageCheck { get; set; }
    public bool MessageRegexCheck { get; set; }

    public string? SenderCheck { get; set; }
    public bool SenderRegexCheck { get; set; }
}
