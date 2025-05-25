using Daybreak.API.Models;
using Daybreak.API.Services;
using Microsoft.AspNetCore.Mvc;
using Net.Sdk.Web;
using System.Core.Extensions;

namespace Daybreak.API.Controllers.Rest;

[GenerateController("api/v1/rest/game-state")]
[Tags("GameState")]
public sealed class GameStateController(
    MainPlayerStateService gameStateService,
    ILogger<GameStateController> logger)
{
    private readonly MainPlayerStateService gameStateService = gameStateService.ThrowIfNull();
    private readonly ILogger<GameStateController> logger = logger.ThrowIfNull();

    [GenerateGet("main-player")]
    [EndpointName("GetMainPlayerState")]
    [EndpointSummary("Get the current MainPlayerState")]
    [EndpointDescription("Get the current MainPlayerState. Returns a json serialized MainPlayerState object. You can use the websocket endpoint to subscribe to MainPlayerState updates on each game thread proc")]
    [ProducesResponseType<MainPlayerState>(StatusCodes.Status200OK)]
    public async Task<IResult> GetMainPlayerState(CancellationToken cancellationToken)
    {
        var mainPlayerState = await this.gameStateService.GetMainPlayerState(cancellationToken);
        return Results.Ok(mainPlayerState);
    }
}
