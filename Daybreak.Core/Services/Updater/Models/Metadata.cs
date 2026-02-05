using System.Text.Json.Serialization;

namespace Daybreak.Services.Updater.Models;
internal sealed class Metadata
{
    [JsonPropertyName(nameof(RelativePath))]
    public string? RelativePath { get; set; }

    [JsonPropertyName(nameof(Name))]
    public string? Name { get; set; }

    [JsonPropertyName(nameof(Size))]
    public int Size { get; set; }

    [JsonPropertyName(nameof(VersionInfo))]
    public string? VersionInfo { get; set; }
}
