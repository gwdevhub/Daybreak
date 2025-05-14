using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Shared.Models.FocusView;
public sealed class QuestLocationEntry : QuestLogEntry
{
    public override string Title { get; init; } = string.Empty;
    public Map? Map { get; init; }
}
