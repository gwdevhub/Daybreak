using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Models.LaunchConfigurations;

namespace Daybreak.Shared.Models.FocusView;
/// <summary>
/// XD Name
/// </summary>
internal sealed class PlayerContextMenuContext
{
    public GuildWarsApplicationLaunchContext? GuildWarsApplicationLaunchContext { get; set; }
    public PlayerInformation? Player { get; set; }
}
