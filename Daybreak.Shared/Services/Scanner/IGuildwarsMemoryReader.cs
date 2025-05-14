using Daybreak.Shared.Models.Guildwars;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Shared.Services.Scanner;

public interface IGuildwarsMemoryReader
{
    Task<bool> IsInitialized(uint processId, CancellationToken cancellationToken);
    Task EnsureInitialized(uint processId, CancellationToken cancellationToken);
    Task<LoginData?> ReadLoginData(CancellationToken cancellationToken);
    Task<WorldData?> ReadWorldData(CancellationToken cancellationToken);
    Task<UserData?> ReadUserData(CancellationToken cancellationToken);
    Task<SessionData?> ReadSessionData(CancellationToken cancellationToken);
    Task<MainPlayerData?> ReadMainPlayerData(CancellationToken cancellationToken);
    Task<TeamBuildData?> ReadTeamBuildData(CancellationToken cancellationToken);
    void Stop();
}
