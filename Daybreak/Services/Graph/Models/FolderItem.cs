﻿using Newtonsoft.Json;

namespace Daybreak.Services.Graph.Models;

public sealed class FolderItem
{
    [JsonProperty("value")]
    public List<FileItem>? Files { get; set; }
}
