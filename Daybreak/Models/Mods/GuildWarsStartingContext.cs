namespace Daybreak.Models.Mods;
public sealed class GuildWarsStartingContext
{
    public ApplicationLauncherContext ApplicationLauncherContext { get; init; }
    public bool CancelStartup { get; set; }
}
