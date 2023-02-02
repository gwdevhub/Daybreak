namespace Daybreak.Models;

public sealed class SynchronizationBuild
{
    public string? Name { get; set; }
    public string? TemplateCode { get; set; }
    public bool Changed { get; set; }
}
