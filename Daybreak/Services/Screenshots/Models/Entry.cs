using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Services.Screenshots.Models;
internal sealed class Entry
{
    public Map? Map { get; init; }
    public string? Url { get; init; }
    public string? Credit { get; init; }
    public int? StartIndex { get; init; }
    public int? Count { get; init; }
    public string? IdFormat { get; init; } = "D2";
}
