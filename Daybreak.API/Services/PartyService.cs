using Daybreak.API.Extensions;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Services.Interop;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.BuildTemplates;
using System.Core.Extensions;
using System.Extensions.Core;
using ZLinq;
using InstanceType = Daybreak.API.Interop.GuildWars.InstanceType;

namespace Daybreak.API.Services;

public sealed class PartyService(
    IBuildTemplateManager buildTemplateManager,
    SkillbarContextService skillbarContextService,
    InstanceContextService instanceContextService,
    PartyContextService partyContextService,
    GameThreadService gameThreadService,
    GameContextService gameContextService,
    ILogger<PartyService> logger)
{
    private readonly IBuildTemplateManager buildTemplateManager = buildTemplateManager.ThrowIfNull();
    private readonly SkillbarContextService skillbarContextService = skillbarContextService.ThrowIfNull();
    private readonly InstanceContextService instanceContextService = instanceContextService.ThrowIfNull();
    private readonly PartyContextService partyContextService = partyContextService.ThrowIfNull();
    private readonly GameThreadService gameThreadService = gameThreadService.ThrowIfNull();
    private readonly GameContextService gameContextService = gameContextService.ThrowIfNull();
    private readonly ILogger<PartyService> logger = logger.ThrowIfNull();

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

        if (!await this.gameThreadService.QueueOnGameThread(() => this.SpawnHeroes(partyLoadout), cancellationToken))
        {
            scopedLogger.LogError("Could not set party loadout. Could not spawn heroes");
            return false;
        }

        if (!await this.gameThreadService.QueueOnGameThread(() => this.ApplyBuilds(partyLoadout), cancellationToken))
        {
            scopedLogger.LogError("Could not set party loadout. Could not apply builds");
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

                if (!this.gameContextService.GetGameContext().TryGetBuildContext(out var skillbars, out var attributes, out var professions, out _))
                {
                    scopedLogger.LogError("Failed to get build context");
                    return default;
                }

                if (!this.gameContextService.GetGameContext().TryGetHeroFlags(out var heroFlags))
                {
                    scopedLogger.LogError("Failed to get hero flags");
                    return default;
                }

                if (!this.gameContextService.GetGameContext().TryGetPlayerId(out var playerId))
                {
                    scopedLogger.LogError("Failed to get player id");
                    return default;
                }

                if (!this.gameContextService.GetGameContext().TryGetPlayerParty(out _, out _, out var heroes, out _))
                {
                    scopedLogger.LogError("Failed to get player party");
                    return default;
                }

                var buildTuples = professions.Value.AsValueEnumerable()
                    .Select(p => (p.AgentId, p, attributes.Value.FirstOrDefault(a => a.AgentId == p.AgentId), skillbars.Value.FirstOrDefault(s => s.AgentId == p.AgentId), heroFlags.Value.FirstOrDefault(f => f.AgentId == p.AgentId)))
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
                        else if (heroes.Value.FirstOrDefault(h => h.AgentId == t.AgentId) is HeroPartyMember hero &&
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
        if (partySize is 1 or 0)
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
                if (!this.gameContextService.GetGameContext().TryGetPlayerParty(out _, out var players, out var heroes, out var henchmen))
                {
                    this.logger.LogError("Failed to get player party");
                    return 0;
                }

                return players.Value.Size + heroes.Value.Size + heroes.Value.Size;
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

    private unsafe bool SpawnHeroes(PartyLoadout partyLoadout)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Spawning {heroCount} heroes for party loadout", partyLoadout.Entries.AsValueEnumerable().Count(c => c.HeroId != 0));
        foreach (var entry in partyLoadout.Entries)
        {
            if (entry.HeroId != 0 &&
                Hero.TryParse(entry.HeroId, out var hero))
            {
                scopedLogger.LogInformation("Adding hero [{heroId}] [{heroName}] with behavior {behavior}", entry.HeroId, hero.Name, entry.HeroBehavior);
                this.partyContextService.AddHero((uint)entry.HeroId);
            }
            else
            {
                scopedLogger.LogWarning("Invalid hero entry in party loadout: {heroId}", entry.HeroId);
                continue;
            }
        }

        return true;
    }

    private unsafe bool ApplyBuilds(PartyLoadout partyLoadout)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Applying builds");
        

        if (!this.gameContextService.GetGameContext().TryGetPlayerParty(out _, out var players, out var heroes, out var henchmen))
        {
            scopedLogger.LogError("Failed to get player party");
            return false;
        }

        if (!this.gameContextService.GetGameContext().TryGetBuildContext(out _, out _, out var professions, out var unlockedSkills))
        {
            scopedLogger.LogError("Failed to get build context");
            return false;
        }

        if (!this.gameContextService.GetGameContext().TryGetPlayerId(out var playerId))
        {
            scopedLogger.LogError("Failed to get player id");
            return false;
        }

        var parsedEntries = partyLoadout.Entries.AsValueEnumerable()
            .Select(entry =>
            {
                if (entry.HeroId is 0)
                {
                    return (entry, playerId);
                }
                else
                {
                    var hero = heroes.Value.AsValueEnumerable().FirstOrDefault(h => h.HeroId == entry.HeroId);
                    return (entry, hero.AgentId);
                }
            })
            .Select(t => (t.entry, t.playerId, professions.Value.AsValueEnumerable().FirstOrDefault(p => p.AgentId == t.playerId)))
            .Select(t =>
            {
                if (!this.buildTemplateManager.CanTemplateApply(
                    new BuildTemplateValidationRequest(
                        (uint)t.entry.Build.Primary,
                        (uint)t.entry.Build.Secondary,
                        [.. t.entry.Build.Skills],
                        (uint)t.Item3.CurrentPrimary,
                        t.Item3.UnlockedProfessionsFlags,
                        [.. unlockedSkills.Value])))
                {
                    return (t.entry, t.playerId, t.Item3, false);
                }

                return (t.entry, t.playerId, t.Item3, true);
            })
            .Where(t =>
            {
                if (!t.Item4)
                {
                    if (t.entry.HeroId is 0)
                    {
                        scopedLogger.LogError("Cannot apply build for player");
                    }
                    else
                    {
                        scopedLogger.LogError("Cannot apply build for hero {heroId}", t.entry.HeroId);
                    }
                }

                return t.Item4;
            })
            .OfType<(PartyLoadoutEntry Entry, uint AgentId, ProfessionsContext ProfessionContext, bool IsValid)>();

        return true;
    }

    private static BuildEntry? GetBuildEntryById(ProfessionsContext professionContext, SkillbarContext skillbar, PartyAttribute attributes)
    {
        return new BuildEntry(
            Primary: (int)professionContext.CurrentPrimary,
            Secondary: (int)professionContext.CurrentSecondary,
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
