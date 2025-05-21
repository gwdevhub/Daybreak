using System.Core.Extensions;
using System.Extensions;
using Daybreak.API.Services;
using Net.Sdk.Web;

namespace Daybreak.API.Controllers;

[GenerateController("api/test")]
public sealed class TestController(
    ILogger<TestController> logger,
    GameStateService gameStateService)
{
    private readonly ILogger<TestController> logger = logger.ThrowIfNull();
    private readonly GameStateService gameStateService = gameStateService.ThrowIfNull();

    [GenerateGet("1")]
    public IResult GetTest(CancellationToken _)
    {
        return Results.Text("Hello from injected ASP-NET Core!", "text/plain");
    }

    [GenerateGet("game")]
    public IResult GetTest2(CancellationToken _)
    {
        var gameState = this.gameStateService.GetGameState();
        return Results.Ok(gameState);
    }
}
