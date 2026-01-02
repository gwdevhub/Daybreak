namespace Daybreak.Shared.Models.Builds;

public interface IBuildEntry
{
    public DateTime CreationTime { get; set; }
    public Dictionary<string, string>? Metadata { get; set; }
    public string? SourceUrl { get; set; }
    public string? PreviousName { get; set; }
    public string? Name { get; set; }
}
