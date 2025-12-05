namespace Daybreak.Shared.Models.Async;
public sealed record ProgressUpdate(Percentage Percentage, string? StatusMessage)
{
}
