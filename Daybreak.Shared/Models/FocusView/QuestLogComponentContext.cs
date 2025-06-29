using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Shared.Models.FocusView;
public sealed class QuestLogComponentContext
{
    public required QuestMetadata? CurrentQuest { get; init; }
    public required List<QuestMetadata> Quests { get; init; }
}
