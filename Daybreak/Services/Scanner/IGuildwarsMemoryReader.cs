using Daybreak.Models;
using Daybreak.Models.Guildwars;
using System.Threading.Tasks;

namespace Daybreak.Services.Scanner;

public interface IGuildwarsMemoryReader
{
    Task EnsureInitialized();
    Task<GameData?> ReadGameData();
    Task<PathingData?> ReadPathingData();
    Task<PathingMetadata?> ReadPathingMetaData();
    void Stop();
}
