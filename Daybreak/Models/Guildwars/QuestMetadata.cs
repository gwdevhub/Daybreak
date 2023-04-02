namespace Daybreak.Models.Guildwars;

public readonly struct QuestMetadata : IPositionalEntity
{
    public Quest? Quest { get; init; }
    public Position? Position { get; init; }
    public Map? From { get; init; }
    public Map? To { get; init; }
}
