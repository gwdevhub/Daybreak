using Daybreak.API.Interop;

namespace Daybreak.API.Services.Interop;

public sealed class PartyContextService
{
    public bool AddHero(uint heroId) => GWCA.GW.PartyMgr.AddHero((int)heroId);

    public bool KickHero(uint heroId) => GWCA.GW.PartyMgr.KickHero((int)heroId);

    public bool KickAllHeroes() => GWCA.GW.PartyMgr.KickAllHeroes();

    public bool LeaveParty() => GWCA.GW.PartyMgr.LeaveParty();
}
