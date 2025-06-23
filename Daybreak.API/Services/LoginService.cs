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
                if (gameContext.IsNull || gameContext.Pointer->CharContext is null)
                {
                    scopedLogger.LogError("Game context is not initialized");
                    return default;
                }

                var playerNameSpan = gameContext.Pointer->CharContext->PlayerName.AsSpan();
                var playerName = new string(playerNameSpan[..playerNameSpan.IndexOf('\0')]);
                var emailSpan = gameContext.Pointer->CharContext->PlayerEmail.AsSpan();
                var email = new string(emailSpan[..emailSpan.IndexOf('\0')]);
                return new LoginInfo(email, playerName);
            }
        }, cancellationToken);
    }
}
