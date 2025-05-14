using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Shared.Models;
public sealed class MainPlayerResourceContext
{
    public UserData? User { get; init; }
    public SessionData? Session { get; init; }
    public MainPlayerData? Player { get; init; }
}
