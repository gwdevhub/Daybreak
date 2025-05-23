using Daybreak.API.Models;
using Daybreak.API.Services;
using Microsoft.AspNetCore.Mvc;
using Net.Sdk.Web;
using System.Core.Extensions;

namespace Daybreak.API.Controllers.Rest;

[GenerateController("api/v1/rest/gamestate")]
[Tags("GameState")]
public sealed class GameStateController(
    GameStateService gameStateService,
    ILogger<GameStateController> logger)
{
    private readonly GameStateService gameStateService = gameStateService.ThrowIfNull();
    private readonly ILogger<GameStateController> logger = logger.ThrowIfNull();

    [GenerateGet]
    [EndpointName("GetGameState")]
    [EndpointSummary("Get the current game state")]
    [EndpointDescription("Get the current game state. Returns a json serialized GameState object. You can use the websocket endpoint to subscribe to GameState updates on each game thread proc")]
    [ProducesResponseType<GameState>(StatusCodes.Status200OK)]
    public Task<IResult> GetGameState(CancellationToken _)
    {
        return Task.FromResult(Results.Ok(this.gameStateService.GetGameState()));
    }
}
