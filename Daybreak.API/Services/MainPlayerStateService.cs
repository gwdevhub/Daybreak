using Daybreak.API.Interop;
using Daybreak.API.Models;
using Daybreak.API.Services.Interop;
using MemoryPack;
using System.Buffers;
using System.Collections.Concurrent;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;
using System.Runtime.InteropServices;

namespace Daybreak.API.Services;

public sealed class MainPlayerStateService : IDisposable
{
    private readonly CallbackRegistration callbackRegistration;
    private readonly GameContextService gameContextService;
    private readonly GameThreadService gameThreadService;
    private readonly ILogger<MainPlayerStateService> logger;

    private readonly ConcurrentQueue<TaskCompletionSource<MainPlayerState>> pendingRequests = new();
    private readonly ConcurrentDictionary<Guid, ByteConsumerEntry> consumers = new();
    private readonly ArrayBufferWriter<byte> bufferWriter = new();

    private TimeSpan minUpdateFrequency = TimeSpan.MaxValue;
    private MainPlayerState? mainPlayerState;
    private DateTimeOffset lastUpdateTime = DateTimeOffset.MinValue;

    public MainPlayerStateService(
        GameContextService gameContextService,
        GameThreadService gameThreadService,
        ILogger<MainPlayerStateService> logger)
    {
        this.gameThreadService = gameThreadService.ThrowIfNull();
        this.gameContextService = gameContextService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.callbackRegistration = this.gameThreadService.RegisterCallback(this.OnGameThreadProc);
    }

    public void Dispose()
    {
        this.callbackRegistration?.Dispose();
    }

    public Task<MainPlayerState> GetMainPlayerState(CancellationToken cancellationToken)
    {
        if (this.mainPlayerState is MainPlayerState state)
        {
            return Task.FromResult(state);
        }

        var tcs = new TaskCompletionSource<MainPlayerState>();
        if (cancellationToken.CanBeCanceled)
        {
            cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken));
        }

        this.pendingRequests.Enqueue(tcs);
        return tcs.Task;
    }

    public CallbackRegistration RegisterConsumer(TimeSpan frequency, Action<ReadOnlySpan<byte>> onUpdate)
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

        // Only rebuild the state when we do not have one yet or the shortest required frequency has elapsed.
        if (this.mainPlayerState is not null &&
            this.minUpdateFrequency is TimeSpan minFreq &&
            now - this.lastUpdateTime < minFreq)
        {
            return;
        }

        var gameContext = this.gameContextService.GetGameContext();
        if (gameContext is null ||
            gameContext->WorldContext is null ||
            gameContext->CharContext is null)
        {
            scopedLogger.LogError("Game context is not initialized");
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
            TotalImperial = gameContext->WorldContext->TotalImperial
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
        this.minUpdateFrequency = this.consumers.IsEmpty
            ? TimeSpan.MaxValue
            : this.consumers.Values.Min(c => c.Frequency);
    }
}
