using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Shared.Services.Wiki;
public interface IWikiService
{
    Task<SkillDescription?> GetSkillDescription(Skill skill, CancellationToken cancellationToken);
}
