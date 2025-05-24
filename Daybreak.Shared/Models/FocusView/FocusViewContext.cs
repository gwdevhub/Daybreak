using Daybreak.Shared.Models.LaunchConfigurations;

namespace Daybreak.Shared.Models.FocusView;

public sealed class FocusViewContext
{
    public required DaybreakAPIContext ApiContext { get; init; }
    public required GuildWarsApplicationLaunchContext LaunchContext { get; init; }
}
