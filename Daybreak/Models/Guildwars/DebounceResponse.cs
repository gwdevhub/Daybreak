using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;

public sealed class DebounceResponse
{
    public MainPlayerInformation MainPlayer { get; init; } = default;
    public IEnumerable<WorldPlayerInformation> WorldPlayers { get; init; } = default!;
    public IEnumerable<PlayerInformation> Party { get; init; } = default!;
    public IEnumerable<LivingEntity> LivingEntities { get; init; } = default!;
}
