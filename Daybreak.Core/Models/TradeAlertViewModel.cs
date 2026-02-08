using Daybreak.Shared.Models.Trade;

namespace Daybreak.Models;
public sealed class TradeAlertViewModel
{
    public enum TradeAlertType
    {
        Unknown,
        Quote,
        Message
    }

    public required ITradeAlert TradeAlert { get; init; }

    public required TradeAlertType Type { get; init; }
}
