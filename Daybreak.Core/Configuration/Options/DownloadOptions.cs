using System.Text.Json.Serialization;
using Daybreak.Shared.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Downloads")]
internal sealed class DownloadOptions
{
    [JsonPropertyName(nameof(HeaderTimeout))]
    [OptionName(
        Name = "Header Timeout",
        Description = "Amount of seconds Daybreak will wait for a download to respond before giving up on the attempt"
    )]
    [OptionRange<double>(MinValue = 5, MaxValue = 300)]
    public double HeaderTimeout { get; set; } = 30;

    [JsonPropertyName(nameof(ChunkTimeout))]
    [OptionName(
        Name = "Chunk Timeout",
        Description = "Amount of seconds Daybreak will wait for the next chunk of a download before giving up on the attempt"
    )]
    [OptionRange<double>(MinValue = 5, MaxValue = 300)]
    public double ChunkTimeout { get; set; } = 30;

    [JsonPropertyName(nameof(Retries))]
    [OptionName(
        Name = "Retries",
        Description = "Amount of times Daybreak will retry a failed download before reporting it as failed"
    )]
    [OptionRange<int>(MinValue = 0, MaxValue = 10)]
    public int Retries { get; set; } = 3;

    [JsonPropertyName(nameof(RetryDelay))]
    [OptionName(
        Name = "Retry Delay",
        Description = "Amount of seconds Daybreak will wait before retrying a failed download"
    )]
    [OptionRange<double>(MinValue = 0, MaxValue = 60)]
    public double RetryDelay { get; set; } = 2;
}
