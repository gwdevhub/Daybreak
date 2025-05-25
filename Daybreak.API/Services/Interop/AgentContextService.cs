using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using System.Core.Extensions;
using System.Extensions.Core;

namespace Daybreak.API.Services.Interop;

public unsafe sealed class AgentContextService : IAddressHealthService
{
    private const string AgentArrayMask = "xxxxxxx";
    private const int AgentArrayOffset = -0x4;
    private static readonly byte[] AgentArrayAddressPattern = [0x8b, 0x0c, 0x90, 0x85, 0xc9, 0x74, 0x19];

    private const string PlayerAgentIdMask = "xx????xxxx";
    private const int PlayerAgentIdOffset = -0xE;
    private static readonly byte[] PlayerAgentIdAddressPattern = [0x5d, 0xe9, 0x00, 0x00, 0x00, 0x00, 0x55, 0x8b, 0xec, 0x53];

    private readonly MemoryScanningService memoryScanningService;
    private readonly GWAddressCache agentArrayAddress;
    private readonly GWAddressCache playerAgentIdAddress;
    private readonly ILogger<AgentContextService> logger;

    public AgentContextService(
        MemoryScanningService memoryScanningService,
        ILogger<AgentContextService> logger)
    {
        this.logger = logger.ThrowIfNull();
        this.memoryScanningService = memoryScanningService.ThrowIfNull();
        this.agentArrayAddress = new GWAddressCache(this.GetAgentArrayAddress);
        this.playerAgentIdAddress = new GWAddressCache(this.GetPreGameContextAddress);
    }

    public List<AddressState> GetAddressStates()
    {
        return [
            new AddressState
            {
                Address = this.agentArrayAddress.GetAddress() ?? 0,
                Name = nameof(this.agentArrayAddress),
            },
            new AddressState
            {
                Address = this.playerAgentIdAddress.GetAddress() ?? 0,
                Name = nameof(this.playerAgentIdAddress),
            },
        ];
    }

    public GuildWarsArray<WrappedPointer<AgentContext>>* GetAgentArray()
    {
        var agentArrayAddress = this.agentArrayAddress.GetAddress();
        if (agentArrayAddress is null or 0x0)
        {
            this.logger.LogError("Failed to get agent array address");
            return null;
        }

        return (GuildWarsArray<WrappedPointer<AgentContext>>*)agentArrayAddress;
    }

    public uint GetPlayerAgentId()
    {
        var playerAgentIdAddress = this.playerAgentIdAddress.GetAddress();
        if (playerAgentIdAddress is null or 0x0)
        {
            this.logger.LogError("Failed to get player agent id address");
            return 0;
        }

        return *(uint*)playerAgentIdAddress;
    }

    private nuint GetAgentArrayAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var address = this.memoryScanningService.FindAndResolveAddress(AgentArrayAddressPattern, AgentArrayMask, AgentArrayOffset);
        if (address is 0)
        {
            scopedLogger.LogError("Failed to find agent array address");
            return 0U;
        }

        scopedLogger.LogInformation("Agent array address: 0x{address:X8}", address);
        return address;
    }

    private nuint GetPreGameContextAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var address = this.memoryScanningService.FindAndResolveAddress(PlayerAgentIdAddressPattern, PlayerAgentIdMask, PlayerAgentIdOffset);
        if (address is 0)
        {
            scopedLogger.LogError("Failed to find player agent id address");
            return 0U;
        }

        scopedLogger.LogInformation("Player agent id address: 0x{address:X8}", address);
        return address;
    }
}
