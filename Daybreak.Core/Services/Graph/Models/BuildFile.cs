namespace Daybreak.Services.Graph.Models;

public sealed class BuildFile
{
    public string? TemplateCode { get; set; }
    public string? FileName { get; set; }
    public string? SourceUrl { get; set; }
    public Dictionary<string, string>? Metadata { get; set; }
}
