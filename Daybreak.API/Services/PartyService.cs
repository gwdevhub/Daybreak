using Daybreak.API.Extensions;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using Daybreak.API.Services.Interop;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.BuildTemplates;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;
using ZLinq;
using InstanceType = Daybreak.API.Interop.GuildWars.InstanceType;

namespace Daybreak.API.Services;

public sealed class PartyService(
    ChatService chatService,
    IBuildTemplateManager buildTemplateManager,
    UIService uiService,
    UIContextService uIContextService,
    SkillbarContextService skillbarContextService,
    InstanceContextService instanceContextService,
    PartyContextService partyContextService,
    GameThreadService gameThreadService,
    GameContextService gameContextService,
    ILogger<PartyService> logger)
{
    private static readonly TimeSpan HeroSpawnDelay = TimeSpan.FromSeconds(1);

    private readonly ChatService chatService = chatService.ThrowIfNull();
    private readonly IBuildTemplateManager buildTemplateManager = buildTemplateManager.ThrowIfNull();
    private readonly UIService uiService = uiService.ThrowIfNull();
    private readonly UIContextService uIContextService = uIContextService.ThrowIfNull();
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
            await this.chatService.AddMessageAsync("Cannot set party loadout. Not in a valid outpost.", "Daybreak.API", Channel.Moderator, cancellationToken);
            return false;
        }

        if (!await this.KickAllHeroes(cancellationToken))
        {
            scopedLogger.LogError("Could not set party loadout. Could not leave party");
            await this.chatService.AddMessageAsync("Cannot set party loadout. Could not leave party.", "Daybreak.API", Channel.Moderator, cancellationToken);
            return false;
        }

        if (!await this.gameThreadService.QueueOnGameThread(() => this.SpawnHeroes(partyLoadout), cancellationToken))
        {
            scopedLogger.LogError("Could not set party loadout. Could not spawn heroes");
            await this.chatService.AddMessageAsync("Cannot set party loadout. Could not spawn heroes.", "Daybreak.API", Channel.Moderator, cancellationToken);
            return false;
        }

        await Task.Delay(HeroSpawnDelay, cancellationToken);

        if (!await this.gameThreadService.QueueOnGameThread(() => this.ApplyBuilds(partyLoadout), cancellationToken))
        {
            scopedLogger.LogError("Could not set party loadout. Could not apply builds");
            await this.chatService.AddMessageAsync("Cannot set party loadout. Could not apply builds.", "Daybreak.API", Channel.Moderator, cancellationToken);
            return false;
        }

        var heroBehaviorSetup = await this.gameThreadService.QueueOnGameThread(() => this.GetHeroBehaviorSetup(partyLoadout), cancellationToken);
        foreach (var heroBehaviorEntry in heroBehaviorSetup ?? [])
        {
            if (!await this.SetHeroBehavior(heroBehaviorEntry.AgentId, heroBehaviorEntry.Behavior, cancellationToken))
            {
                scopedLogger.LogWarning("Could not set hero behavior for agent {agentId} to {behavior}", heroBehaviorEntry.AgentId, heroBehaviorEntry.Behavior);
                await this.chatService.AddMessageAsync($"Cannot set party loadout. Could not set behavior {heroBehaviorEntry.Behavior} for agent {heroBehaviorEntry.AgentId}.", "Daybreak.API", Channel.Moderator, cancellationToken);
            }
        }

        await this.chatService.AddMessageAsync("Party loadout set.", "Daybreak.API", Channel.Moderator, cancellationToken);
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
                    .Select(t => (t.AgentId, GetBuildEntryById(t.p, t.Item4, t.Item3), t.Item5.Behavior))
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

    public async Task<bool> KickAllHeroes(CancellationToken cancellationToken)
    {
        var partySize = await this.GetPartySize(cancellationToken);
        if (partySize is 1 or 0)
        {
            return true;
        }

        return await this.gameThreadService.QueueOnGameThread(this.partyContextService.KickAllHeroes, cancellationToken);
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

    public async Task<bool> SetHeroBehavior(uint heroAgentId, HeroBehavior behavior, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();

        if (!await this.IsInValidOutpost(cancellationToken))
        {
            scopedLogger.LogError("Could not set hero behavior. Not in a valid outpost");
            return false;
        }

        var heroCommanderFrameLabel = await this.HeroCommanderFrameLabel(heroAgentId, cancellationToken);
        if (heroCommanderFrameLabel is null)
        {
            scopedLogger.LogError("Could not set hero behavior. Could not find hero commander frame label for agent {heroAgentId}", heroAgentId);
            return false;
        }

        var maybeFrame = await this.uiService.GetManagedFrame(heroCommanderFrameLabel, cancellationToken);
        if (!maybeFrame.HasValue)
        {
            scopedLogger.LogError("Could not set hero behavior. Could not get managed frame for label {heroCommanderFrameLabel}", heroCommanderFrameLabel);
            return false;
        }

        using var frame = maybeFrame.Value;
        var result = await this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                var btn = this.uIContextService.GetChildFrame(frame.Frame, (uint)behavior);
                var packet = new UIPackets.MouseClick(UIPackets.MouseButtons.Left, 0, 0);
                if (btn.IsNull)
                {
                    scopedLogger.LogError("Could not set hero behavior. Could not find button for behavior {behavior} in frame {frameLabel}", behavior, heroCommanderFrameLabel);
                    return false;
                }

                return this.uIContextService.SendFrameUIMessage(btn, UIMessage.MouseClick, &packet);
            }
        }, cancellationToken);

        if (!result)
        {
            scopedLogger.LogError("Failed to send UI message to set hero behavior {behavior} for agent {heroAgentId}", behavior, heroAgentId);
        }

        scopedLogger.LogInformation("Set hero behavior {behavior} for agent {agentId}", behavior, heroAgentId);
        return result;
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
        foreach (var entry in partyLoadout.Entries.AsValueEnumerable().OrderBy(e => e.Build.Primary))
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

        if (!this.gameContextService.GetGameContext().TryGetAccountContext(out var accountContext) ||
            accountContext.IsNull)
        {
            scopedLogger.LogError("Failed to get account context");
            return false;
        }

        var playerProfessionContext = professions.Value.AsValueEnumerable().FirstOrDefault(p => p.AgentId == playerId);
        if (playerProfessionContext.AgentId != playerId)
        {
            scopedLogger.LogError("Failed to get player profession context for player id {playerId}", playerId);
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
                var validSkills = t.playerId == playerId
                    ? unlockedSkills
                    : accountContext.Pointer->UnlockedAccountSkills;

                var unlockedProfessionsFlags = t.playerId == playerId
                    ? playerProfessionContext.UnlockedProfessionsFlags
                    : uint.MaxValue;

                if (!this.buildTemplateManager.CanTemplateApply(
                    new BuildTemplateValidationRequest(
                        (uint)t.entry.Build.Primary,
                        (uint)t.entry.Build.Secondary,
                        [.. t.entry.Build.Skills],
                        (uint)t.Item3.CurrentPrimary,
                        unlockedProfessionsFlags,
                        [.. validSkills])))
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
            });

        foreach (var (entry, maybeAgentId, _, _) in parsedEntries)
        {
            if (maybeAgentId is not uint agentId)
            {
                continue;
            }

            var attributeIds = new Array12Uint();
            var attributeValues = new Array12Uint();
            for (var i = 0; i < entry.Build.Attributes.Count && i < 12; i++)
            {
                attributeIds[i] = (uint)entry.Build.Attributes[i].Id;
                attributeValues[i] = (uint)entry.Build.Attributes[i].BasePoints;
            }

            var skills = new Array8Uint();
            for (var i = 0; i < entry.Build.Skills.Count && i < 8; i++)
            {
                skills[i] = entry.Build.Skills[i];
            }

            var skillTemplate = new SkillTemplate(
                (uint)entry.Build.Primary,
                (uint)entry.Build.Secondary,
                (uint)entry.Build.Attributes.Count,
                attributeIds,
                attributeValues,
                skills);

            scopedLogger.LogInformation("Applying build for agent {agentId} with primary {primary} and secondary {secondary}", agentId, entry.Build.Primary, entry.Build.Secondary);
            this.skillbarContextService.LoadBuild(agentId, &skillTemplate);
        }

        return true;
    }

    private unsafe List<(uint AgentId, HeroBehavior Behavior)>? GetHeroBehaviorSetup(PartyLoadout partyLoadout)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Getting hero behavior setup");
        
        if (!this.gameContextService.GetGameContext().TryGetPlayerParty(out _, out _, out var heroes, out _) ||
            !heroes.HasValue)
        {
            scopedLogger.LogError("Failed to get player party");
            return default;
        }

        return heroes.Value.AsValueEnumerable()
            .Select(h =>
            {
                if (h.HeroId is 0)
                {
                    return default;
                }

                var entry = partyLoadout.Entries.AsValueEnumerable().FirstOrDefault(e => (uint)e.HeroId == h.HeroId);
                if (entry is null)
                {
                    scopedLogger.LogWarning("No entry found for hero {heroId}", h.HeroId);
                    return default;
                }

                return (h.AgentId, entry.HeroBehavior);
            })
            .Where(t => t.AgentId is not 0)
            .ToList();
    }

    private async Task<string?> HeroCommanderFrameLabel(uint heroAgentId, CancellationToken cancellationToken)
    {
        var heroOffset = await this.GetHeroNumber(heroAgentId, cancellationToken);
        if (heroOffset > 6)
        {
            return default;
        }

        return heroOffset switch
        {
            0 => "AgentCommander0",
            1 => "AgentCommander1",
            2 => "AgentCommander2",
            3 => "AgentCommander3",
            4 => "AgentCommander4",
            5 => "AgentCommander5",
            6 => "AgentCommander6",
            _ => default
        };
    }

    private async Task<uint?> GetHeroNumber(uint agentId, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var heroNumber = await this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                var gameContext = this.gameContextService.GetGameContext();
                if (gameContext.IsNull)
                {
                    scopedLogger.LogError("Game context is null");
                    return -1;
                }

                var partyContext = gameContext.Pointer->PartyContext;
                if (partyContext is null)
                {
                    scopedLogger.LogError("Party context is null");
                    return -1;
                }

                var charContext = gameContext.Pointer->CharContext;
                if (charContext is null)
                {
                    scopedLogger.LogError("Char context is null");
                    return -1;
                }

                var playerParty = partyContext->PlayerParty;
                var playerId = charContext->PlayerNumber;
                var offset = 0;
                foreach(var hero in playerParty->Heroes)
                {
                    if (hero.OwnerPlayerId == playerId)
                    {
                        if (hero.AgentId == agentId)
                        {
                            return offset;
                        }

                        offset++;
                    }
                }

                return -1;
            }
        }, cancellationToken);

        return heroNumber is -1 ? default : (uint)heroNumber;
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
