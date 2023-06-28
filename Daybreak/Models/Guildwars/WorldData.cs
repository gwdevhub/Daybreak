namespace Daybreak.Models.Guildwars;

public readonly struct WorldData
{
    public Campaign? Campaign { get; init; }
    public Continent? Continent { get; init; }
    public Region? Region { get; init; }
    public Map? Map { get; init; }
}
