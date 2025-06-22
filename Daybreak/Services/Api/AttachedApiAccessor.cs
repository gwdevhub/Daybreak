using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Services.Api;

namespace Daybreak.Services.Api;
public sealed class AttachedApiAccessor : IAttachedApiAccessor
{
    public GuildWarsApplicationLaunchContext? LaunchContext { get; internal set; }
    public ScopedApiContext? ApiContext { get; internal set; }
}
