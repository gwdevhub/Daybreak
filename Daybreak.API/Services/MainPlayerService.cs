using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using Daybreak.API.Services.Interop;
using Daybreak.Shared.Models.Api;
using MemoryPack;
using System.Buffers;
using System.Collections.Concurrent;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;
using System.Runtime.CompilerServices;
using ZLinq;

namespace Daybreak.API.Services;

public sealed class MainPlayerService : IDisposable
{
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
        GameContextService gameContextService,
        AgentContextService agentContextService,
        GameThreadService gameThreadService,
        ILogger<MainPlayerService> logger)
    {
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

    public async Task<QuestLogInformation?> GetQuestLog(CancellationToken cancellationToken)
    {
        return await this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                var gameContext = this.gameContextService.GetGameContext();
                if (gameContext is null || gameContext->WorldContext is null)
                {
                    this.logger.LogError("Game context or world context is not initialized");
                    return default;
                }

                return new QuestLogInformation
                {
                    Quests = gameContext->WorldContext->QuestLog.AsValueEnumerable().Select(q => new QuestInformation
                    {
                        QuestId = q.QuestId,
                        MapFrom = q.MapFrom,
                        MapTo = q.MapTo,
                    }).ToList(),
                    CurrentQuestId = gameContext->WorldContext->ActiveQuestId
                };
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

        var gameContext = this.gameContextService.GetGameContext();
        var agentArray = this.agentContextService.GetAgentArray();
        var playerAgentId = this.agentContextService.GetPlayerAgentId();
        if (gameContext is null ||
            gameContext->WorldContext is null ||
            gameContext->CharContext is null ||
            gameContext->WorldContext->MapAgents.Buffer is null ||
            agentArray is null ||
            agentArray->Buffer is null ||
            playerAgentId is 0x0)
        {
            scopedLogger.LogDebug("Game data is not yet initialized");
            return;
        }

        var playerMapAgent = gameContext->WorldContext->MapAgents.AsValueEnumerable().Skip((int)playerAgentId).FirstOrDefault();
        var playerAgent = this.GetAgentContext(playerAgentId);
        if (playerAgent is null ||
            playerAgent->Type is not AgentType.Living ||
            playerAgent->AgentId != playerAgentId)
        {
            scopedLogger.LogError("Player agent {playerAgentId} not found in agent array", playerAgentId);
            return;
        }

        var livingAgent = (AgentLivingContext*)playerAgent;
        if (livingAgent->Level != gameContext->WorldContext->Level)
        {
            scopedLogger.LogError("Player agent not found. Player level mismatch: {level} != {gameLevel}", livingAgent->Level, gameContext->WorldContext->Level);
            return;
        }

        this.mainPlayerState = new MainPlayerState
        {
            Level = gameContext->WorldContext->Level,
            CurrentExperience = gameContext->WorldContext->Experience,

            CurrentLuxon = gameContext->WorldContext->CurrentLuxon,
            CurrentKurzick = gameContext->WorldContext->CurrentKurzick,
            CurrentBalthazar = gameContext->WorldContext->CurrentBalthazar,
            CurrentImperial = gameContext->WorldContext->CurrentImperial,
            MaxBalthazar = gameContext->WorldContext->MaxBalthazar,
            MaxImperial = gameContext->WorldContext->MaxImperial,
            MaxKurzick = gameContext->WorldContext->MaxKurzick,
            MaxLuxon = gameContext->WorldContext->MaxLuxon,
            TotalLuxon = gameContext->WorldContext->TotalLuxon,
            TotalKurzick = gameContext->WorldContext->TotalKurzick,
            TotalBalthazar = gameContext->WorldContext->TotalBalthazar,
            TotalImperial = gameContext->WorldContext->TotalImperial,

            PosX = playerAgent->Pos.X,
            PosY = playerAgent->Pos.Y,

            PrimaryProfession = livingAgent->Primary,
            SecondaryProfession = livingAgent->Secondary,

            MaxEnergy = livingAgent->MaxEnergy,
            MaxHp = livingAgent->MaxHealth,

            // Energy and health are percentages of Max
            CurrentEnergy = livingAgent->Energy * livingAgent->MaxEnergy, 
            CurrentHp = livingAgent->Health * livingAgent->MaxHealth,
        };

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
        if (agentArray is null || agentArray->Buffer is null ||
            agentArray->Size <= agentId)
        {
            return null;
        }

        return agentArray->Buffer[agentId].Pointer;
    }
}
