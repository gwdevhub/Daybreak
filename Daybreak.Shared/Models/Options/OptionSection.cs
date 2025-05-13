using System;

namespace Daybreak.Models.Options;
public sealed class OptionSection
{
    public string? Name { get; init; }
    public string? Tooltip { get; init; }
    public Type? Type { get; init; }
}
