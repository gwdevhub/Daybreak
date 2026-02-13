using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Github;

public sealed class GithubContentItem
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("size")]
    public long Size { get; set; }

    [JsonPropertyName("download_url")]
    public string? DownloadUrl { get; set; }

    public bool IsFile => this.Type == "file";

    public bool IsDirectory => this.Type == "dir";
}
