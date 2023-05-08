using Daybreak.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Browser")]
public sealed class BrowserOptions
{
    [JsonProperty(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, the browser component will be displayed")]
    public bool Enabled { get; set; } = true;

    [JsonProperty(nameof(AddressBarReadonly))]
    [OptionName(Name = "Read-only Address Bar", Description = "If true, the browser component will force the address bar to be read-only")]
    public bool AddressBarReadonly { get; set; } = true;

    [JsonProperty(nameof(DynamicBuildLoading))]
    [OptionName(Name = "Dynamic Build Loading", Description = "If true, the browser will dynamically parse build templates and prompt the user to load them")]
    public bool DynamicBuildLoading { get; set; } = true;
}
