namespace Daybreak.Shared.Models.Builds;

public interface IBuildEntry
{
    DateTime CreationTime { get; set; }
    string? SourceUrl { get; set; }
    string? PreviousName { get; set; }
    string? Name { get; set; }
    int? ToolboxBuildId { get; set; }
    bool IsToolboxBuild { get; set; }
}
