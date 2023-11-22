using Newtonsoft.Json;

namespace Daybreak.Services.Updater.Models;
internal sealed class Metadata
{
    [JsonProperty(nameof(RelativePath))]
    public string? RelativePath { get; set; }

    [JsonProperty(nameof(Name))]
    public string? Name { get; set; }

    [JsonProperty(nameof(Size))]
    public int Size { get; set; }
}
