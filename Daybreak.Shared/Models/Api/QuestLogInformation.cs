using System.Collections.Generic;

namespace Daybreak.Shared.Models.Api;
public sealed record QuestLogInformation(uint CurrentQuestId, IReadOnlyList<QuestInformation> Quests)
{
}
