using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Models;
public sealed record SkillSnippetContext((int PosX, int PosY) MousePosition, Skill Skill)
{
}
