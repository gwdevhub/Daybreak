using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;

namespace Daybreak.API.Services.Interop;

public unsafe sealed class GameContextService
{

    public WrappedPointer<GameContext> GetGameContext() => GWCA.GW.GetGameContext();

    public WrappedPointer<PreGameContext> GetPreGameContext() => GWCA.GW.GetPreGameContext();

    public WrappedPointer<GuildWarsArray<CharacterInformation>> GetAvailableChars() => GWCA.GW.GetAvailableChars();

    public bool IsMapLoaded() => GWCA.GW.Map.GetIsMapLoaded();
}
