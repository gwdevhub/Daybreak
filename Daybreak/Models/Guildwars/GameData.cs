using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;

public sealed class GameData
{
    public bool Valid { get; init; }
    public MainPlayerInformation? MainPlayer { get; init; }
    public List<PlayerInformation>? Party { get; init; }
    public List<WorldPlayerInformation>? WorldPlayers { get; init; }
    public List<LivingEntity>? LivingEntities { get; init; }
    public List<MapIcon>? MapIcons { get; init; }
    public int? CurrentTargetId { get; init; }
}
