using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Services.Api;

namespace Daybreak.Shared.Models.FocusView;

public sealed class FocusViewContext
{
    public required ScopedApiContext ApiContext { get; init; }
    public required GuildWarsApplicationLaunchContext LaunchContext { get; init; }
}
