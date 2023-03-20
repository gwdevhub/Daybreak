using Daybreak.Models;
using Daybreak.Models.Guildwars;

namespace Daybreak.Services.Scanner;

public interface IGuildwarsEntityDebouncer
{
    DebounceResponse DebounceEntities(GameData gameData);

    void ClearCaches();
}
