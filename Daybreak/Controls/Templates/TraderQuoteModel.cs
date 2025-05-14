using Daybreak.Shared.Models.Guildwars;
using System;

namespace Daybreak.Controls.Templates;
public sealed class TraderQuoteModel
{
    public ItemBase? Item { get; set; }
    public int BuyPrice { get; set; }
    public int SellPrice { get; set; }
    public DateTime TimeStamp { get; set; }
}
