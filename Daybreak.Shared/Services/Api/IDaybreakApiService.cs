using Daybreak.Shared.Models;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Services.Mods;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Shared.Services.Api;
public interface IDaybreakApiService : IModService
{
    Task<ScopedApiContext?> AttachDaybreakApiContext(GuildWarsApplicationLaunchContext launchContext, ScopedApiContext scopedApiContext, CancellationToken cancellationToken);
    Task<ScopedApiContext?> AttachDaybreakApiContext(GuildWarsApplicationLaunchContext launchContext, CancellationToken cancellationToken);
    Task<ScopedApiContext?> FindDaybreakApiContextByCredentials(LoginCredentials loginCredentials, CancellationToken cancellationToken);
    Task<ScopedApiContext?> GetDaybreakApiContext(Process guildWarsProcess, CancellationToken cancellationToken);
}
