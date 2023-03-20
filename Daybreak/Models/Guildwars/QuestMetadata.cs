namespace Daybreak.Models.Guildwars;

public readonly struct QuestMetadata
{
    public Quest? Quest { get; init; }
    public Map? From { get; init; }
    public Map? To { get; init; }
}
