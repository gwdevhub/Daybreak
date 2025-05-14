using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Shared.Models.FocusView;

public sealed class QuestEntry : QuestLogEntry
{
    public override string Title { get; init; } = string.Empty;
    public Quest? Quest { get; init; }
}
