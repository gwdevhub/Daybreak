using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using System.Core.Extensions;
using System.Extensions.Core;
using System.Runtime.InteropServices;

namespace Daybreak.API.Services.Interop;

public unsafe sealed class GameContextService : IAddressHealthService
{
    // TLS-based pattern to find GameContext getter function
    private const string GameContextTlsMask = "xx????xxxxxxxxxxx????x";
    private const int GameContextTlsOffset = 0;
    private static readonly byte[] GameContextTlsPattern = 
    [
        0x8B, 0x0D, 0x00, 0x00, 0x00, 0x00,  // mov ecx, [TlsIndexPtr]       - 6 bytes: xx????
        0x64, 0xA1, 0x2C, 0x00, 0x00, 0x00,  // mov eax, fs:[0000002C]       - 6 bytes: xxxxxx
        0x8B, 0x04, 0x88,                    // mov eax, [eax+ecx*4]         - 3 bytes: xxx
        0x8B, 0x80, 0x04, 0x00, 0x00, 0x00,  // mov eax, [eax+00000004]      - 6 bytes: xx????
        0xC3                                 // ret                          - 1 byte:  x
    ];                                       // Total: 22 bytes, mask: 22 chars

    private const string PreGameContextFile = "UiPregame.cpp";
    private const string PreGameContextAssertion = "!s_scene";
    private const int PreGameContextOffset = 0x34;

    private const string AvailableCharsMask = "xx????xxxxxxx";
    private const int AvailableCharsOffset = 0x2;
    private static readonly byte[] AvailableCharsPattern = [0x8B, 0x35, 0x00, 0x00, 0x00, 0x00, 0x57, 0x69, 0xF8, 0x84, 0x00, 0x00, 0x00];

    private readonly MemoryScanningService memoryScanningService;
    private readonly GWAddressCache gameContextGetterAddress;
    private readonly GWAddressCache preGameContextAddress;
    private readonly GWAddressCache availableCharsAddress;
    private readonly ILogger<GameContextService> logger;

    public GameContextService(
        MemoryScanningService memoryScanningService,
        ILogger<GameContextService> logger)
    {
        this.logger = logger.ThrowIfNull();
        this.memoryScanningService = memoryScanningService.ThrowIfNull();
        this.gameContextGetterAddress = new GWAddressCache(this.GetGameContextGetterAddress);
        this.preGameContextAddress = new GWAddressCache(this.GetPreGameContextAddress);
        this.availableCharsAddress = new GWAddressCache(this.GetAvailableCharactersAddress);
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate nuint GetGameContextDelegate();

    public List<AddressState> GetAddressStates()
    {
        return [
            new AddressState
            {
                Address = this.gameContextGetterAddress.GetAddress() ?? 0,
                Name = nameof(this.gameContextGetterAddress),
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
        
        // Get the address of the GameContext getter function
        var getterAddress = this.gameContextGetterAddress.GetAddress();
        if (getterAddress is null or 0)
        {
            scopedLogger.LogError("Failed to get GameContext getter function address");
            return 0;
        }

        // Call the function to get GameContext
        var getter = Marshal.GetDelegateForFunctionPointer<GetGameContextDelegate>((nint)getterAddress);
        var gameContextPtr = getter();

        if (gameContextPtr == 0)
        {
            scopedLogger.LogError("GameContext getter returned null");
            return 0;
        }

        scopedLogger.LogInformation("GameContext address: 0x{address:X8}", gameContextPtr);
        return gameContextPtr;
    }

    private nuint GetGameContextGetterAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        
        var address = this.memoryScanningService.FindAddress(
            GameContextTlsPattern, 
            GameContextTlsMask, 
            0);
            
        if (address is 0)
        {
            scopedLogger.LogError("Failed to find GameContext getter function");
            return 0U;
        }

        scopedLogger.LogInformation("GameContext getter function: 0x{address:X8}", address);
        return address;
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

        var actualAddress = *(nuint*)preGameContextAddress;
        scopedLogger.LogInformation("Pre-game context address: 0x{address:X8}", actualAddress);
        return actualAddress;
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
