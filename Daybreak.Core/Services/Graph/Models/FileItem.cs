using System.Text.Json.Serialization;

namespace Daybreak.Services.Graph.Models;

public sealed class FileItem
{
    [JsonPropertyName("@microsoft.graph.downloadUrl")]
    public string? DownloadUrl { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("lastModifiedDateTime")]
    public DateTime LastModifiedDateTime { get; set; }
}
