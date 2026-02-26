using Daybreak.API.Interop;
using static Daybreak.API.Interop.GWCA.GW.Constants;

namespace Daybreak.API.Services.Interop;

public sealed class PartyContextService
{
    public bool AddHero(uint heroId) => GWCA.GW.PartyMgr.AddHero((HeroID)heroId);

    public bool KickHero(uint heroId) => GWCA.GW.PartyMgr.KickHero((HeroID)heroId);

    public bool KickAllHeroes() => GWCA.GW.PartyMgr.KickAllHeroes();

    public bool LeaveParty() => GWCA.GW.PartyMgr.LeaveParty();
}
