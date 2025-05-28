using Daybreak.API.Interop.GuildWars;
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

    public Task<PartyLoadout?> GetPartyLoadout(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        return this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
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

                var buildTuples = proffessions.AsValueEnumerable()
                    .Select(p => (p.AgentId, p, attributes.FirstOrDefault(a => a.AgentId == p.AgentId), skillBars.FirstOrDefault(s => s.AgentId == p.AgentId)))
                    .Select(t => (t.AgentId, GetBuildEntryById(t.Item4, t.Item3)))
                    .Where(t => t.Item2 is not null)
                    .OfType<(uint AgentId, BuildEntry BuildEntry)>()
                    .ToList();

                return new PartyLoadout(
                    [.. buildTuples.Select(t =>
                    {
                        if (t.AgentId == playerId)
                        {
                            return new PartyLoadoutEntry(0, t.BuildEntry);
                        }
                        else if (gameContext->PartyContext->PlayerParty->Heroes.FirstOrDefault(h => h.AgentId == t.AgentId) is HeroPartyMember hero &&
                                hero.AgentId == t.AgentId)
                        {
                            return new PartyLoadoutEntry((int)hero.HeroId, t.BuildEntry);
                        }

                        return default;
                    })
                    .OfType<PartyLoadoutEntry>()]);
            }
        }, cancellationToken);
    }

    private static BuildEntry? GetBuildEntryById(SkillbarContext skillbar, PartyAttribute attributes)
    {
        return new BuildEntry(
            Attributes: attributes.Attributes
                .AsValueEnumerable()
                .Select(a => new AttributeEntry(a.Id, a.LevelBase, a.Level))
                .ToList(),
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
