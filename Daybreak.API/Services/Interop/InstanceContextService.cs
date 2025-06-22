using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using System.Core.Extensions;

namespace Daybreak.API.Services.Interop;

public sealed unsafe class InstanceContextService : IAddressHealthService
{
    private const string InstanceInfoAddressMask = "xxxx????xxxx";
    private const int InstanceInfoAddressOffset = +0xD;
    private static readonly byte[] InstanceInfoAddressPattern = [0x6A, 0x2C, 0x50, 0xE8, 0x00, 0x00, 0x00, 0x00, 0x83, 0xC4, 0x08, 0xC7];
    private const string AreaInfoAddressMask = "xxxxx";
    private const int AreaInfoAddressOffset = +0x5;
    private static readonly byte[] AreaInfoAddressPattern = [0x6B, 0xC6, 0x7C, 0x5E, 0x05];
    private const string ServerRegionAddressMask = "xxxxxxx";
    private const int ServerRegionAddressOffset = -0x4;
    private static readonly byte[] ServerRegionAddressPattern = [0x6A, 0x54, 0x8D, 0x46, 0x24, 0x89, 0x08];

    private readonly MemoryScanningService memoryScanningService;
    private readonly GWAddressCache instanceInfoAddress;
    private readonly GWAddressCache areaInfoAddress;
    private readonly GWAddressCache serverRegionAddress;
    private readonly ILogger<InstanceContextService> logger;

    public InstanceContextService(
        MemoryScanningService memoryScanningService,
        ILogger<InstanceContextService> logger)
    {
        this.logger = logger.ThrowIfNull();
        this.memoryScanningService = memoryScanningService.ThrowIfNull();
        this.instanceInfoAddress = new GWAddressCache(() => this.memoryScanningService.FindAndResolveAddress(InstanceInfoAddressPattern, InstanceInfoAddressMask, InstanceInfoAddressOffset));
        this.areaInfoAddress = new GWAddressCache(() => this.memoryScanningService.FindAndResolveAddress(AreaInfoAddressPattern, AreaInfoAddressMask, AreaInfoAddressOffset));
        this.serverRegionAddress = new GWAddressCache(() => this.memoryScanningService.FindAndResolveAddress(ServerRegionAddressPattern, ServerRegionAddressMask, ServerRegionAddressOffset));
    }

    public List<AddressState> GetAddressStates()
    {
        return [
            new AddressState
            {
                Address = this.instanceInfoAddress.GetAddress() ?? 0,
                Name = nameof(this.instanceInfoAddress),
            },
            new AddressState
            {
                Address = this.areaInfoAddress.GetAddress() ?? 0,
                Name = nameof(this.areaInfoAddress),
            },
            new AddressState
            {
                Address = this.serverRegionAddress.GetAddress() ?? 0,
                Name = nameof(this.serverRegionAddress),
            }
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

    public WrappedPointer<AreaInfo> GetAreaInfo()
    {
        var areaInfoAddress = this.areaInfoAddress.GetAddress();
        if (areaInfoAddress is null)
        {
            this.logger.LogError("Failed to get area info address");
            return null;
        }

        return (AreaInfo*)areaInfoAddress;
    }

    public ServerRegion GetServerRegion()
    {
        var serverRegionAddress = this.serverRegionAddress.GetAddress();
        if (serverRegionAddress is null)
        {
            this.logger.LogError("Failed to get server region address");
            return ServerRegion.Unknown;
        }

        return *(ServerRegion*)serverRegionAddress;
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
}
