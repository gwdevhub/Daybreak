using Daybreak.Shared.Models.Builds;

namespace Daybreak.Shared.Models;

public sealed class BuildWithTemplateCode
{
    public IBuildEntry? Build { get; set; }
    public string? TemplateCode { get; set; }
}
