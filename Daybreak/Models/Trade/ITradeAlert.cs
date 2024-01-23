using Daybreak.Converters;
using Newtonsoft.Json;

namespace Daybreak.Models.Trade;

[JsonConverter(typeof(TradeAlertConverter))]
public interface ITradeAlert
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; }
}
