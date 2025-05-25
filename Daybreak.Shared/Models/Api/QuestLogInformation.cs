using System.Collections.Generic;

namespace Daybreak.Shared.Models.Api;
public sealed class QuestLogInformation
{
    public uint CurrentQuestId { get; init; }
    public List<QuestInformation> Quests { get; init; } = [];
}
