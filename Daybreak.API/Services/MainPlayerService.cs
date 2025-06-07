﻿using Daybreak.API.Extensions;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using Daybreak.API.Services.Interop;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Services.BuildTemplates;
using MemoryPack;
using System.Buffers;
using System.Collections.Concurrent;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;
using System.Runtime.CompilerServices;
using ZLinq;
using InstanceType = Daybreak.API.Interop.GuildWars.InstanceType;

namespace Daybreak.API.Services;

public sealed class MainPlayerService : IDisposable
{
    private readonly InstanceContextService instanceContextService;
    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly SkillbarContextService skillbarContextService;
    private readonly AgentContextService agentContextService;
    private readonly CallbackRegistration callbackRegistration;
    private readonly GameContextService gameContextService;
    private readonly GameThreadService gameThreadService;
    private readonly ILogger<MainPlayerService> logger;

    private readonly ConcurrentQueue<TaskCompletionSource<MainPlayerState>> pendingRequests = new();
    private readonly ConcurrentDictionary<Guid, ByteConsumerEntry> consumers = new();
    private readonly ArrayBufferWriter<byte> bufferWriter = new();

    private TimeSpan minUpdateFrequency = TimeSpan.MaxValue;
    private MainPlayerState? mainPlayerState;
    private DateTimeOffset lastUpdateTime = DateTimeOffset.MinValue;
    private DateTimeOffset lastFrequencyUpdate = DateTimeOffset.MinValue;

    public MainPlayerService(
        InstanceContextService instanceContextService,
        IBuildTemplateManager buildTemplateManager,
        SkillbarContextService skillbarContextService,
        GameContextService gameContextService,
        AgentContextService agentContextService,
        GameThreadService gameThreadService,
        ILogger<MainPlayerService> logger)
    {
        this.instanceContextService = instanceContextService.ThrowIfNull();
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.skillbarContextService = skillbarContextService.ThrowIfNull();
        this.gameThreadService = gameThreadService.ThrowIfNull();
        this.agentContextService = agentContextService.ThrowIfNull();
        this.gameContextService = gameContextService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.callbackRegistration = this.gameThreadService.RegisterCallback(this.OnGameThreadProc);
    }

    public void Dispose()
    {
        this.callbackRegistration?.Dispose();
    }

    public Task<bool> SetCurrentBuild(string buildCode, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (!this.buildTemplateManager.TryDecodeTemplate(buildCode, out var build) ||
            build is not SingleBuildEntry singleBuild)
        {
            scopedLogger.LogError("Failed to decode build template from code {buildCode}", buildCode);
            return Task.FromResult(false);
        }

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
                if (gameContext.IsNull ||
                    gameContext.Pointer->WorldContext is null)
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

                var playerAgent = this.GetAgentContext(playerAgentId);
                if (playerAgent is null ||
                    playerAgent->Type is not AgentType.Living ||
                    playerAgent->AgentId != playerAgentId)
                {
                    scopedLogger.LogError("Player agent {playerAgentId} not found in agent array", playerAgentId);
                    return false;
                }

                var livingAgent = (AgentLivingContext*)playerAgent;
                if (livingAgent->Level != gameContext.Pointer->WorldContext->Level)
                {
                    scopedLogger.LogError("Player agent not found. Player level mismatch: {level} != {gameLevel}", livingAgent->Level, gameContext.Pointer->WorldContext->Level);
                    return false;
                }

                var agentProfession = gameContext.Pointer->WorldContext->Professions.AsValueEnumerable().FirstOrDefault(p => p.AgentId == playerAgentId);
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
                    [.. gameContext.Pointer->WorldContext->UnlockedCharacterSkills]);

                if (!this.buildTemplateManager.CanTemplateApply(validationRequest))
                {
                    scopedLogger.LogError("Build template validation failed for player agent id {agentId}", playerAgentId);
                    return false;
                }

                var attributeIds = new Array12Uint();
                var attributeValues = new Array12Uint();
                var skills = new Array8Uint();
                for(var i = 0; i < singleBuild.Attributes.Count && i < 12; i++)
                {
                    if (singleBuild.Attributes[i].Attribute is null)
                    {
                        continue;
                    }

                    attributeIds[i] = (uint)singleBuild.Attributes[i].Attribute!.Id;
                    attributeValues[i] = (uint)singleBuild.Attributes[i].Points;
                }

                for(var i =0; i < singleBuild.Skills.Count && i < 8; i++)
                {
                    skills[i] = (uint)singleBuild.Skills[i].Id;
                }

                var skillTemplate = new SkillTemplate(
                    (uint)singleBuild.Primary.Id,
                    (uint)singleBuild.Secondary.Id,
                    (uint)Math.Min(singleBuild.Attributes.Count, 12),
                    attributeIds,
                    attributeValues,
                    skills);

