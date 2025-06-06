using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using System.Core.Extensions;
using System.Extensions.Core;
using System.Runtime.InteropServices;
using System.Security;

namespace Daybreak.API.Services.Interop;

public sealed class UIContextService
    : IAddressHealthService
{
    private const string CreateHashFromStringMask = "xxxxxxx";
    private const int CreateHashFromStringOffset = 0x7;
    private static readonly byte[] CreateHashFromStringSeq = [0x85, 0xC0, 0x74, 0x0D, 0x6A, 0xFF, 0x50];
    private const string FrameArrayFile = "\\Code\\Engine\\Frame\\FrMsg.cpp";
    private const string FrameArrayAssertion = "frame";

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint CreateHashFromStringFunc([MarshalAs(UnmanagedType.LPWStr)] string value, int seed);

    private readonly MemoryScanningService memoryScanningService;
    private readonly GWDelegateCache<CreateHashFromStringFunc> createHashFromString;
    private readonly GWAddressCache frameArrayAddressCache;
    private readonly ILogger<UIContextService> logger;

    public UIContextService(
        MemoryScanningService memoryScanningService,
        ILogger<UIContextService> logger)
    {
        this.logger = logger.ThrowIfNull();
        this.memoryScanningService = memoryScanningService.ThrowIfNull();
        this.createHashFromString = new GWDelegateCache<CreateHashFromStringFunc>(new GWAddressCache(() => this.memoryScanningService.ToFunctionStart(
                this.memoryScanningService.FindAddress(CreateHashFromStringSeq, CreateHashFromStringMask, CreateHashFromStringOffset))));
        this.frameArrayAddressCache = new GWAddressCache(() => this.memoryScanningService.FindAssertion(FrameArrayFile, FrameArrayAssertion, 0, -0x14));
    }

    public List<AddressState> GetAddressStates()
    {
        return
        [
            new()
            {
                Name = nameof(this.createHashFromString),
                Address = this.createHashFromString.Cache.GetAddress() ?? 0U
            },
            new()
            {
                Name = nameof(this.frameArrayAddressCache),
                Address = this.frameArrayAddressCache.GetAddress() ?? 0U
            }
        ];
    }

    public unsafe GuildWarsArray<WrappedPointer<Frame>>* GetFrameArray()
    {
        if (this.frameArrayAddressCache.GetAddress() is not nuint address)
        {
            return null;
        }

        return *(GuildWarsArray<WrappedPointer<Frame>>**)address;
    }

    public uint CreateHashFromString(string value, int seed)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (this.createHashFromString.GetDelegate() is not CreateHashFromStringFunc del)
        {
            scopedLogger.LogError("Failed to get CreateHashFromString delegate");
            return 0;
        }

        return del(value, seed);
    }
}
