using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Shared.Models.Builds;
public sealed class PartyMemberEntry
{
    public required SingleBuildEntry Build { get; init; }
    public required Hero? Hero { get; init; }
    public required HeroBehavior Behavior { get; init; }
}
