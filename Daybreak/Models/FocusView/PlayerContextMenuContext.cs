using Daybreak.Models.Guildwars;
using Daybreak.Models.LaunchConfigurations;

namespace Daybreak.Models.FocusView;
/// <summary>
/// XD Name
/// </summary>
internal sealed class PlayerContextMenuContext
{
    public GuildWarsApplicationLaunchContext? GuildWarsApplicationLaunchContext { get; set; }
    public PlayerInformation? Player { get; set; }
}
