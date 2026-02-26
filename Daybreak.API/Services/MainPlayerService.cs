using Daybreak.API.Extensions;
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
                    scopedLogger.LogError("Player agent not found. Player level mismatch: {level} != {gameLevel}", livingAgent->Level, gameContext.Pointer->WorldContext->Level);
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
                    livingAgent->Primary,
                    agentProfession.UnlockedProfessionsFlags,
                    [.. gameContext.Pointer->World->UnlockedCharacterSkills]);

                if (!this.buildTemplateManager.CanTemplateApply(validationRequest))
                {
                    scopedLogger.LogError("Build template validation failed for player agent id {agentId}", playerAgentId);
                    return false;
                }

                var attributeIds = new Array12Uint();
                var attributeValues = new Array12Uint();
                var skills = new Array8Uint();
                for (var i = 0; i < singleBuild.Attributes.Count && i < 12; i++)
                {
                    if (singleBuild.Attributes[i].Attribute is null)
                    {
                        continue;
                    }

                    attributeIds[i] = (uint)singleBuild.Attributes[i].Attribute!.Id;
                    attributeValues[i] = (uint)singleBuild.Attributes[i].Points;
                }

                for (var i = 0; i < singleBuild.Skills.Count && i < 8; i++)
                {
                    skills[i] = (uint)singleBuild.Skills[i].Id;
                }

                var skillTemplate = new SkillTemplate
                {
                    Primary = (uint)singleBuild.Primary.Id,
                    Secondary = (uint)singleBuild.Secondary.Id,
                    AttributesCount = (uint)Math.Min(singleBuild.Attributes.Count, 12),
                    AttributeIds = attributeIds,
                    AttributeValues = attributeValues,
                    Skills = skills
                };
                this.skillbarContextService.LoadBuild(playerAgentId, &skillTemplate);
                return true;
            }
        }, cancellationToken);

        if (result)
        {
            await this.chatService.AddMessageAsync("Build applied successfully.", "Daybreak.API", Channel.Moderator, cancellationToken);
        }
        else
        {
            await this.chatService.AddMessageAsync("Failed to apply build.", "Daybreak.API", Channel.Moderator, cancellationToken);
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

                var skillbarContext = gameContext.Pointer->WorldContext->Skillbars.AsValueEnumerable().FirstOrDefault(s => s.AgentId == playerAgentId);
                if (skillbarContext.AgentId != playerAgentId)
                {
                    scopedLogger.LogError("Failed to find skillbar context for player agent id {agentId}", playerAgentId);
                    return default;
                }

                var agentAttributes = gameContext.Pointer->WorldContext->Attributes.AsValueEnumerable().FirstOrDefault(a => a.AgentId == playerAgentId);
                if (agentAttributes.AgentId != playerAgentId)
                {
                    scopedLogger.LogError("Failed to find agent attributes for player agent id {agentId}", playerAgentId);
                    return default;
                }

                var professionContext = gameContext.Pointer->WorldContext->Professions.AsValueEnumerable().FirstOrDefault(p => p.AgentId == playerAgentId);
                if (professionContext.AgentId != playerAgentId)
                {
                    scopedLogger.LogError("Failed to find agent profession for player agent id {agentId}", playerAgentId);
                    return default;
                }

                return new BuildEntry(
                    (int)professionContext.CurrentPrimary,
                    (int)professionContext.CurrentSecondary,
                    agentAttributes.Attributes.GetAttributeEntryList(),
                    [skillbarContext.Skill0.Id, skillbarContext.Skill1.Id, skillbarContext.Skill2.Id, skillbarContext.Skill3.Id,
                    skillbarContext.Skill4.Id, skillbarContext.Skill5.Id, skillbarContext.Skill6.Id, skillbarContext.Skill7.Id]);
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
                if (gameContext.IsNull || gameContext.Pointer->WorldContext is null)
                {
                    scopedLogger.LogError("Game context is not initialized");
                    return default;
                }

                return new QuestLogInformation(
                    gameContext.Pointer->WorldContext->ActiveQuestId,
                    gameContext.Pointer->WorldContext->QuestLog.AsValueEnumerable().Select(q => new QuestInformation(q.QuestId, q.MapFrom)).ToList());
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
                if (gameContext.IsNull || gameContext.Pointer->CharContext is null ||
                    gameContext.Pointer->WorldContext is null)
                {
                    scopedLogger.LogError("Game context is not initialized");
                    return default;
                }

                var playerNameSpan = gameContext.Pointer->CharContext->PlayerName.AsSpan();
                var playerName = new string(playerNameSpan[..playerNameSpan.IndexOf('\0')]);
                var emailSpan = gameContext.Pointer->CharContext->PlayerEmail.AsSpan();
                var email = new string(emailSpan[..emailSpan.IndexOf('\0')]);
                var accountName = new string(gameContext.Pointer->WorldContext->AccountInfo->AccountName);
                return new MainPlayerInformation(
                    gameContext.Pointer->CharContext->PlayerUuid.ToString(),
                    email,
                    playerName,
                    accountName,
                    gameContext.Pointer->WorldContext->AccountInfo->Wins,
                    gameContext.Pointer->WorldContext->AccountInfo->Losses,
                    gameContext.Pointer->WorldContext->AccountInfo->Rating,
                    gameContext.Pointer->WorldContext->AccountInfo->QualifierPoints,
                    gameContext.Pointer->WorldContext->AccountInfo->Rank,
                    gameContext.Pointer->WorldContext->AccountInfo->TournamentRewardPoints);
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
                var agentArray = this.agentContextService.GetAgentArray();
                var playerAgentId = this.agentContextService.GetPlayerAgentId();
                if (gameContext.IsNull ||
                    gameContext.Pointer->WorldContext is null ||
                    gameContext.Pointer->CharContext is null ||
                    gameContext.Pointer->WorldContext->MapAgents.Buffer is null ||
                    agentArray.IsNull ||
                    agentArray.Pointer->Buffer is null ||
                    playerAgentId is 0x0)
                {
                    scopedLogger.LogDebug("Game data is not yet initialized");
                    return default;
                }

                var playerAgent = this.GetAgentContext(playerAgentId);
                if (playerAgent is null ||
                    playerAgent->Type is not AgentType.Living ||
                    playerAgent->AgentId != playerAgentId)
                {
                    scopedLogger.LogError("Player agent {playerAgentId} not found in agent array", playerAgentId);
                    return default;
                }

                var livingAgent = (AgentLivingContext*)playerAgent;
                if (livingAgent->Level != gameContext.Pointer->WorldContext->Level)
                {
                    scopedLogger.LogError("Player agent not found. Player level mismatch: {level} != {gameLevel}", livingAgent->Level, gameContext.Pointer->WorldContext->Level);
                    return default;
                }

                return new MainPlayerState(
                    gameContext.Pointer->WorldContext->Experience,
                    gameContext.Pointer->WorldContext->Level,

                    gameContext.Pointer->WorldContext->CurrentLuxon,
                    gameContext.Pointer->WorldContext->CurrentKurzick,
                    gameContext.Pointer->WorldContext->CurrentImperial,
                    gameContext.Pointer->WorldContext->CurrentBalthazar,
                    gameContext.Pointer->WorldContext->MaxLuxon,
                    gameContext.Pointer->WorldContext->MaxKurzick,
                    gameContext.Pointer->WorldContext->MaxImperial,
                    gameContext.Pointer->WorldContext->MaxBalthazar,
                    gameContext.Pointer->WorldContext->TotalLuxon,
                    gameContext.Pointer->WorldContext->TotalKurzick,
                    gameContext.Pointer->WorldContext->TotalImperial,
                    gameContext.Pointer->WorldContext->TotalBalthazar,

                    // Energy and health are percentages of Max
                    livingAgent->Health * livingAgent->MaxHealth,
                    livingAgent->MaxHealth,
                    livingAgent->Energy * livingAgent->MaxEnergy,
                    livingAgent->MaxEnergy,

                    livingAgent->Primary,
                    livingAgent->Secondary,

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
                    gameContext.Pointer->CharContext is null ||
                    gameContext.Pointer->WorldContext is null ||
                    gameContext.Pointer->PartyContext is null ||
                    currentMapInfo.IsNull)
                {
                    scopedLogger.LogError("Game context is not initialized");
                    return new InstanceInfo(0, 0, 0, 0, Shared.Models.Api.InstanceType.Loading, DistrictRegionInfo.Unknown, LanguageInfo.Unknown, CampaignInfo.Unknown, ContinentInfo.Unknown, RegionInfo.Unknown, DifficultyInfo.Unknown);
                }

                var charContext = *gameContext.Pointer->CharContext;
                var worldContext = *gameContext.Pointer->WorldContext;
                var partyContext = *gameContext.Pointer->PartyContext;
                var mapInfo = *currentMapInfo.Pointer;
                var mapId = charContext.MapId;
                var language = charContext.Language;
                var districtNumber = charContext.DistrictNumber;
                var foesKilled = worldContext.FoesKilled;
                var foesToKill = worldContext.FoesToKill;

                return new InstanceInfo(
                    MapId: charContext.MapId,
                    DistrictNumber: charContext.DistrictNumber,
                    FoesKilled: worldContext.FoesKilled,
                    FoesToKill: worldContext.FoesToKill,
                    Type: (Shared.Models.Api.InstanceType)instanceType,
                    DistrictRegion: (DistrictRegionInfo)serverRegion,
                    Language: (LanguageInfo)charContext.Language,
                    Campaign: (CampaignInfo)mapInfo.Campaign,
                    Continent: (ContinentInfo)mapInfo.Continent,
                    Region: (RegionInfo)mapInfo.Region,
                    Difficulty: partyContext.Flags.HasFlag(PartyFlags.HardMode) ? DifficultyInfo.Hard : DifficultyInfo.Normal);
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
                if (gameContext.IsNull || gameContext.Pointer->WorldContext is null)
                {
                    scopedLogger.LogError("Game context is not initialized");
                    return default;
                }

                var playerIndex = gameContext.Pointer->CharContext->PlayerNumber;
                var players = gameContext.Pointer->WorldContext->Players;
                if (playerIndex < 0 || playerIndex >= players.Size)
                {
                    scopedLogger.LogError("Failed to get player id. Got {playerNumber}", playerIndex);
                    return default;
                }

                var player = gameContext.Pointer->WorldContext->Players[(int)playerIndex];
                var currentTier = player.ActiveTitleTier;
                TitleContext? currentTitle = default;
                int? id = -1;
                for (var i = 0; i < gameContext.Pointer->WorldContext->Titles.Size; i++)
                {
                    var title = gameContext.Pointer->WorldContext->Titles.Skip(i).FirstOrDefault();
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

                var titleTier = gameContext.Pointer->WorldContext->TitleTiers.Skip((int)currentTitle.Value.CurrentTitleTierIndex).FirstOrDefault();
                return new TitleInfo
                (
                    Id: (uint)id,
                    IsPercentage: currentTitle.Value.Props.HasFlag(TitleProps.PercentageBased),
                    PointsForCurrentRank: currentTitle.Value.PointsNeededForCurrentRank,
                    PointsForNextRank: titleTier.TierNumber == currentTitle.Value.MaxTitleRank ?
                                        currentTitle.Value.CurrentPoints :
                                        currentTitle.Value.PointsNeededForNextRank,
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
                    gameContext.Pointer->AccountContext is null ||
                    gameContext.Pointer->WorldContext is null)
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

                var agentProfession = gameContext.Pointer->WorldContext->Professions.AsValueEnumerable().FirstOrDefault(p => p.AgentId == playerAgentId);
                if (agentProfession.AgentId != playerAgentId)
                {
                    scopedLogger.LogError("Failed to find agent profession for player agent id {agentId}", playerAgentId);
                    return default;
                }

                return new MainPlayerBuildContext(
                    PrimaryProfessionId: (uint)agentProfession.CurrentPrimary,
                    UnlockedProfessions: agentProfession.UnlockedProfessionsFlags,
                    UnlockedAccountSkills: gameContext.Pointer->AccountContext->UnlockedAccountSkills.AsValueEnumerable().ToArray(),
                    UnlockedCharacterSkills: gameContext.Pointer->WorldContext->UnlockedCharacterSkills.AsValueEnumerable().ToArray());

            }
        }, cancellationToken);
    }
}
