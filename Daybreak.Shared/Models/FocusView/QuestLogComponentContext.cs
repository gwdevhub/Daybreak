using Daybreak.Shared.Models.Guildwars;
using System.Collections.Generic;

namespace Daybreak.Shared.Models.FocusView;
public sealed class QuestLogComponentContext
{
    public required List<QuestMetadata> Quests { get; init; }
}
