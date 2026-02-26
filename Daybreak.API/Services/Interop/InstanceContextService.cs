using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;

namespace Daybreak.API.Services.Interop;

public sealed unsafe class InstanceContextService
{
    public WrappedPointer<AreaInfo> GetAreaInfo() => GWCA.GW.Map.GetMapInfo(0);

    public ServerRegion GetServerRegion() => (Daybreak.API.Interop.GuildWars.ServerRegion)GWCA.GW.Map.GetRegion();

    public InstanceType GetInstanceType() => (Daybreak.API.Interop.GuildWars.InstanceType)GWCA.GW.Map.GetInstanceType();
}
