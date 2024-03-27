using System;
using System.Extensions;

namespace Daybreak.Services.TradeChat.Models;

public sealed class TraderQuoteDTO
{
    public string Id => $"{this.ItemId}{(this.ModifiersHash?.IsNullOrWhiteSpace() is true ? "" : $"_{this.ModifiersHash}")}_{(this.IsLatest ? "LATEST" : this.TimeStamp.ToOADate())}_{this.TraderQuoteType}";

    public int ItemId { get; set; }

    public int Price { get; set; }

    public bool IsLatest { get; set; }

    public string? ModifiersHash { get; set; }

    public TraderQuoteType TraderQuoteType { get; set; }

    public DateTime TimeStamp { get; set; }

    public DateTime InsertionTime { get; set; }
}
