using Daybreak.API.Interop;
using Daybreak.API.Models;
using System.Core.Extensions;
using System.Extensions;
using static Daybreak.API.Models.UIPackets;

namespace Daybreak.API.Services.Interop;

public sealed class PartyContextService
        : IAddressHealthService
{
    private const string SearchButtonFile = "\\Code\\Gw\\Ui\\Game\\Party\\PtSearch.cpp";
    private const string SearchButtonAssertion = "m_activeList == LIST_HEROES";
    private const string WindowButtonAssertion = "selection.agentId";

    // Fastcall funcs do not work in .NET for x86 right now. https://github.com/dotnet/runtime/issues/113851
    // This fastcall equivalent is <void*, uint, UIPacket::kMouseAction*, void>
    private readonly GWFastCall<nint, uint, nint, GWFastCall.Void> searchButtonCallback;
    // This fastcall equivalent is <void*, uint, UIPacket::kMouseAction*, void>
    private readonly GWFastCall<nint, uint, nint, GWFastCall.Void> windowButtonCallback;

    private readonly MemoryScanningService memoryScanningService;
    private readonly ILogger<PartyContextService> logger;

    public PartyContextService(
        MemoryScanningService memoryScanningService,
        ILogger<PartyContextService> logger)
    {
        this.memoryScanningService = memoryScanningService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.searchButtonCallback = new(new GWAddressCache(() => this.memoryScanningService.ToFunctionStart(this.memoryScanningService.FindAssertion(SearchButtonFile, SearchButtonAssertion, 0, 0))));
        this.windowButtonCallback = new(new GWAddressCache(() => this.memoryScanningService.ToFunctionStart(this.memoryScanningService.FindUseOfString(WindowButtonAssertion))));
    }

    public List<AddressState> GetAddressStates()
    {
        unsafe
        {
            return
            [
                new AddressState
                {
                    Address = this.searchButtonCallback.Cache.GetAddress() ?? 0,
                    Name = nameof(this.searchButtonCallback)
                },
                new AddressState
                {
                    Address = this.windowButtonCallback.Cache.GetAddress() ?? 0,
                    Name = nameof(this.windowButtonCallback)
                }
            ];
        }
    }

    public unsafe bool CallSearchButtonCallback(void* ctx, uint edx, MouseAction* wparam)
    {
        this.searchButtonCallback.Invoke((nint)ctx, edx, (nint)wparam);
        return true;
    }

    public unsafe bool CallWindowButtonCallback(void* ctx, uint edx, MouseAction* wparam)
    {
        this.windowButtonCallback.Invoke((nint)ctx, edx, (nint)wparam);
        return true;
    }

    public unsafe bool AddHero(uint heroId)
    {
        var action = new MouseAction(
            frameId: 0,
            childOffsetId: 0x1,
            currentState: ActionState.MouseUp);

        var ctx = stackalloc uint[13];
        ctx[0xb] = 1;
        ctx[0x9] = heroId;
        return this.CallSearchButtonCallback(ctx, 2, &action);
    }

    public unsafe bool KickHero(uint heroId)
    {
        var action = new MouseAction(
            frameId: 0,
            childOffsetId: 0x6,
            currentState: ActionState.MouseUp);

        var ctx = stackalloc uint[13];
        ctx[0xb] = 1;
        ctx[0x9] = heroId;
        return this.CallSearchButtonCallback(ctx, 0, &action);
    }

    public bool KickAllHeroes() => this.KickHero(0x26);

    public unsafe bool LeaveParty()
    {
        var ctx = stackalloc uint[14];
        ctx[0xd] = 1;
        return this.CallWindowButtonCallback(ctx, 0, (MouseAction*)0);
    }
}
