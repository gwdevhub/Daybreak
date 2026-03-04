using Daybreak.API.Extensions;
using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using Daybreak.API.Services.Interop;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.BuildTemplates;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;
using System.Runtime.CompilerServices;
using ZLinq;
using InstanceType = Daybreak.API.Interop.GuildWars.InstanceType;

namespace Daybreak.API.Services;

public sealed class PartyService : IHostedService
{
    private static readonly TimeSpan HeroSpawnDelay = TimeSpan.FromSeconds(1);

    private readonly ChatService chatService;
    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly UIService uiService;
    private readonly UIContextService uIContextService;
    private readonly SkillbarContextService skillbarContextService;
    private readonly InstanceContextService instanceContextService;
    private readonly PartyContextService partyContextService;
    private readonly GameThreadService gameThreadService;
    private readonly GameContextService gameContextService;
    private readonly ILogger<PartyService> logger;
    private readonly CallbackRegistration<Func<string, WrappedPointer<SkillTemplate>, bool>> decodeTemplateHeaderCallbackRegistration;
    private readonly CallbackRegistration<Func<SkillTemplate, bool>> loadSkillTemplateCallbackRegistration;
    private readonly CallbackRegistration<Func<WrappedPointer<TemplateSummaryFrameData>, WrappedPointer<SkillTemplate>, bool>> populateSkillDataCallbackRegistration;

    private string? cachedTemplateCode;
    private nint floatingPreviewFrame;
    private bool isPopulatingExtraBuilds;

    public PartyService(
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
        this.chatService = chatService.ThrowIfNull();
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.uiService = uiService.ThrowIfNull();
        this.uIContextService = uIContextService.ThrowIfNull();
        this.skillbarContextService = skillbarContextService.ThrowIfNull();
        this.instanceContextService = instanceContextService.ThrowIfNull();
        this.partyContextService = partyContextService.ThrowIfNull();
        this.gameThreadService = gameThreadService.ThrowIfNull();
        this.gameContextService = gameContextService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.decodeTemplateHeaderCallbackRegistration = this.skillbarContextService.RegisterDecodeTemplateHeaderHandler(this.DecodeTemplateHeader);
        this.loadSkillTemplateCallbackRegistration = this.skillbarContextService.RegisterLoadSkillTemplateHandler(this.LoadSkillTemplate);
        this.populateSkillDataCallbackRegistration = this.skillbarContextService.RegisterPopulateSkillDataHandler(this.PopulateSkillData);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Needed to be a hosted service so that we can register the callbacks in the constructor.
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.decodeTemplateHeaderCallbackRegistration.Dispose();
        this.loadSkillTemplateCallbackRegistration.Dispose();
        this.populateSkillDataCallbackRegistration.Dispose();
        // Floating preview will be cleaned up by the game on shutdown.
        this.floatingPreviewFrame = 0;
        return Task.CompletedTask;
    }

