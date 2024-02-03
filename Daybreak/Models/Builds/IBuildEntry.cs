namespace Daybreak.Models.Builds;
public interface IBuildEntry
{
    public string? SourceUrl { get; set; }
    public string? PreviousName { get; set; }
    public string? Name { get; set; }
}
