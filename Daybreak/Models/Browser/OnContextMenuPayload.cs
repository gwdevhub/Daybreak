using Newtonsoft.Json;

namespace Daybreak.Models.Browser;

public sealed class OnContextMenuPayload
{
    [JsonProperty(nameof(X))]
    public double X { get; set; }
    [JsonProperty(nameof(Y))]
    public double Y { get; set; }
    [JsonProperty(nameof(Selection))]
    public string? Selection { get; set; }
}
