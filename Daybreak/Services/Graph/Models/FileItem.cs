using Newtonsoft.Json;
using System;

namespace Daybreak.Services.Graph.Models;

public sealed class FileItem
{
    [JsonProperty("@microsoft.graph.downloadUrl")]
    public string DownloadUrl { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("lastModifiedDateTime")]
    public DateTime LastModifiedDateTime { get; set; }
}
