using Daybreak.Shared.Models;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Services.Mods;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Shared.Services.Api;
public interface IDaybreakApiService : IModService
{
    Task<DaybreakAPIContext?> GetDaybreakApiContext(GuildWarsApplicationLaunchContext launchContext, CancellationToken cancellationToken);
    Task<DaybreakAPIContext?> GetDaybreakApiContext(Process guildWarsProcess, CancellationToken cancellationToken);
}