    public async Task<bool> SetPartyLoadout(string partyLoadoutTemplate, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();

        if (!await this.IsInValidOutpost(cancellationToken))
        {
            scopedLogger.LogError("Could not set party loadout. Not in a valid outpost");
            await this.chatService.AddMessageAsync("Cannot set party loadout. Not in a valid outpost.", "Daybreak.API", Channel.CHANNEL_MODERATOR, cancellationToken);
            return false;
        }

        if (!this.buildTemplateManager.TryDecodeTemplate(partyLoadoutTemplate, out var build) ||
                build is not TeamBuildEntry teamBuild)
        {
            scopedLogger.LogError("Could not set party loadout. Not a valid party loadout");
            await this.chatService.AddMessageAsync("Cannot set party loadout. Not a valid party loadout.", "Daybreak.API", Channel.CHANNEL_MODERATOR, cancellationToken);
            return false;
        }

        if (!await this.KickAllHeroes(cancellationToken))
        {
            scopedLogger.LogError("Could not set party loadout. Could not leave party");
            await this.chatService.AddMessageAsync("Cannot set party loadout. Could not leave party.", "Daybreak.API", Channel.CHANNEL_MODERATOR, cancellationToken);
            return false;
        }

        if (!await this.gameThreadService.QueueOnGameThread(() => this.SpawnHeroes(teamBuild), cancellationToken))
        {
            scopedLogger.LogError("Could not set party loadout. Could not spawn heroes");
            await this.chatService.AddMessageAsync("Cannot set party loadout. Could not spawn heroes.", "Daybreak.API", Channel.CHANNEL_MODERATOR, cancellationToken);
            return false;
        }

        await Task.Delay(HeroSpawnDelay, cancellationToken);

        if (!await this.gameThreadService.QueueOnGameThread(() => this.ApplyBuilds(teamBuild), cancellationToken))
        {
            scopedLogger.LogError("Could not set party loadout. Could not apply builds");
            await this.chatService.AddMessageAsync("Cannot set party loadout. Could not apply builds.", "Daybreak.API", Channel.CHANNEL_MODERATOR, cancellationToken);
            return false;
        }

        var heroBehaviorSetup = await this.gameThreadService.QueueOnGameThread(() => this.GetHeroBehaviorSetup(teamBuild), cancellationToken);
        foreach (var (AgentId, Behavior) in heroBehaviorSetup ?? [])
        {
            if (!await this.SetHeroBehavior(AgentId, Behavior, cancellationToken))
            {
                scopedLogger.LogWarning("Could not set hero behavior for agent {agentId} to {behavior}", AgentId, Behavior);
                await this.chatService.AddMessageAsync($"Cannot set party loadout. Could not set behavior {Behavior} for agent {AgentId}.", "Daybreak.API", Channel.CHANNEL_MODERATOR, cancellationToken);
            }
        }

        await this.chatService.AddMessageAsync("Party loadout set.", "Daybreak.API", Channel.CHANNEL_MODERATOR, cancellationToken);
        return true;
    }

