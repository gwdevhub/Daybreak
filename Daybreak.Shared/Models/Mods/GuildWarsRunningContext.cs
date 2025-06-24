using System.Collections.Generic;

namespace Daybreak.Shared.Models.Mods;
public sealed class GuildWarsRunningContext
{
    public required ApplicationLauncherContext ApplicationLauncherContext { get; init; }
    public required IReadOnlyList<string> LoadedModules { get; init; }
}
