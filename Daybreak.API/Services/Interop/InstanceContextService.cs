using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;

namespace Daybreak.API.Services.Interop;

public sealed unsafe class InstanceContextService
{
    public WrappedPointer<AreaInfo> GetAreaInfo() => GWCA.GW.Map.GetMapInfo(0);

    public ServerRegion GetServerRegion() => GWCA.GW.Map.GetRegion();

    public InstanceType GetInstanceType() => GWCA.GW.Map.GetInstanceType();
}
