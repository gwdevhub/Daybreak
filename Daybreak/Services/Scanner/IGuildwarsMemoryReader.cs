using Daybreak.Models;
using Daybreak.Models.Guildwars;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Scanner;

public interface IGuildwarsMemoryReader
{
    Task EnsureInitialized(CancellationToken cancellationToken);
    Task<GameData?> ReadGameData(CancellationToken cancellationToken);
    Task<PathingData?> ReadPathingData(CancellationToken cancellationToken);
    Task<PathingMetadata?> ReadPathingMetaData(CancellationToken cancellationToken);
    void Stop();
}
