using System;

namespace Daybreak.Services.TradeChat.Models;

public sealed class TraderQuoteDTO
{
    public string Id => $"{this.ItemId}_{this.TimeStamp.ToOADate()}_{this.TraderQuoteType}";

    public int ItemId { get; set; }

    public int Price { get; set; }

    public TraderQuoteType TraderQuoteType { get; set; }

    public DateTime TimeStamp { get; set; }

    public DateTime InsertionTime { get; set; }
}
