using System.Text.Json.Serialization;

namespace Daybreak.Services.Graph.Models;

public sealed class FolderItem
{
    [JsonPropertyName("value")]
    public List<FileItem>? Files { get; set; }
}
