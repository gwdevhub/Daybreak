using Daybreak.API.Services.Interop;
using Daybreak.Shared.Models.Api;
using System.Core.Extensions;
using System.Extensions.Core;

namespace Daybreak.API.Services;

public sealed class LoginService(
    GameThreadService gameThreadService,
    GameContextService gameContextService,
    ILogger<LoginService> logger)
{
    private readonly GameThreadService gameThreadService = gameThreadService.ThrowIfNull();
    private readonly GameContextService gameContextService = gameContextService.ThrowIfNull();
    private readonly ILogger<LoginService> logger = logger.ThrowIfNull();

    public Task<LoginInfo?> GetLoginInformation(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        return this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                var gameContext = this.gameContextService.GetGameContext();
                if (gameContext.IsNull || gameContext.Pointer->Character is null)
                {
                    scopedLogger.LogError("Game context is not initialized");
                    return default;
                }

                var playerName = new string(gameContext.Pointer->Character->PlayerName);
                var email = new string(gameContext.Pointer->Character->PlayerEmail);
                return new LoginInfo(email, playerName);
            }
        }, cancellationToken);
    }
}
