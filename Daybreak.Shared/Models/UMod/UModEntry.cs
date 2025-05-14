namespace Daybreak.Shared.Models.UMod;

public sealed class UModEntry
{
    public string? Name { get; init; }
    public string? PathToFile { get; init; }
    public bool Imported { get; init; }
    public bool Enabled { get; set; }
}
