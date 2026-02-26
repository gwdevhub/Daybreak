using Daybreak.API.Extensions;
using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using Daybreak.API.Services.Interop;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Services.BuildTemplates;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;
using System.Runtime.CompilerServices;
using ZLinq;
using InstanceType = Daybreak.API.Interop.GuildWars.InstanceType;

namespace Daybreak.API.Services;

public sealed class MainPlayerService : IDisposable
{
    private readonly ChatService chatService;
    private readonly InstanceContextService instanceContextService;
    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly SkillbarContextService skillbarContextService;
    private readonly AgentContextService agentContextService;
    private readonly GameContextService gameContextService;
    private readonly GameThreadService gameThreadService;
    private readonly ILogger<MainPlayerService> logger;

    public MainPlayerService(
        ChatService chatService,
        InstanceContextService instanceContextService,
        IBuildTemplateManager buildTemplateManager,
        SkillbarContextService skillbarContextService,
        GameContextService gameContextService,
        AgentContextService agentContextService,
        GameThreadService gameThreadService,
        ILogger<MainPlayerService> logger)
    {
        this.chatService = chatService.ThrowIfNull();
        this.instanceContextService = instanceContextService.ThrowIfNull();
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.skillbarContextService = skillbarContextService.ThrowIfNull();
        this.gameThreadService = gameThreadService.ThrowIfNull();
        this.agentContextService = agentContextService.ThrowIfNull();
        this.gameContextService = gameContextService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public void Dispose()
    {
    }

    public async Task<bool> SetCurrentBuild(string buildCode, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (!this.buildTemplateManager.TryDecodeTemplate(buildCode, out var build) ||
            build is not SingleBuildEntry singleBuild)
        {
            scopedLogger.LogError("Failed to decode build template from code {buildCode}", buildCode);
            return false;
        }

        var result = await this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                if (this.instanceContextService.GetInstanceType() is InstanceType.Loading or InstanceType.Explorable)
                {
                    scopedLogger.LogError("Not in outpost");
                    return default;
                }

                var gameContext = this.gameContextService.GetGameContext();
                if (gameContext.IsNull ||
                    gameContext.Pointer->World is null)
                {
                    scopedLogger.LogError("Failed to get game context");
                    return false;
                }

                var playerAgentId = this.agentContextService.GetPlayerAgentId();
                if (playerAgentId is 0x0)
                {
                    scopedLogger.LogError("Failed to get player agent id");
                    return false;
                }

                var playerAgent = this.agentContextService.GetAgentById(playerAgentId);
                if (playerAgent is null ||
                    playerAgent->Type is not (uint)AgentType.Living ||
                    playerAgent->AgentId != playerAgentId)
                {
                    scopedLogger.LogError("Player agent {playerAgentId} not found in agent array", playerAgentId);
                    return false;
                }

                var livingAgent = (AgentLiving*)playerAgent;
                if (livingAgent->Level != gameContext.Pointer->World->Level)
                {
                    scopedLogger.LogError("Player agent not found. Player level mismatch: {level} != {gameLevel}", livingAgent->Level, gameContext.Pointer->World->Level);
                    return false;
                }

                var agentProfession = gameContext.Pointer->World->PartyProfessionStates.AsValueEnumerable().FirstOrDefault(p => p.AgentId == playerAgentId);
                if (agentProfession.AgentId != playerAgentId)
                {
                    scopedLogger.LogError("Failed to find agent profession for player agent id {agentId}", playerAgentId);
                    return false;
                }

                var validationRequest = new BuildTemplateValidationRequest(
                    (uint)singleBuild.Primary.Id,
                    (uint)singleBuild.Secondary.Id,
                    singleBuild.Skills.AsValueEnumerable().Select(s => (uint)s.Id).ToArray(),
                    livingAgent->Tags->Primary,
                    agentProfession.UnlockedProfessions,
                    [.. gameContext.Pointer->World->UnlockedCharacterSkills]);

                if (!this.buildTemplateManager.CanTemplateApply(validationRequest))
                {
                    scopedLogger.LogError("Build template validation failed for player agent id {agentId}", playerAgentId);
                    return false;
                }

                var attributeIds = new AttributeArray12();
                var attributeValues = new Array12Uint();
                var skills = new SkillIDArray8();
                for (var i = 0; i < singleBuild.Attributes.Count && i < 12; i++)
                {
                    if (singleBuild.Attributes[i].Attribute is null)
                    {
                        continue;
                    }

                    attributeIds[i] = (GWCA.GW.Constants.Attribute)singleBuild.Attributes[i].Attribute!.Id;
                    attributeValues[i] = (uint)singleBuild.Attributes[i].Points;
                }

                for (var i = 0; i < singleBuild.Skills.Count && i < 8; i++)
                {
                    skills[i] = (GWCA.GW.Constants.SkillID)singleBuild.Skills[i].Id;
                }

                var skillTemplate = new SkillTemplate
                {
                    Primary = (GWCA.GW.Constants.Profession)singleBuild.Primary.Id,
                    Secondary = (GWCA.GW.Constants.Profession)singleBuild.Secondary.Id,
                    AttributesCount = (uint)Math.Min(singleBuild.Attributes.Count, 12),
                    AttributeIds = attributeIds,
                    Skills = skills,
                };
                Unsafe.CopyBlock(skillTemplate.AttributeValues, Unsafe.AsPointer(ref attributeValues), (uint)(12 * sizeof(uint)));
                this.skillbarContextService.LoadBuild(playerAgentId, &skillTemplate);
                return true;
            }
        }, cancellationToken);

        if (result)
        {
            await this.chatService.AddMessageAsync("Build applied successfully.", "Daybreak.API", Channel.CHANNEL_MODERATOR, cancellationToken);
        }
        else
        {
            await this.chatService.AddMessageAsync("Failed to apply build.", "Daybreak.API", Channel.CHANNEL_MODERATOR, cancellationToken);
        }

        return result;
    }

    public Task<BuildEntry?> GetCurrentBuild(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        return this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                if (this.instanceContextService.GetInstanceType() is InstanceType.Loading)
                {
                    scopedLogger.LogError("Not loaded");
                    return default;
                }

                var gameContext = this.gameContextService.GetGameContext();
                if (gameContext.IsNull)
                {
                    scopedLogger.LogError("Failed to get game context");
                    return default;
                }

                var playerAgentId = this.agentContextService.GetPlayerAgentId();
                if (playerAgentId is 0x0)
                {
                    scopedLogger.LogError("Failed to get player agent id");
                    return default;
                }

                var skillbarContext = gameContext.Pointer->World->Skillbar.Value.AsValueEnumerable().FirstOrDefault(s => s.AgentId == playerAgentId);
                if (skillbarContext.AgentId != playerAgentId)
                {
                    scopedLogger.LogError("Failed to find skillbar context for player agent id {agentId}", playerAgentId);
                    return default;
                }

                var agentAttributes = gameContext.Pointer->World->Attributes.Value.AsValueEnumerable().FirstOrDefault(a => a.AgentId == playerAgentId);
                if (agentAttributes.AgentId != playerAgentId)
                {
                    scopedLogger.LogError("Failed to find agent attributes for player agent id {agentId}", playerAgentId);
                    return default;
                }

                var professionContext = gameContext.Pointer->World->PartyProfessionStates.AsValueEnumerable().FirstOrDefault(p => p.AgentId == playerAgentId);
                if (professionContext.AgentId != playerAgentId)
                {
                    scopedLogger.LogError("Failed to find agent profession for player agent id {agentId}", playerAgentId);
                    return default;
                }

                return new BuildEntry(
                    (int)professionContext.Primary,
                    (int)professionContext.Secondary,
                    [.. agentAttributes.Attribute.GetAttributeEntryList()],
                    [(uint)skillbarContext.Skills[0].SkillId, (uint)skillbarContext.Skills[1].SkillId, (uint)skillbarContext.Skills[2].SkillId, (uint)skillbarContext.Skills[3].SkillId,
                    (uint)skillbarContext.Skills[4].SkillId, (uint)skillbarContext.Skills[5].SkillId, (uint)skillbarContext.Skills[6].SkillId, (uint)skillbarContext.Skills[7].SkillId]);
            }
        }, cancellationToken);
    }

    public Task<QuestLogInformation?> GetQuestLog(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        return this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                if (this.instanceContextService.GetInstanceType() is InstanceType.Loading)
                {
                    scopedLogger.LogError("Not loaded");
                    return default;
                }

                var gameContext = this.gameContextService.GetGameContext();
                if (gameContext.IsNull || gameContext.Pointer->World is null)
                {
                    scopedLogger.LogError("Game context is not initialized");
                    return default;
                }

                return new QuestLogInformation(
                    (uint)gameContext.Pointer->World->ActiveQuestId,
                    gameContext.Pointer->World->QuestLog.Value.AsValueEnumerable().Select(q => new QuestInformation((uint)q.QuestId, (uint)q.MapFrom)).ToList());
            }
        }, cancellationToken);
    }

    public Task<MainPlayerInformation?> GetMainPlayerInformation(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        return this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                if (this.instanceContextService.GetInstanceType() is InstanceType.Loading)
                {
                    scopedLogger.LogError("Not loaded");
                    return default;
                }

                var gameContext = this.gameContextService.GetGameContext();
                if (gameContext.IsNull || gameContext.Pointer->Character is null ||
                    gameContext.Pointer->World is null)
                {
                    scopedLogger.LogError("Game context is not initialized");
                    return default;
                }

                var playerName = new string (gameContext.Pointer->Character->PlayerName);
                var email = new string(gameContext.Pointer->Character->PlayerEmail);
                var accountName = new string((char*)gameContext.Pointer->World->AccountInfo->AccountName);
                return new MainPlayerInformation(
                    (*(Uuid*)gameContext.Pointer->Character->PlayerUuid).ToString(),
                    email,
                    playerName,
                    accountName,
                    gameContext.Pointer->World->AccountInfo->Wins,
                    gameContext.Pointer->World->AccountInfo->Losses,
                    gameContext.Pointer->World->AccountInfo->Rating,
                    gameContext.Pointer->World->AccountInfo->QualifierPoints,
                    gameContext.Pointer->World->AccountInfo->Rank,
                    gameContext.Pointer->World->AccountInfo->TournamentRewardPoints);
            }
        }, cancellationToken);
    }

    public Task<MainPlayerState?> GetMainPlayerState(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        return this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                if (this.instanceContextService.GetInstanceType() is InstanceType.Loading)
                {
                    scopedLogger.LogError("Not loaded");
                    return default;
                }

                var gameContext = this.gameContextService.GetGameContext();
                var playerAgentId = this.agentContextService.GetPlayerAgentId();
                if (gameContext.IsNull ||
                    gameContext.Pointer->World is null ||
                    gameContext.Pointer->Character is null ||
                    playerAgentId is 0x0)
                {
                    scopedLogger.LogDebug("Game data is not yet initialized");
                    return default;
                }

                var playerAgent = this.agentContextService.GetAgentById(playerAgentId);
                if (playerAgent is null ||
                    playerAgent->Type is not (uint)AgentType.Living ||
                    playerAgent->AgentId != playerAgentId)
                {
                    scopedLogger.LogError("Player agent {playerAgentId} not found in agent array", playerAgentId);
                    return default;
                }

                var livingAgent = (AgentLiving*)playerAgent;
                if (livingAgent->Level != gameContext.Pointer->World->Level)
                {
                    scopedLogger.LogError("Player agent not found. Player level mismatch: {level} != {gameLevel}", livingAgent->Level, gameContext.Pointer->World->Level);
                    return default;
                }

                return new MainPlayerState(
                    gameContext.Pointer->World->Experience,
                    gameContext.Pointer->World->Level,

                    gameContext.Pointer->World->CurrentLuxon,
                    gameContext.Pointer->World->CurrentKurzick,
                    gameContext.Pointer->World->CurrentImperial,
                    gameContext.Pointer->World->CurrentBalth,
                    gameContext.Pointer->World->MaxLuxon,
                    gameContext.Pointer->World->MaxKurzick,
                    gameContext.Pointer->World->MaxImperial,
                    gameContext.Pointer->World->MaxBalth,
                    gameContext.Pointer->World->TotalEarnedLuxon,
                    gameContext.Pointer->World->TotalEarnedKurzick,
                    gameContext.Pointer->World->TotalEarnedImperial,
                    gameContext.Pointer->World->TotalEarnedBalth,

                    // Energy and health are percentages of Max
                    livingAgent->Hp * livingAgent->MaxHp,
                    livingAgent->MaxHp,
                    livingAgent->Energy * livingAgent->MaxEnergy,
                    livingAgent->MaxEnergy,

                    livingAgent->Tags->Primary,
                    livingAgent->Tags->Secondary,

                    playerAgent->Pos.X,
                    playerAgent->Pos.Y
                );
            }
        }, cancellationToken);
    }

    public Task<InstanceInfo> GetMainPlayerInstance(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        return this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                var instanceType = this.instanceContextService.GetInstanceType();
                if (instanceType is InstanceType.Loading)
                {
                    return new InstanceInfo(0, 0, 0, 0, Shared.Models.Api.InstanceType.Loading, DistrictRegionInfo.Unknown, LanguageInfo.Unknown, CampaignInfo.Unknown, ContinentInfo.Unknown, RegionInfo.Unknown, DifficultyInfo.Unknown);
                }

                var gameContext = this.gameContextService.GetGameContext();
                var currentMapInfo = this.instanceContextService.GetAreaInfo();
                var serverRegion = this.instanceContextService.GetServerRegion();
                if (gameContext.IsNull ||
                    gameContext.Pointer->Character is null ||
                    gameContext.Pointer->World is null ||
                    gameContext.Pointer->Party is 0 ||
                    currentMapInfo.IsNull)
                {
                    scopedLogger.LogError("Game context is not initialized");
                    return new InstanceInfo(0, 0, 0, 0, Shared.Models.Api.InstanceType.Loading, DistrictRegionInfo.Unknown, LanguageInfo.Unknown, CampaignInfo.Unknown, ContinentInfo.Unknown, RegionInfo.Unknown, DifficultyInfo.Unknown);
                }

                var charContext = *gameContext.Pointer->Character;
                var worldContext = *gameContext.Pointer->World;
                var mapInfo = *currentMapInfo.Pointer;
                var mapId = charContext.MapId;
                var language = charContext.Language;
                var districtNumber = charContext.DistrictNumber;
                var foesKilled = worldContext.FoesKilled;
                var foesToKill = worldContext.FoesToKill;

                return new InstanceInfo(
                    MapId: (uint)charContext.MapId,
                    DistrictNumber: charContext.DistrictNumber,
                    FoesKilled: worldContext.FoesKilled,
                    FoesToKill: worldContext.FoesToKill,
                    Type: (Shared.Models.Api.InstanceType)instanceType,
                    DistrictRegion: (DistrictRegionInfo)serverRegion,
                    Language: (LanguageInfo)charContext.Language,
                    Campaign: (CampaignInfo)mapInfo.Campaign,
                    Continent: (ContinentInfo)mapInfo.Continent,
                    Region: (RegionInfo)mapInfo.Region,
                    Difficulty: GWCA.GW.PartyMgr.GetIsPartyInHardMode() ? DifficultyInfo.Hard : DifficultyInfo.Normal);
            }
        }, cancellationToken);
    }

    public Task<TitleInfo?> GetTitleInfo(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        return this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                if (this.instanceContextService.GetInstanceType() is InstanceType.Loading)
                {
                    scopedLogger.LogError("Not loaded");
                    return default;
                }

                var gameContext = this.gameContextService.GetGameContext();
                if (gameContext.IsNull || gameContext.Pointer->World is null)
                {
                    scopedLogger.LogError("Game context is not initialized");
                    return default;
                }

                var playerIndex = gameContext.Pointer->Character->PlayerNumber;
                var player = GWCA.GW.PlayerMgr.GetPlayerByID(playerIndex);
                if (player is null)
                {
                    scopedLogger.LogError("Failed to get player");
                    return default;
                }

                var currentTier = player->ActiveTitleTier;
                Title? currentTitle = default;
                int? id = -1;
                for (var i = 0; i < gameContext.Pointer->World->Titles.Value.Size; i++)
                {
                    var title = gameContext.Pointer->World->Titles.Value.Skip(i).FirstOrDefault();
                    if (title.CurrentTitleTierIndex == currentTier)
                    {
                        currentTitle = title;
                        id = i;
                        break;
                    }
                }

                if (!currentTitle.HasValue)
                {
                    scopedLogger.LogError("Failed to find current title with tier {currentTier}", currentTier);
                    return default;
                }

                var titleTier = gameContext.Pointer->World->TitleTiers.Skip((int)currentTitle.Value.CurrentTitleTierIndex).FirstOrDefault();
                return new TitleInfo
                (
                    Id: (uint)id,
                    IsPercentage: currentTitle.Value.IsPercentageBased(),
                    PointsForCurrentRank: currentTitle.Value.PointsNeededCurrentRank,
                    PointsForNextRank: titleTier.TierNumber == currentTitle.Value.MaxTitleRank ?
                                        currentTitle.Value.CurrentPoints :
                                        currentTitle.Value.PointsNeededNextRank,
                    CurrentPoints: currentTitle.Value.CurrentPoints,
                    TierNumber: titleTier.TierNumber,
                    MaxTierNumber: currentTitle.Value.MaxTitleTierIndex
                );
            }
        }, cancellationToken);
    }

    public Task<MainPlayerBuildContext?> GetMainPlayerBuildContext(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        return this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                if (this.instanceContextService.GetInstanceType() is InstanceType.Loading)
                {
                    scopedLogger.LogError("Not loaded");
                    return default;
                }

                var gameContext = this.gameContextService.GetGameContext();
                if (gameContext.IsNull ||
                    gameContext.Pointer->Account is null ||
                    gameContext.Pointer->World is null)
                {
                    this.logger.LogError("Game context is not initialized");
                    return default;
                }

                var playerAgentId = this.agentContextService.GetPlayerAgentId();
                if (playerAgentId is 0x0)
                {
                    scopedLogger.LogError("Failed to get player agent id");
                    return default;
                }

                var agentProfession = gameContext.Pointer->World->PartyProfessionStates.AsValueEnumerable().FirstOrDefault(p => p.AgentId == playerAgentId);
                if (agentProfession.AgentId != playerAgentId)
                {
                    scopedLogger.LogError("Failed to find agent profession for player agent id {agentId}", playerAgentId);
                    return default;
                }

                return new MainPlayerBuildContext(
                    PrimaryProfessionId: (uint)agentProfession.Primary,
                    UnlockedProfessions: agentProfession.UnlockedProfessions,
                    UnlockedCharacterSkills: gameContext.Pointer->World->UnlockedCharacterSkills.AsValueEnumerable().ToArray());

            }
        }, cancellationToken);
    }
}
