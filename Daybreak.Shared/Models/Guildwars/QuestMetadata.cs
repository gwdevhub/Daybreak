namespace Daybreak.Shared.Models.Guildwars;

public sealed class QuestMetadata
{
    public Quest? Quest { get; init; }
    public Position? Position { get; init; }
    public Map? From { get; init; }
    public Map? To { get; init; }
    public float RotationAngle { get; } = 0f;
}
