using Daybreak.API.Extensions;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Services.Interop;
using Daybreak.Shared.Models.Api;
using System.Extensions.Core;
using ZLinq;
using InstanceType = Daybreak.API.Interop.GuildWars.InstanceType;

namespace Daybreak.API.Services;

public sealed class PartyService(
    InstanceContextService instanceContextService,
    PartyContextService partyContextService,
    GameThreadService gameThreadService,
    AgentContextService agentContextService,
    GameContextService gameContextService,
    ILogger<PartyService> logger)
{
    private readonly InstanceContextService instanceContextService = instanceContextService;
    private readonly PartyContextService partyContextService = partyContextService;
    private readonly GameThreadService gameThreadService = gameThreadService;
    private readonly AgentContextService agentContextService = agentContextService;
    private readonly GameContextService gameContextService = gameContextService;
    private readonly ILogger<PartyService> logger = logger;

    public async Task<bool> SetPartyLoadout(PartyLoadout partyLoadout, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        
        if (!await this.IsInValidOutpost(cancellationToken))
        {
            scopedLogger.LogError("Could not set party loadout. Not in a valid outpost");
            return false;
        }

        if (!await this.LeaveParty(cancellationToken))
        {
            scopedLogger.LogError("Could not set party loadout. Could not leave party");
            return false;
        }


        return true;
    }

    public Task<PartyLoadout?> GetPartyLoadout(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        return this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                if (this.instanceContextService.GetInstanceType() is InstanceType.Loading or InstanceType.Explorable)
                {
                    scopedLogger.LogError("Not in outpost");
                    return default;
                }

                var gameContext = this.gameContextService.GetGameContext();
                if (gameContext is null ||
                    gameContext->WorldContext is null ||
                    gameContext->PartyContext is null)
                {
                    scopedLogger.LogError("Failed to get game context");
                    return default;
                }

                var playerId = gameContext->WorldContext->PlayerControlledChar->AgentId;
                var skillBars = gameContext->WorldContext->Skillbars;
                var attributes = gameContext->WorldContext->Attributes;
                var proffessions = gameContext->WorldContext->Professions;
                var heroFlags = gameContext->WorldContext->HeroFlags;

                var buildTuples = proffessions.AsValueEnumerable()
                    .Select(p => (p.AgentId, p, attributes.FirstOrDefault(a => a.AgentId == p.AgentId), skillBars.FirstOrDefault(s => s.AgentId == p.AgentId), heroFlags.FirstOrDefault(f => f.AgentId == p.AgentId)))
                    .Select(t => (t.AgentId, GetBuildEntryById(t.Item4, t.Item3), t.Item5.Behavior))
                    .Where(t => t.Item2 is not null)
                    .OfType<(uint AgentId, BuildEntry BuildEntry, Behavior Behavior)>()
                    .ToList();

                return new PartyLoadout(
                    [.. buildTuples.Select(t =>
                    {
                        if (t.AgentId == playerId)
                        {
                            return new PartyLoadoutEntry(0, HeroBehavior.Undefined, t.BuildEntry);
                        }
                        else if (gameContext->PartyContext->PlayerParty->Heroes.FirstOrDefault(h => h.AgentId == t.AgentId) is HeroPartyMember hero &&
                                hero.AgentId == t.AgentId)
                        {
                            return new PartyLoadoutEntry((int)hero.HeroId, (HeroBehavior)t.Behavior, t.BuildEntry);
                        }

                        return default;
                    })
                    .OfType<PartyLoadoutEntry>()]);
            }
        }, cancellationToken);
    }

    public async Task<bool> LeaveParty(CancellationToken cancellationToken)
    {
        var partySize = await this.GetPartySize(cancellationToken);
        if (partySize is 1)
        {
            return true;
        }

        return await this.gameThreadService.QueueOnGameThread(this.partyContextService.LeaveParty, cancellationToken);
    }

    public async Task<uint> GetPartySize(CancellationToken cancellationToken)
    {
        return await this.gameThreadService.QueueOnGameThread<uint>(() =>
        {
            unsafe
            {
                var gameContext = this.gameContextService.GetGameContext();
                if (gameContext is null || gameContext->PartyContext is null)
                {
                    this.logger.LogError("Failed to get game context");
                    return 0;
                }

                if (gameContext->PartyContext->PlayerParty is null)
                {
                    this.logger.LogError("Party context not initialized");
                    return 0;
                }

                return gameContext->PartyContext->PlayerParty->Players.Size + gameContext->PartyContext->PlayerParty->Heroes.Size + gameContext->PartyContext->PlayerParty->Henchmen.Size;
            }
        }, cancellationToken);
    }

    private async Task<bool> IsInValidOutpost(CancellationToken cancellationToken)
    {
        return await this.gameThreadService.QueueOnGameThread(() =>
        {
            return this.instanceContextService.GetInstanceType() is InstanceType.Outpost;
        }, cancellationToken);
    }

    private static BuildEntry? GetBuildEntryById(SkillbarContext skillbar, PartyAttribute attributes)
    {
        return new BuildEntry(
            Attributes: attributes.Attributes.GetAttributeEntryList(),
            Skills:
            [
                skillbar.Skill0.Id,
                skillbar.Skill1.Id,
                skillbar.Skill2.Id,
                skillbar.Skill3.Id,
                skillbar.Skill4.Id,
                skillbar.Skill5.Id,
                skillbar.Skill6.Id,
                skillbar.Skill7.Id
            ]);
    }
}