                this.skillbarContextService.LoadBuild(playerAgentId, &skillTemplate);
                return true;
            }
        }, cancellationToken);
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

                return new BuildEntry(
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
                    gameContext.Pointer->WorldContext->QuestLog.AsValueEnumerable().Select(q => new QuestInformation(q.QuestId, q.MapFrom, q.MapTo)).ToList());
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

    public Task<MainPlayerState> GetMainPlayerState(CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<MainPlayerState>();
        if (cancellationToken.CanBeCanceled)
        {
            cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken));
        }

        this.pendingRequests.Enqueue(tcs);
        return tcs.Task;
    }

    public CallbackRegistration RegisterMainStateConsumer(TimeSpan frequency, Action<ReadOnlySpan<byte>> onUpdate)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(frequency, TimeSpan.Zero);

        var id = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;
        var entry = new ByteConsumerEntry(id, frequency, onUpdate);

        this.consumers[id] = entry;
        this.RecalculateMinFrequency();

        return new CallbackRegistration(id, () =>
        {
            this.consumers.TryRemove(id, out _);
            this.RecalculateMinFrequency();
        });
    }

    private unsafe void OnGameThreadProc()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var now = DateTimeOffset.UtcNow;

        /*
         * Periodically adjust the frequency of updates based on connected clients.
         * Sometimes clients crash and disconnect in a way that the server wasn't able
         * to trigger a frequency recalculation, so this periodic recalculation should
         * take care of inconsistent state.
         */
        if (now - this.lastFrequencyUpdate > TimeSpan.FromSeconds(10))
        {
            this.RecalculateMinFrequency();
        }

        /*
         * Only rebuild the state when we do not have one yet or the shortest required frequency has elapsed.
         * If we have any pending request (excluding consumers), we should also rebuild the state
         * to return the latest data to the request
         */
        if (this.mainPlayerState is not null &&
            this.minUpdateFrequency is TimeSpan minFreq &&
            now - this.lastUpdateTime < minFreq &&
            this.pendingRequests.IsEmpty)
        {
            return;
        }

        if (this.instanceContextService.GetInstanceType() is InstanceType.Loading)
        {
            scopedLogger.LogError("Not loaded");
            return;
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
            return;
        }

        var playerMapAgent = gameContext.Pointer->WorldContext->MapAgents.AsValueEnumerable().Skip((int)playerAgentId).FirstOrDefault();
        var playerAgent = this.GetAgentContext(playerAgentId);
        if (playerAgent is null ||
            playerAgent->Type is not AgentType.Living ||
            playerAgent->AgentId != playerAgentId)
        {
            scopedLogger.LogError("Player agent {playerAgentId} not found in agent array", playerAgentId);
            return;
        }

        var livingAgent = (AgentLivingContext*)playerAgent;
        if (livingAgent->Level != gameContext.Pointer->WorldContext->Level)
        {
            scopedLogger.LogError("Player agent not found. Player level mismatch: {level} != {gameLevel}", livingAgent->Level, gameContext.Pointer->WorldContext->Level);
            return;
        }

        this.mainPlayerState = new MainPlayerState(
            gameContext.Pointer->WorldContext->Level,
            gameContext.Pointer->WorldContext->Experience,

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

        this.bufferWriter.ResetWrittenCount();
        MemoryPackSerializer.Serialize(this.bufferWriter, this.mainPlayerState);
        this.lastUpdateTime = now;

        while (this.pendingRequests.TryDequeue(out var tcs))
        {
            tcs.TrySetResult(this.mainPlayerState);
        }

        foreach (var kvp in this.consumers)
        {
            var entry = kvp.Value;
            entry.TryConsume(now, this.bufferWriter.WrittenSpan);
        }
    }

    private void RecalculateMinFrequency()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var noConsumers = this.consumers.IsEmpty;
        if (noConsumers)
        {
            this.minUpdateFrequency = TimeSpan.MaxValue;
            scopedLogger.LogDebug("No consumers registered, disabling updates");
            this.lastFrequencyUpdate = DateTimeOffset.UtcNow;
            return;
        }

        var minValue = this.consumers.Values.Min(c => c.Frequency);
        scopedLogger.LogDebug("Adjusted update frequency to {frequency}", minValue);
        this.lastFrequencyUpdate = DateTimeOffset.UtcNow;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe AgentContext* GetAgentContext(uint agentId)
    {
        var agentArray = this.agentContextService.GetAgentArray();
        if (agentArray.IsNull || agentArray.Pointer->Buffer is null ||
            agentArray.Pointer->Size <= agentId)
        {
            return null;
        }

        return agentArray.Pointer->Buffer[agentId].Pointer;
    }
}
