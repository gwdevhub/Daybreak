using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;

namespace Daybreak.API.Services.Interop;

public unsafe sealed class GameContextService(
    MemoryScanningService memoryScanningService)
{
    private const string AvailableCharsMask = "xx????xxxxxxx";
    private const int AvailableCharsOffset = 0x2;
    private static readonly byte[] AvailableCharsPattern = [0x8B, 0x35, 0x00, 0x00, 0x00, 0x00, 0x57, 0x69, 0xF8, 0x84, 0x00, 0x00, 0x00];

    private readonly GWAddressCache getAvailableCharsCache = new(
        () => memoryScanningService.FindAddress(AvailableCharsPattern, AvailableCharsMask, AvailableCharsOffset)
    );

    public WrappedPointer<GameContext> GetGameContext()
    {
        return GWCA.GW.GetGameContext();
    }

    public WrappedPointer<PreGameContext> GetPreGameContext()
    {
        return GWCA.GW.GetPreGameContext();
    }

    public WrappedPointer<GuildWarsArray<CharInfoContext>> GetAvailableChars()
    {
        var address = this.getAvailableCharsCache.GetAddress();
        if (address is null or 0)
        {
            return default;
        }

        return *(GuildWarsArray<CharInfoContext>**)(void*)address;
    }

    public bool IsMapLoaded()
    {
        return GWCA.GW.Map.GetIsMapLoaded();
    }
}
