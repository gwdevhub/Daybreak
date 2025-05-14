namespace Daybreak.Shared.Models.Browser;

public sealed class DownloadPayload
{
    public string? ResultingFilePath { get; init; }
    public bool CanDownload { get; set; }
}
