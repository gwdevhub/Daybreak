using Daybreak.API.Services.Interop;
using Daybreak.Shared.Models.Api;
using System.Extensions.Core;
using ZLinq;

namespace Daybreak.API.Services;

public sealed class PartyService(
    GameThreadService gameThreadService,
    AgentContextService agentContextService,
    GameContextService gameContextService,
    ILogger<PartyService> logger)
{
    private readonly GameThreadService gameThreadService = gameThreadService;
    private readonly AgentContextService agentContextService = agentContextService;
    private readonly GameContextService gameContextService = gameContextService;
    private readonly ILogger<PartyService> logger = logger;

}
