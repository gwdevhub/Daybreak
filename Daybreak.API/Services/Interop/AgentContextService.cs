using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;

namespace Daybreak.API.Services.Interop;

public unsafe sealed class AgentContextService
{
    public WrappedPointer<GuildWarsArray<WrappedPointer<AgentContext>>> GetAgentArray()
    {
        // GuildWarsArray<nint> and GuildWarsArray<WrappedPointer<AgentContext>> have the same memory layout
        // since both nint and WrappedPointer<T> are pointer-sized
        return (GuildWarsArray<WrappedPointer<AgentContext>>*)GWCA.GW.Agents.GetAgentArray();
    }

    public uint GetPlayerAgentId()
    {
        return GWCA.GW.PlayerMgr.GetPlayerAgentId(0);
    }
}
