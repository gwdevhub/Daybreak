using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;

namespace Daybreak.API.Services.Interop;

public sealed class SkillbarContextService
{
    public unsafe void LoadBuild(uint agentId, SkillTemplate* template) => GWCA.GW.SkillbarMgr.LoadSkillTemplate(agentId, template);
}
