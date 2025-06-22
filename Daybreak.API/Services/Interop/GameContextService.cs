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

    private const string AvailableCharsMask = "xx????xxxxxxx";
    private const int AvailableCharsOffset = 0x2;
    private static readonly byte[] AvailableCharsPattern = [0x8B, 0x35, 0x00, 0x00, 0x00, 0x00, 0x57, 0x69, 0xF8, 0x84, 0x00, 0x00, 0x00];

    private readonly MemoryScanningService memoryScanningService;
    private readonly GWAddressCache baseAddress;
    private readonly GWAddressCache preGameContextAddress;
    private readonly GWAddressCache availableCharsAddress;
    private readonly ILogger<GameContextService> logger;

    public GameContextService(
        MemoryScanningService memoryScanningService,
        ILogger<GameContextService> logger)
    {
        this.logger = logger.ThrowIfNull();
        this.memoryScanningService = memoryScanningService.ThrowIfNull();
        this.baseAddress = new GWAddressCache(this.GetBaseAddress);
        this.preGameContextAddress = new GWAddressCache(this.GetPreGameContextAddress);
        this.availableCharsAddress = new GWAddressCache(this.GetAvailableCharactersAddress);
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
            new AddressState
            {
                Address = this.availableCharsAddress.GetAddress() ?? 0,
                Name = nameof(this.availableCharsAddress),
            }
        ];
    }

    public WrappedPointer<GameContext> GetGameContext()
    {
        var gameContextAddress = this.GetGameContextAddress();
        if (gameContextAddress is 0x0)
        {
            this.logger.LogError("Failed to get game context address");
            return null;
        }

        return (GameContext*)gameContextAddress;
    }

    public WrappedPointer<PreGameContext> GetPreGameContext()
    {
        var preGameContextAddress = this.preGameContextAddress.GetAddress();
        if (preGameContextAddress is null or 0x0)
        {
            this.logger.LogError("Failed to get pre-game context address");
            return null;
        }

        return *(PreGameContext**)preGameContextAddress;
    }

    public WrappedPointer<GuildWarsArray<CharInfoContext>> GetAvailableChars()
    {
        var availableCharsAddress = this.availableCharsAddress.GetAddress();
        if (availableCharsAddress is null or 0x0)
        {
            this.logger.LogError("Failed to get available chars address");
            return null;
        }

        return *(GuildWarsArray<CharInfoContext>**)availableCharsAddress;
    }

    public bool IsMapLoaded()
    {
        var gameContext = this.GetGameContext();
        if (gameContext.IsNull)
        {
            return false;
        }

        if (gameContext.Pointer->MapContext is null)
        {
            return false;
        }

        return true;
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

    private unsafe nuint GetPreGameContextAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var preGameContextAddress = this.memoryScanningService.FindAssertion(PreGameContextFile, PreGameContextAssertion, 0, PreGameContextOffset);
        if (preGameContextAddress is 0)
        {
            scopedLogger.LogError("Failed to find pre-game context address");
            return 0U;
        }

        preGameContextAddress = *(nuint*)preGameContextAddress;
        scopedLogger.LogInformation("Pre-game context address: 0x{address:X8}", preGameContextAddress);
        return preGameContextAddress;
    }

    private unsafe nuint GetAvailableCharactersAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var availableCharsAddress = this.memoryScanningService.FindAddress(AvailableCharsPattern, AvailableCharsMask, AvailableCharsOffset);
        if (availableCharsAddress is 0)
        {
            scopedLogger.LogError("Failed to find available chars address");
            return 0U;
        }

        scopedLogger.LogInformation("Available chars address: 0x{address:X8}", availableCharsAddress);
        return availableCharsAddress;
    }
}
