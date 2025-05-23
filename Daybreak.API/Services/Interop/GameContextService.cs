using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using System.Core.Extensions;
using System.Extensions.Core;

namespace Daybreak.API.Services.Interop;

public unsafe sealed class GameContextService : IAddressHealthService
{
    private const string BaseAddressMask = "xxxxxxx";
    private const int BaseAddressOffset = +7;
    private static readonly byte[] BaseAddressPattern = [0x50, 0x6A, 0x0F, 0x6A, 0x00, 0xFF, 0x35];

    private const string PreGameContextFile = "UiPregame.cpp";
    private const string PreGameContextAssertion = "!s_scene";
    private const int PreGameContextOffset = 0x34;

    private readonly MemoryScanningService memoryScanningService;
    private readonly GWAddressCache baseAddress;
    private readonly GWAddressCache preGameContextAddress;
    private readonly ILogger<GameContextService> logger;

    public GameContextService(
        MemoryScanningService memoryScanningService,
        ILogger<GameContextService> logger)
    {
        this.logger = logger.ThrowIfNull();
        this.memoryScanningService = memoryScanningService.ThrowIfNull();
        this.baseAddress = new GWAddressCache(this.GetBaseAddress);
        this.preGameContextAddress = new GWAddressCache(this.GetPreGameContextAddress);
    }

    public List<AddressState> GetAddressStates()
    {
        return [
            new AddressState
            {
                Address = this.baseAddress.GetAddress() ?? 0,
                Name = nameof(this.baseAddress),
            },
            new AddressState
            {
                Address = this.preGameContextAddress.GetAddress() ?? 0,
                Name = nameof(this.preGameContextAddress),
            },
        ];
    }

    public GameContext* GetGameContext()
    {
        var gameContextAddress = this.GetGameContextAddress();
        if (gameContextAddress is 0x0)
        {
            this.logger.LogError("Failed to get game context address");
            return null;
        }

        return (GameContext*)gameContextAddress;
    }

    public PreGameContext* GetPreGameContext()
    {
        var preGameContextAddress = this.preGameContextAddress.GetAddress();
        if (preGameContextAddress is null or 0x0)
        {
            this.logger.LogError("Failed to get pre-game context address");
            return null;
        }

        return *(PreGameContext**)preGameContextAddress;
    }

    private nuint GetGameContextAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (this.baseAddress.GetAddress() is not nuint basePtr)
        {
            scopedLogger.LogError("Failed to get base address");
            return default;
        }

        nuint** baseContext = *(nuint***)basePtr;
        if (baseContext is null)
        {
            scopedLogger.LogError("Failed to get base context address");
            return default;
        }

        var globalContextPtr = baseContext[0x6];
        if (globalContextPtr is null)
        {
            scopedLogger.LogError("Failed to get game context address");
            return default;
        }

        return (nuint)globalContextPtr;
    }

    private nuint GetBaseAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var baseAddress = this.memoryScanningService.FindAndResolveAddress(BaseAddressPattern, BaseAddressMask, BaseAddressOffset);
        if (baseAddress is 0)
        {
            scopedLogger.LogError("Failed to find base address");
            return 0U;
        }

        scopedLogger.LogInformation("Base address: 0x{address:X8}", baseAddress);
        return baseAddress;
    }

    private nuint GetPreGameContextAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var preGameContextAddress = this.memoryScanningService.FindAssertion(PreGameContextFile, PreGameContextAssertion, 0, PreGameContextOffset);
        if (preGameContextAddress is 0)
        {
            scopedLogger.LogError("Failed to find pre-game context address");
            return 0U;
        }

        scopedLogger.LogInformation("Pre-game context address: 0x{address:X8}", preGameContextAddress);
        return preGameContextAddress;
    }
}
