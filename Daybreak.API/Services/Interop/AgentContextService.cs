using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;

namespace Daybreak.API.Services.Interop;

public unsafe sealed class AgentContextService
{
    public uint GetPlayerAgentId() => GWCA.GW.PlayerMgr.GetPlayerAgentId(0);

    public Agent* GetAgentById(uint agentId) => GWCA.GW.Agents.GetAgentByID(agentId);
}
