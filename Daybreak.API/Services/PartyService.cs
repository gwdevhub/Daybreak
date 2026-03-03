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
    private readonly List<uint> extraSummaryFrameIds = [];
    private readonly List<(uint FrameId, float OriginalBottom)> expandedAncestors = [];
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
    /// Creates additional skill summary child frames under the same parent,
    /// one per build in the team loadout, so all builds are displayed vertically.
    /// </summary>
    private unsafe bool PopulateSkillData(WrappedPointer<TemplateSummaryFrameData> frameData, WrappedPointer<SkillTemplate> skillTemplateData)
    {
        // Reentrancy guard — our SendFrameUIMessage(0x55) calls re-enter this hook
        if (this.isPopulatingExtraBuilds)
        {
            return false;
        }

        var scopedLogger = this.logger.CreateScopedLogger();
        if (this.cachedTemplateCode is null)
        {
            this.CleanupExtraSummaryFrames();
            return false;
        }

        if (!this.buildTemplateManager.TryDecodeTemplate(this.cachedTemplateCode, out var build) ||
            build is not TeamBuildEntry teamBuild ||
            teamBuild.Builds.Count <= 1)
        {
            this.CleanupExtraSummaryFrames();
            return false;
        }

        // Clean up any previously created extra frames
        this.CleanupExtraSummaryFrames();

        var childFramePtr = GWCA.GW.UI.GetFrameById(frameData.Pointer->FrameId);
        if (childFramePtr is null)
        {
            scopedLogger.LogWarning("Could not resolve child frame from FrameId");
            return false;
        }

        // Extract the SkillFrameHandler address from the child frame's callback table.
        // Frame + 0xA8 = GW::Array<FrameInteractionCallback>
        // Array.m_buffer at offset 0, FrameInteractionCallback[0].callback at offset 0
        var callbackBuffer = *(nint*)((byte*)childFramePtr + 0xA8);
        if (callbackBuffer == 0)
        {
            scopedLogger.LogWarning("Could not read frame callback buffer");
            return false;
        }

        var handlerAddress = *(nint*)callbackBuffer;

        // Get parent frame and its FrameId for CreateUIComponent
        var parentFrame = GWCA.GW.UI.GetParentFrame(childFramePtr);
        if (parentFrame is null)
        {
            scopedLogger.LogWarning("Could not get parent frame");
            return false;
        }

        var parentFrameId = parentFrame->FrameId;

        scopedLogger.LogDebug(
            "Creating {count} extra skill summary frames under parent FrameId={parentFrameId}",
            teamBuild.Builds.Count - 1, parentFrameId);

        this.isPopulatingExtraBuilds = true;
        try
        {
            for (var i = 1; i < teamBuild.Builds.Count; i++)
            {
                // Use child offset IDs starting at 100 to avoid conflicting with existing children
                var childId = (uint)(100 + i);

                var newFrameId = GWCA.GW.UI.CreateUIComponent(
                    parentFrameId, 0x300, childId,
                    handlerAddress, 0, null);

                if (newFrameId is 0)
                {
                    scopedLogger.LogWarning("Failed to create extra summary frame for build {index}", i);
                    continue;
                }

                this.extraSummaryFrameIds.Add(newFrameId);

                var newFrame = GWCA.GW.UI.GetFrameById(newFrameId);
                if (newFrame is null)
                {
                    scopedLogger.LogWarning("Failed to get frame for build {index}", i);
                    continue;
                }

                // Get the TemplateSummaryFrameData* allocated by the frame handler on create (msg 0x09)
                var newFrameData = (TemplateSummaryFrameData*)GWCA.GW.UI.GetFrameContext(newFrame);
                if (newFrameData is null)
                {
                    scopedLogger.LogWarning("Failed to get frame context for build {index}", i);
                    continue;
                }

                // Build the SkillTemplate for this build entry
                var template = new SkillTemplate();
                PopulateSkillTemplate(teamBuild.Builds[i], &template);

                // Call the original PopulateSkillData directly on the new frame's data
                this.skillbarContextService.CallOriginalPopulateSkillData(newFrameData, &template);

                scopedLogger.LogDebug(
                    "Created and populated frame {index}: FrameId={frameId}",
                    i, newFrameId);
            }

            // Defer positioning to the next game tick. Right now all position data is zero
            // because PopulateSkillData fires during frame creation, before the layout pass.
            // On the next tick, ProcessLayoutQueue will have computed screen coordinates
            // for the original frame, so we can read its position and stack our frames below it.
            // We use forceEnqueue: true so the callback is deferred to the next game loop
            // iteration rather than executing immediately (which would still be before layout).
            var originalFrameId = frameData.Pointer->FrameId;
            var extraFrameIds = this.extraSummaryFrameIds.ToArray();
            _ = this.gameThreadService.QueueOnGameThread(() =>
            {
                this.PositionExtraFrames(originalFrameId, extraFrameIds, retriesRemaining: 20);
            }, CancellationToken.None, forceEnqueue: true);
        }
        finally
        {
            this.isPopulatingExtraBuilds = false;
        }

        // Let the original proceed for build 0 on the existing frame
        return false;
    }

    /// <summary>
    /// Deferred callback that runs on a later game tick after PopulateSkillData.
    /// After the layout pass, FramePositionData at offset 0xD8 will have valid Screen*
    /// coordinates. We read the original frame's position data and replicate it on
    /// extra frames, shifting each one down to stack vertically.
    ///
    /// Uses an iterative approach:
    ///   1. Wait for the original frame to have valid layout data
    ///   2. Position all extras below the original
    ///   3. Check if the lowest extra overflows the parent's content area
    ///   4. If it overflows, expand ancestors by exactly the overflow amount, wait a tick, repeat from 2
    ///   5. If everything fits, we're done
    /// </summary>
    private unsafe void PositionExtraFrames(uint originalFrameId, uint[] extraFrameIds, int retriesRemaining)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var origFrame = GWCA.GW.UI.GetFrameById(originalFrameId);
        if (origFrame is null)
        {
            scopedLogger.LogWarning("Original frame {frameId} no longer valid for positioning", originalFrameId);
            return;
        }

        // Read FramePositionData at offset 0xD8 (the Position field on Frame struct).
        // After the layout pass, Screen* fields should be non-zero for visible frames.
        var origPos = (FramePositionData*)((byte*)origFrame + 0xD8);

        var screenHeight = origPos->ScreenTop - origPos->ScreenBottom;

        scopedLogger.LogDebug(
            "Iter {iter}: FrameId={fid} " +
            "Pos=[flags={pf:X}, L={pl}, B={pb}, R={pr}, T={pt}] " +
            "Content=[L={cl}, B={cb}, R={cr}, T={ct}] " +
            "Screen=[L={sl}, B={sb}, R={sr}, T={st}]",
            20 - retriesRemaining, originalFrameId,
            origPos->Flags, origPos->Left, origPos->Bottom, origPos->Right, origPos->Top,
            origPos->ContentLeft, origPos->ContentBottom, origPos->ContentRight, origPos->ContentTop,
            origPos->ScreenLeft, origPos->ScreenBottom, origPos->ScreenRight, origPos->ScreenTop);

        if (screenHeight <= 0f)
        {
            if (retriesRemaining <= 0)
            {
                scopedLogger.LogWarning(
                    "Original frame still has no layout dimensions after all retries, giving up");
                return;
            }

            // Re-queue for the next game tick with forceEnqueue so we actually wait
            _ = this.gameThreadService.QueueOnGameThread(() =>
            {
                this.PositionExtraFrames(originalFrameId, extraFrameIds, retriesRemaining - 1);
            }, CancellationToken.None, forceEnqueue: true);
            return;
        }

        // The original frame's position data has valid layout coordinates.
        // Position fields (Left/Bottom/Right/Top) are relative to the parent's content rect.
        var rowHeight = origPos->Top - origPos->Bottom;

        scopedLogger.LogDebug(
            "Positioning: origPos=[L={l}, B={b}, R={r}, T={t}] rowH={rh} extras={n}",
            origPos->Left, origPos->Bottom, origPos->Right, origPos->Top,
            rowHeight, extraFrameIds.Length);

        // Position all extra frames below the original, stacking downward.
        for (var i = 0; i < extraFrameIds.Length; i++)
        {
            var extraFrame = GWCA.GW.UI.GetFrameById(extraFrameIds[i]);
            if (extraFrame is null)
            {
                continue;
            }

            var extraPos = (FramePositionData*)((byte*)extraFrame + 0xD8);

            // In GW's coordinate system, Y increases upward, so we subtract
            // to place frames below the original.
            extraPos->Flags = origPos->Flags;
            extraPos->Left = origPos->Left;
            extraPos->Right = origPos->Right;
            extraPos->Bottom = origPos->Bottom - ((i + 1) * rowHeight);
            extraPos->Top = origPos->Top - ((i + 1) * rowHeight);

            scopedLogger.LogDebug(
                "Extra {index} (id={fid}): Pos=[L={l}, B={b}, R={r}, T={t}]",
                i + 1, extraFrameIds[i],
                extraPos->Left, extraPos->Bottom, extraPos->Right, extraPos->Top);

            GWCA.GW.UI.TriggerFrameRedraw(extraFrame);
        }

        // Now check if the lowest extra frame overflows the parent's content area.
        // The lowest extra has Bottom = origBottom - (count * rowHeight).
        // If that Bottom is negative (below the parent's content origin at 0), the
        // parent's content area doesn't extend far enough. We need to check against
        // the parent's actual content rect.
        var lowestBottom = origPos->Bottom - (extraFrameIds.Length * rowHeight);
        var parentFrame = GWCA.GW.UI.GetParentFrame(origFrame);

        if (parentFrame is null)
        {
            scopedLogger.LogDebug("No parent frame, skipping overflow check");
            return;
        }

        var parentPos = (FramePositionData*)((byte*)parentFrame + 0xD8);

        // The parent's content rect in its own coordinate space: children are
        // positioned relative to (ContentLeft, ContentBottom). A child at Bottom=0
        // sits at the parent's ContentBottom edge. The usable height is
        // ContentTop - ContentBottom. Children are placed from Top downward, so
        // the lowest child's Bottom must be >= 0 in the parent's content space.
        // Since position values are relative to the parent content rect, Bottom < 0
        // means the child extends below the parent's content area.
        var overflow = -lowestBottom; // positive if lowestBottom < 0

        scopedLogger.LogDebug(
            "Overflow check: lowestBottom={lb}, overflow={ov}, " +
            "parent(id={pid}) Content=[L={cl}, B={cb}, R={cr}, T={ct}] " +
            "Pos=[L={pl}, B={pb}, R={pr}, T={pt}]",
            lowestBottom, overflow,
            parentFrame->FrameId,
            parentPos->ContentLeft, parentPos->ContentBottom, parentPos->ContentRight, parentPos->ContentTop,
            parentPos->Left, parentPos->Bottom, parentPos->Right, parentPos->Top);

        if (overflow <= 0f)
        {
            // Everything fits, we're done!
            scopedLogger.LogDebug("All extras fit within parent bounds, done");
            return;
        }

        if (retriesRemaining <= 0)
        {
            scopedLogger.LogWarning(
                "Still overflowing by {overflow} after all retries, giving up", overflow);
            return;
        }

        // Expand ancestor frames by exactly the overflow amount.
        // Walk up 2 levels (content container + dialog frame). Skip deeper ancestors
        // to avoid pushing the entire viewport.
        scopedLogger.LogDebug("Expanding ancestors by {overflow} to fit content", overflow);
        var ancestorFrame = parentFrame;
        for (var depth = 0; depth < 2 && ancestorFrame is not null; depth++)
        {
            var ancestorPos = (FramePositionData*)((byte*)ancestorFrame + 0xD8);

            // Only record the original Bottom on the first expansion so we can
            // restore it during cleanup. If we've already recorded this ancestor,
            // don't overwrite the original value.
            if (!this.expandedAncestors.Any(a => a.Item1 == ancestorFrame->FrameId))
            {
                this.expandedAncestors.Add((ancestorFrame->FrameId, ancestorPos->Bottom));
            }

            ancestorPos->Bottom -= overflow;

            scopedLogger.LogDebug(
                "Expanded ancestor depth {depth} (id={fid}): Bottom -> {newB}",
                depth, ancestorFrame->FrameId, ancestorPos->Bottom);

            GWCA.GW.UI.TriggerFrameRedraw(ancestorFrame);
            ancestorFrame = GWCA.GW.UI.GetParentFrame(ancestorFrame);
        }

        // Wait a tick for the layout system to process the expansion, then re-check.
        // The layout pass may reposition the original frame, so we need to re-read
        // its position and re-stack the extras.
        _ = this.gameThreadService.QueueOnGameThread(() =>
        {
            this.PositionExtraFrames(originalFrameId, extraFrameIds, retriesRemaining - 1);
        }, CancellationToken.None, forceEnqueue: true);
    }

    private unsafe void CleanupExtraSummaryFrames()
    {
        // Restore ancestor frame positions that we expanded
        foreach (var (frameId, originalBottom) in this.expandedAncestors)
        {
            var frame = GWCA.GW.UI.GetFrameById(frameId);
            if (frame is not null)
            {
                var pos = (FramePositionData*)((byte*)frame + 0xD8);
                pos->Bottom = originalBottom;
                GWCA.GW.UI.TriggerFrameRedraw(frame);
            }
        }

        this.expandedAncestors.Clear();

        // Destroy extra child frames
        foreach (var frameId in this.extraSummaryFrameIds)
        {
            var frame = GWCA.GW.UI.GetFrameById(frameId);
            if (frame is not null)
            {
                GWCA.GW.UI.DestroyUIComponent(frame);
            }
        }

        this.extraSummaryFrameIds.Clear();
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