    public async Task<string?> GetPartyLoadout(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var teamBuild = await this.gameThreadService.QueueOnGameThread(() =>
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
                    .Select(t => (t.AgentId, GetBuildEntryById(t.p, t.Item4, t.Item3), t.Item5.HeroBehavior))
                    .Where(t => t.Item2 is not null)
                    .OfType<(uint AgentId, BuildEntry BuildEntry, API.Interop.GWCA.GW.HeroBehavior Behavior)>()
                    .ToList();

                var teamBuildEntry = this.buildTemplateManager.CreateTeamBuild();
                teamBuildEntry.Builds.Clear();
                var partyComposition = new List<PartyCompositionMetadataEntry>();

                foreach (var t in buildTuples.OrderBy(t => t.AgentId == playerId ? 0 : 1))
                {
                    var singleBuild = this.buildTemplateManager.CreateSingleBuild(t.BuildEntry);
                    teamBuildEntry.Builds.Add(singleBuild);

                    if (t.AgentId == playerId)
                    {
                        partyComposition.Add(new PartyCompositionMetadataEntry
                        {
                            Type = PartyCompositionMemberType.MainPlayer,
                            Index = teamBuildEntry.Builds.Count - 1
                        });
                    }
                    else if (heroes.Value.FirstOrDefault(h => h.AgentId == t.AgentId) is HeroPartyMember hero &&
                             hero.AgentId == t.AgentId)
                    {
                        partyComposition.Add(new PartyCompositionMetadataEntry
                        {
                            Type = PartyCompositionMemberType.Hero,
                            Index = teamBuildEntry.Builds.Count - 1,
                            HeroId = (int)hero.HeroId,
                            Behavior = (Shared.Models.Api.HeroBehavior)t.Behavior
                        });
                    }
                }

                teamBuildEntry.PartyComposition = partyComposition;
                return teamBuildEntry;
            }
        }, cancellationToken);

        if (teamBuild is null)
        {
            return default;
        }

        return this.buildTemplateManager.EncodeTemplate(teamBuild);
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

    public async Task<bool> SetHeroBehavior(uint heroAgentId, Shared.Models.Api.HeroBehavior behavior, CancellationToken cancellationToken)
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
                var packet = new kMouseClick { MouseButton = 0, IsDoubleclick = 0 };
                if (btn.IsNull)
                {
                    scopedLogger.LogError("Could not set hero behavior. Could not find button for behavior {behavior} in frame {frameLabel}", behavior, heroCommanderFrameLabel);
                    return false;
                }

                return this.uIContextService.SendFrameUIMessage(btn, UIMessage.kMouseClick, &packet);
            }
        }, cancellationToken);

        if (!result)
        {
            scopedLogger.LogError("Failed to send UI message to set hero behavior {behavior} for agent {heroAgentId}", behavior, heroAgentId);
        }

        scopedLogger.LogInformation("Set hero behavior {behavior} for agent {agentId}", behavior, heroAgentId);
        return result;
    }

    private unsafe bool DecodeTemplateHeader(string templateCode, WrappedPointer<SkillTemplate> skillTemplate)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        this.cachedTemplateCode = default;
        // Clean up any previous floating preview when a new template is being decoded
        this.CleanupFloatingPreview();
        if (!this.buildTemplateManager.TryDecodeTemplate(templateCode, out var build) ||
            build is not TeamBuildEntry teamBuildEntry ||
            teamBuildEntry.Builds.Count is 0)
        {
            return false;
        }

        this.cachedTemplateCode = templateCode;
        PopulateSkillTemplate(teamBuildEntry.Builds[0], skillTemplate);
        scopedLogger.LogDebug("Decoded skill template from template code: {templateCode}", templateCode);
        return true;
    }

    private bool LoadSkillTemplate(SkillTemplate template)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (this.cachedTemplateCode is null)
        {
            return false;
        }

        var pendingLoadout = this.cachedTemplateCode;
        this.cachedTemplateCode = default;
        scopedLogger.LogDebug("Loading skill template from cached template code: {templateCode}", pendingLoadout);
        Task.Run(async () => await this.SetPartyLoadout(pendingLoadout, CancellationToken.None));
        // Return false to allow the normal template loading to continue.
        // Returning true would block the original LoadSkillTemplate call chain,
        // leaving the game with uninitialized template data and causing
        // TemplatesSkillsCanApply assertion failures.
        return false;
    }

    /// <summary>
    /// Intercepts TemplatesSummary_PopulateSkillData when a team build is cached.
    /// Creates an independent floating frame with skill template previews for
    /// each build beyond the first, positioned relative to the template dialog.
    /// </summary>
    private unsafe bool PopulateSkillData(WrappedPointer<TemplateSummaryFrameData> frameData, WrappedPointer<SkillTemplate> skillTemplateData)
    {
        // Reentrancy guard — PopulateSkillTemplatePreview internally calls the game's
        // PopulateSkillData which re-enters this hook for each child frame we create.
        if (this.isPopulatingExtraBuilds)
        {
            return false;
        }

        var scopedLogger = this.logger.CreateScopedLogger();
        if (this.cachedTemplateCode is null)
        {
            this.CleanupFloatingPreview();
            return false;
        }

        if (!this.buildTemplateManager.TryDecodeTemplate(this.cachedTemplateCode, out var build) ||
            build is not TeamBuildEntry teamBuild ||
            teamBuild.Builds.Count <= 1)
        {
            this.CleanupFloatingPreview();
            return false;
        }

        // If we already have a floating preview for this template, don't recreate
        if (this.floatingPreviewFrame != 0)
        {
            return false;
        }

        scopedLogger.LogDebug(
            "Creating floating preview for {count} extra builds",
            teamBuild.Builds.Count - 1);

        // Create a floating frame to hold the extra build previews
        fixed (char* label = "DaybreakTeamPreview")
        {
            var floatingFrame = GWCA.GW.FrameMgr.CreateFloatingFrame(
                (ushort*)label, 0, 0);

            if (floatingFrame is null)
            {
                scopedLogger.LogWarning("Failed to create floating preview frame");
                return false;
            }

            this.floatingPreviewFrame = (nint)floatingFrame;

            this.isPopulatingExtraBuilds = true;
            try
            {
                // Create a child frame for each build beyond the first and populate it
                for (var i = 1; i < teamBuild.Builds.Count; i++)
                {
                    var childFrame = GWCA.GW.FrameMgr.CreateChildFrame(
                        floatingFrame, 0, (uint)i, 0, 0, null);

                    if (childFrame is null)
                    {
                        scopedLogger.LogWarning("Failed to create child frame for build {index}", i);
                        continue;
                    }

                    var template = new SkillTemplate();
                    PopulateSkillTemplate(teamBuild.Builds[i], &template);

                    if (!GWCA.GW.FrameMgr.PopulateSkillTemplatePreview(childFrame, &template, 0))
                    {
                        scopedLogger.LogWarning("Failed to populate preview for build {index}", i);
                        continue;
                    }

                    scopedLogger.LogDebug("Created and populated preview for build {index}", i);
                }
            }
            finally
            {
                this.isPopulatingExtraBuilds = false;
            }

            // Position the floating preview relative to the template dialog
            var dialogFrame = GWCA.GW.FrameMgr.GetTemplateDialogFrame();
            if (dialogFrame is not null)
            {
                GWCA.GW.FrameMgr.PositionRelativeTo(floatingFrame, dialogFrame, 0f, -1f);
            }
        }

        // Let the original proceed for build 0 on the existing frame
        return false;
    }

    private unsafe void CleanupFloatingPreview()
    {
        if (this.floatingPreviewFrame != 0)
        {
            var frame = (Frame*)this.floatingPreviewFrame;
            GWCA.GW.FrameMgr.DestroyFloatingFrame(frame);
            this.floatingPreviewFrame = 0;
        }
    }

    private async Task<bool> IsInValidOutpost(CancellationToken cancellationToken)
    {
        return await this.gameThreadService.QueueOnGameThread(() =>
        {
            return this.instanceContextService.GetInstanceType() is InstanceType.Outpost;
        }, cancellationToken);
    }

    private bool SpawnHeroes(TeamBuildEntry partyLoadout)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Spawning {heroCount} heroes for party loadout", partyLoadout.PartyComposition?.AsValueEnumerable().Count(c => c.HeroId != 0) ?? 0);
        foreach ((var index, var entry) in partyLoadout.Builds.AsValueEnumerable().OrderBy(e => e.Primary.Id)
                    .Select((e, i) => (e, partyLoadout.PartyComposition?.First(e => e.Index == i)))
                    .OfType<(SingleBuildEntry, PartyCompositionMetadataEntry)>()
                    .Where(t => t.Item2.Type is PartyCompositionMemberType.Hero))
        {
            if (entry.HeroId is not null &&
                entry.HeroId != 0 &&
                Hero.TryParse(entry.HeroId.Value, out var hero))
            {
                scopedLogger.LogInformation("Adding hero [{heroId}] [{heroName}] with behavior {behavior}", entry.HeroId, hero.Name, entry.Behavior ?? Shared.Models.Api.HeroBehavior.Guard);
                this.partyContextService.AddHero((uint)entry.HeroId);
            }
            else
            {
                scopedLogger.LogWarning("Invalid hero entry in party loadout: {heroId}", entry.HeroId ?? -1);
                continue;
            }
        }

        return true;
    }

    private unsafe bool ApplyBuilds(TeamBuildEntry partyLoadout)
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

        for (var buildIndex = 0; buildIndex < partyLoadout.Builds.Count; buildIndex++)
        {
            var build = partyLoadout.Builds[buildIndex];
            var composition = partyLoadout.PartyComposition?.FirstOrDefault(c => c.Index == buildIndex);
            if (composition is null)
            {
                continue;
            }

            uint agentId;
            if (composition.HeroId is 0 or null)
            {
                agentId = playerId.Value;
            }
            else
            {
                var hero = heroes.Value.AsValueEnumerable().FirstOrDefault(h => (int)h.HeroId == composition.HeroId.GetValueOrDefault());
                agentId = hero.AgentId;
            }

            var professionContext = professions.Value.AsValueEnumerable().FirstOrDefault(p => p.AgentId == agentId);

            var validSkills = agentId == playerId
                ? unlockedSkills
                : accountContext.Pointer->UnlockedAccountSkills;

            var unlockedProfessionsFlags = agentId == playerId
                ? playerProfessionContext.UnlockedProfessions
                : uint.MaxValue;

            if (!this.buildTemplateManager.CanTemplateApply(
                new BuildTemplateValidationRequest(
                    (uint)build.Primary.Id,
                    (uint)build.Secondary.Id,
                    [.. build.Skills.Select(s => (uint)s.Id)],
                    (uint)(int)professionContext.Primary,
                    unlockedProfessionsFlags,
                    [.. validSkills])))
            {
                if (composition.HeroId is 0 or null)
                {
                    scopedLogger.LogError("Cannot apply build for player");
                }
                else
                {
                    scopedLogger.LogError("Cannot apply build for hero {heroId}", composition.HeroId);
                }

                continue;
            }

            var skillTemplate = new SkillTemplate();
            PopulateSkillTemplate(build, &skillTemplate);
            scopedLogger.LogInformation("Applying build for agent {agentId} with primary {primary} and secondary {secondary}", agentId, build.Primary, build.Secondary);
            this.skillbarContextService.LoadBuild(agentId, &skillTemplate);
        }

        return true;
    }

    private unsafe List<(uint AgentId, Shared.Models.Api.HeroBehavior Behavior)>? GetHeroBehaviorSetup(TeamBuildEntry teamBuild)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Getting hero behavior setup");

        if (!this.gameContextService.GetGameContext().TryGetPlayerParty(out _, out _, out var heroes, out _) ||
            !heroes.HasValue)
        {
            scopedLogger.LogError("Failed to get player party");
            return default;
        }

        if (teamBuild.PartyComposition is null)
        {
            scopedLogger.LogWarning("No party composition metadata in team build");
            return default;
        }

        return heroes.Value.AsValueEnumerable()
            .Select(h =>
            {
                if (h.HeroId is API.Interop.GWCA.GW.Constants.HeroID.NoHero)
                {
                    return default;
                }

                var entry = teamBuild.PartyComposition.AsValueEnumerable().FirstOrDefault(e => e.HeroId == (int)h.HeroId);
                if (entry is null)
                {
                    scopedLogger.LogWarning("No entry found for hero {heroId}", h.HeroId);
                    return default;
                }

                return (h.AgentId, entry.Behavior ?? Shared.Models.Api.HeroBehavior.Guard);
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

                var charContext = gameContext.Pointer->Character;
                if (charContext is null)
                {
                    scopedLogger.LogError("Char context is null");
                    return -1;
                }

                var playerParty = GWCA.GW.PartyMgr.GetPartyInfo(0);
                var playerId = charContext->PlayerNumber;
                var offset = 0;
                foreach (var hero in playerParty->Heroes)
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

    private static BuildEntry? GetBuildEntryById(ProfessionState professionContext, SkillbarData skillbar, PartyAttribute attributes)
    {
        return new BuildEntry(
            Primary: (int)professionContext.Primary,
            Secondary: (int)professionContext.Secondary,
            Attributes: attributes.Attribute.GetAttributeEntryList().ToList(),
            Skills:
            [
                (uint)skillbar.Skills[0].SkillId,
                (uint)skillbar.Skills[1].SkillId,
                (uint)skillbar.Skills[2].SkillId,
                (uint)skillbar.Skills[3].SkillId,
                (uint)skillbar.Skills[4].SkillId,
                (uint)skillbar.Skills[5].SkillId,
                (uint)skillbar.Skills[6].SkillId,
                (uint)skillbar.Skills[7].SkillId
            ]);
    }

    private static unsafe void PopulateSkillTemplate(SingleBuildEntry build, SkillTemplate* skillTemplate)
    {
        var attributeIds = new AttributeArray12();
            var attributeValues = new Array12Uint();
            for (var i = 0; i < build.Attributes.Count && i < 12; i++)
            {
                attributeIds[i] = (GWCA.GW.Constants.Attribute)(build.Attributes[i].Attribute?.Id ?? 0);
                attributeValues[i] = (uint)build.Attributes[i].Points;
            }

            var skills = new SkillIDArray8();
            for (var i = 0; i < build.Skills.Count && i < 8; i++)
            {
                skills[i] = (GWCA.GW.Constants.SkillID)build.Skills[i].Id;
            }

            skillTemplate->Primary = (GWCA.GW.Constants.Profession)build.Primary.Id;
            skillTemplate->Secondary = (GWCA.GW.Constants.Profession)build.Secondary.Id;
            skillTemplate->AttributesCount = (uint)Math.Min(build.Attributes.Count, 12);
            skillTemplate->AttributeIds = attributeIds;
            skillTemplate->Skills = skills;
            Unsafe.CopyBlock(skillTemplate->AttributeValues, Unsafe.AsPointer(ref attributeValues), (uint)(12 * sizeof(uint)));
    }
}
