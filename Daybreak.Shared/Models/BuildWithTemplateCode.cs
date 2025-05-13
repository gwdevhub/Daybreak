using Daybreak.Models.Builds;

namespace Daybreak.Models;

public sealed class BuildWithTemplateCode
{
    public IBuildEntry? Build { get; set; }
    public string? TemplateCode { get; set; }
}
