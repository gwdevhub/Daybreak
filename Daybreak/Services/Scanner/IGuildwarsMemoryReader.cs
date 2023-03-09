using Daybreak.Models;
using System.Threading.Tasks;

namespace Daybreak.Services.Scanner;

public interface IGuildwarsMemoryReader
{
    Task EnsureInitialized();
    Task<GameData?> ReadGameData();
    void Stop();
}
