using Daybreak.Shared.Models.LaunchConfigurations;

namespace Daybreak.Shared.Services.Api;
public interface IAttachedApiAccessor
{
    GuildWarsApplicationLaunchContext? LaunchContext { get; }
    ScopedApiContext? ApiContext { get; }
}
