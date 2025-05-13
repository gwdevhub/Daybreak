namespace Daybreak.Models.Mods;
public sealed class GuildWarsCreatedContext
{
    public ApplicationLauncherContext ApplicationLauncherContext { get; init; }
    public bool CancelStartup { get; set; }
}
