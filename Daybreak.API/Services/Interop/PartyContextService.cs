using Daybreak.API.Interop;

namespace Daybreak.API.Services.Interop;

public sealed class PartyContextService
{
    public bool AddHero(uint heroId)
    {
        return GWCA.GW.PartyMgr.AddHero((int)heroId);
    }

    public bool KickHero(uint heroId)
    {
        return GWCA.GW.PartyMgr.KickHero((int)heroId);
    }

    public bool KickAllHeroes() => this.KickHero(0x26);

    public bool LeaveParty()
    {
        return GWCA.GW.PartyMgr.LeaveParty();
    }
}
