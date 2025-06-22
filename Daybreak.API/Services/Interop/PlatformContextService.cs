using Daybreak.API.Interop;
using Daybreak.API.Models;
using System.Core.Extensions;
using System.Extensions.Core;

namespace Daybreak.API.Services.Interop;

public sealed class PlatformContextService
    : IAddressHealthService
{
    private const string WindowHandleAddressMask = "xxxxx????xxx";
    private const int WindowHandleAddressOffset = -0xC;
    private static readonly byte[] WindowHandleAddressPattern = [0x83, 0xC4, 0x04, 0x83, 0x3D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x75, 0x31];

    private readonly MemoryScanningService memoryScanningService;
    private readonly GWAddressCache windowHandleAddress;
    private readonly ILogger<PlatformContextService> logger;

    public PlatformContextService(
        MemoryScanningService memoryScanningService,
        ILogger<PlatformContextService> logger)
    {
        this.logger = logger.ThrowIfNull();
        this.memoryScanningService = memoryScanningService.ThrowIfNull();
        this.windowHandleAddress = new GWAddressCache(this.GetWindowHandleAddress);
    }

    public List<AddressState> GetAddressStates()
    {
        return [
            new AddressState
            {
                Address = this.windowHandleAddress.GetAddress() ?? 0,
                Name = nameof(this.windowHandleAddress),
            },
            ];
    }

    public unsafe uint? GetWindowHandle()
    {
        var windowHandlePtr = this.windowHandleAddress.GetAddress();
        if (windowHandlePtr is null)
        {
            this.logger.LogError("Failed to get window handle");
            return null;
        }

        return *(uint*)windowHandlePtr;
    }

    private nuint GetWindowHandleAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var baseAddress = this.memoryScanningService.FindAndResolveAddress(WindowHandleAddressPattern, WindowHandleAddressMask, WindowHandleAddressOffset);
        if (baseAddress is 0)
        {
            scopedLogger.LogError("Failed to find window handle address");
            return 0U;
        }

        scopedLogger.LogInformation("Window handle address: 0x{address:X8}", baseAddress);
        return baseAddress;
    }
}
