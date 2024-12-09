using System;
using System.Collections.Generic;

namespace Daybreak.Models.Builds;
public interface IBuildEntry
{
    public DateTimeOffset CreationTime { get; set; }
    public Dictionary<string, string>? Metadata { get; set; }
    public string? SourceUrl { get; set; }
    public string? PreviousName { get; set; }
    public string? Name { get; set; }
}
