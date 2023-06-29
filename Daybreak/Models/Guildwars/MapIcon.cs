namespace Daybreak.Models.Guildwars;

public sealed class MapIcon : IPositionalEntity
{
    public Position? Position { get; init; }
    public GuildwarsIcon? Icon { get; init; }
    public Affiliation? Affiliation { get; init; }
}
