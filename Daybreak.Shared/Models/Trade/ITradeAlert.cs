using Daybreak.Shared.Converters;
using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Trade;

[JsonConverter(typeof(TradeAlertConverter))]
public interface ITradeAlert
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; }
}
