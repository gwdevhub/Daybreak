using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;

namespace Daybreak.API.Services.Interop;

public sealed unsafe class InstanceContextService
{
    public WrappedPointer<AreaInfo> GetAreaInfo()
    {
        return GWCA.GW.Map.GetMapInfo(0);
    }

    public ServerRegion GetServerRegion()
    {
        return GWCA.GW.Map.GetRegion();
    }

    public InstanceType GetInstanceType()
    {
        return GWCA.GW.Map.GetInstanceType();
    }
}
