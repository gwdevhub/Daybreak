using Daybreak.Models.Guildwars;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Scanner;

public interface IGuildwarsMemoryReader
{
    Task EnsureInitialized(uint processId, CancellationToken cancellationToken);
    Task<LoginData?> ReadLoginData(CancellationToken cancellationToken);
    Task<GameData?> ReadGameData(CancellationToken cancellationToken);
    Task<PathingData?> ReadPathingData(CancellationToken cancellationToken);
    Task<PathingMetadata?> ReadPathingMetaData(CancellationToken cancellationToken);
    Task<InventoryData?> ReadInventoryData(CancellationToken cancellationToken);
    Task<WorldData?> ReadWorldData(CancellationToken cancellationToken);
    Task<UserData?> ReadUserData(CancellationToken cancellationToken);
    Task<SessionData?> ReadSessionData(CancellationToken cancellationToken);
    Task<MainPlayerData?> ReadMainPlayerData(CancellationToken cancellationToken);
    Task<PreGameData?> ReadPreGameData(CancellationToken cancellationToken);
    Task<GameState?> ReadGameState(CancellationToken cancellationToken);
    Task<string?> GetEntityName(IEntity entity, CancellationToken cancellationToken);
    Task<string?> GetItemName(int id, List<uint> modifiers, CancellationToken cancellationToken);
    void Stop();
}
