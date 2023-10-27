using Daybreak.Models.Guildwars;
using Daybreak.Models.LaunchConfigurations;

namespace Daybreak.Models.FocusView;
/// <summary>
/// XD Name
/// </summary>
internal sealed class LivingEntityContextMenuContext
{
    public GuildWarsApplicationLaunchContext? GuildWarsApplicationLaunchContext { get; set; }
    public LivingEntity? LivingEntity { get; set; }
}
