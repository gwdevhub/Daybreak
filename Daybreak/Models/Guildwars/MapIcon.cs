namespace Daybreak.Models.Guildwars;

public readonly struct MapIcon : IPositionalEntity
{
    public Position? Position { get; init; }
    public GuildwarsIcon? Icon { get; init; }
}
