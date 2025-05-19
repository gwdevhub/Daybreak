using System.Core.Extensions;
using System.Extensions;
using Daybreak.API.Services;
using Daybreak.Shared.Models.Interop;
using Net.Sdk.Web;

namespace Daybreak.API.Controllers;

[GenerateController("api/test")]
public sealed class TestController(
    ILogger<TestController> logger,
    GameThreadService gameThreadService,
    MemoryScanningService memoryScanningService)
{
    private readonly ILogger<TestController> logger = logger.ThrowIfNull();
    private readonly GameThreadService gameThreadService = gameThreadService.ThrowIfNull();
    private readonly MemoryScanningService memoryScanningService = memoryScanningService.ThrowIfNull();

    [GenerateGet("1")]
    public IResult GetTest(CancellationToken token)
    {
        return Results.Text("Hello from injected ASP-NET Core!", "text/plain");
    }

    [GenerateGet("2")]
    public IResult GetTest2(CancellationToken token)
    {
        GameContext? gameContext = default;
        Task.Run(() => this.gameThreadService.QueueActionOnGameThread(() =>
        {
            var globalContext = this.memoryScanningService.GetGlobalContext();
            if (globalContext is not GlobalContext context)
            {
                return;
            }

            gameContext = this.memoryScanningService.ReadPointer(context.GameContext);
        }), token).Wait(token);
        
        if (gameContext is not GameContext game)
        {
            return Results.Ok();
        }

        return Results.Ok(new TempResponse
        {
            Level = game.Level,
        });
    }

    public class TempResponse
    {
        public uint Level { get; set; }
    }
}
