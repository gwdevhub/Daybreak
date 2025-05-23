using Daybreak.API.Interop;
using Daybreak.API.Models;
using Daybreak.API.Services.Interop;
using System.Core.Extensions;
using System.Extensions.Core;

namespace Daybreak.API.Services;

public sealed class GameStateService : IDisposable
{
    private readonly CallbackRegistration callbackRegistration;
    private readonly GameContextService gameContextService;
    private readonly GameThreadService gameThreadService;
    private readonly ILogger<GameStateService> logger;

    private GameState? gameState;

    public GameStateService(
        GameContextService gameContextService,
        GameThreadService gameThreadService,
        ILogger<GameStateService> logger)
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

    public GameState? GetGameState()
    {
        return this.gameState;
    }

    private unsafe void OnGameThreadProc()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var gameContext = this.gameContextService.GetGameContext();
        if (gameContext is null ||
            gameContext->WorldContext is null ||
            gameContext->CharContext is null)
        {
            scopedLogger.LogError("Game context is not initialized");
            return;
        }

        this.gameState = new GameState
        {
            Email = new string(gameContext->CharContext->PlayerEmail.AsSpan()),
            CharacterName = new string(gameContext->CharContext->PlayerName.AsSpan()),
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
            CurrentMap = gameContext->MapContext is not null
                ? gameContext->CharContext is not null ? gameContext->CharContext->CurrentMapId : default
                : default
        };
    }
}
