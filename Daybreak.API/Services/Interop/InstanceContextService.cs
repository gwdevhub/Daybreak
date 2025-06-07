using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using System.Core.Extensions;
using System.Extensions.Core;

namespace Daybreak.API.Services.Interop;

public sealed unsafe class InstanceContextService : IAddressHealthService
{
    private const string InstanceInfoAddressMask = "xxxx????xxxx";
    private const int InstanceInfoAddressOffset = +0xD;
    private static readonly byte[] InstanceInfoAddressPattern = [0x6A, 0x2C, 0x50, 0xE8, 0x00, 0x00, 0x00, 0x00, 0x83, 0xC4, 0x08, 0xC7];

    private readonly MemoryScanningService memoryScanningService;
    private readonly GWAddressCache instanceInfoAddress;
    private readonly ILogger<InstanceContextService> logger;

    public InstanceContextService(
        MemoryScanningService memoryScanningService,
        ILogger<InstanceContextService> logger)
    {
        this.logger = logger.ThrowIfNull();
        this.memoryScanningService = memoryScanningService.ThrowIfNull();
        this.instanceInfoAddress = new GWAddressCache(this.GetInstanceInfoAddress);
    }

    public List<AddressState> GetAddressStates()
    {
        return [
            new AddressState
            {
                Address = this.instanceInfoAddress.GetAddress() ?? 0,
                Name = nameof(this.instanceInfoAddress),
            },
            ];
    }

    public WrappedPointer<InstanceInfoContext> GetInstanceInfoContext()
    {
        var instanceInfoAddress = this.instanceInfoAddress.GetAddress();
        if (instanceInfoAddress is null)
        {
            this.logger.LogError("Failed to get instance info address");
            return null;
        }

        return (InstanceInfoContext*)instanceInfoAddress;
    }

    public InstanceType GetInstanceType()
    {
        var instanceInfo = this.GetInstanceInfoContext();
        if (instanceInfo.IsNull)
        {
            return InstanceType.Loading;
        }

        return instanceInfo.Pointer->InstanceType;
    }

    private nuint GetInstanceInfoAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var baseAddress = this.memoryScanningService.FindAndResolveAddress(InstanceInfoAddressPattern, InstanceInfoAddressMask, InstanceInfoAddressOffset);
        if (baseAddress is 0)
        {
            scopedLogger.LogError("Failed to find instance info address");
            return 0U;
        }

        scopedLogger.LogInformation("Instance info address: 0x{address:X8}", baseAddress);
        return baseAddress;
    }
}
