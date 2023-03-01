namespace Daybreak.Models.Guildwars;

public sealed class QuestMetadata
{
    public Quest? Quest { get; init; }
    public Map? From { get; init; }
    public Map? To { get; init; }
}
