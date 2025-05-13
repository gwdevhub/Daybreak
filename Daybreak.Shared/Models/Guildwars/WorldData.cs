namespace Daybreak.Models.Guildwars;

public sealed class WorldData
{
    public Campaign? Campaign { get; init; }
    public Continent? Continent { get; init; }
    public Region? Region { get; init; }
    public Map? Map { get; init; }
}
