using Daybreak.Shared.Services.Mods;

namespace Daybreak.Models;

public sealed class ModListEntry
{
    public required IModService ModService { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required bool IsVisible { get; init; }
    public required bool CanManage { get; init; }
    public required bool CanUninstall { get; init; }

    public bool IsEnabled { get; set; }
    public bool IsInstalled { get; set; }
    public bool CanUpdate { get; set; }
    public bool Loading { get; set; }
}
