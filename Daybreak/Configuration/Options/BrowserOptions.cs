using Daybreak.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Browser")]
public sealed class BrowserOptions
{
    [JsonProperty(nameof(Enabled))]
    public bool Enabled { get; set; } = true;

    [JsonProperty(nameof(AddressBarReadonly))]
    public bool AddressBarReadonly { get; set; } = true;

    [JsonProperty(nameof(DynamicBuildLoading))]
    public bool DynamicBuildLoading { get; set; } = true;
}
