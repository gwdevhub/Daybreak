using Daybreak.Models.Guildwars;

namespace Daybreak.Models;
public sealed class MainPlayerResourceContext
{
    public UserData? User { get; init; }
    public SessionData? Session { get; init; }
    public MainPlayerData? Player { get; init; }
}
