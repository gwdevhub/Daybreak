namespace Daybreak.Models.Guildwars;

public sealed class QuestMetadata : IPositionalEntity
{
    public Quest? Quest { get; init; }
    public Position? Position { get; init; }
    public Map? From { get; init; }
    public Map? To { get; init; }
}
