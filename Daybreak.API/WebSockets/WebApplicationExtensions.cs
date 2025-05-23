using Daybreak.API.Controllers.WebSocket;
using Net.Sdk.Web.Websockets;
using Net.Sdk.Web.Websockets.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Extensions;

namespace Daybreak.API.WebSockets;

public static class WebApplicationExtensions
{
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code")]
    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.")]
    public static WebApplication UseWebSocketRoutes(this WebApplication app)
    {
        app.RegisterWebSocketRoute<GameStateRoute>(
            path: "/api/v1/ws/gamestate",
            name: "GameState",
            summary: "GameState WebSocket",
            description: "Subscribe to GameState websocket. On each game thread proc, the server will send a serialized GameState payload");

        return app;
    }

    private static WebApplication RegisterWebSocketRoute<T>(this WebApplication app, string path, string name, string summary, string description)
        where T : WebSocketRouteBase
    {
        app.UseWebSocketRoute<GameStateRoute>(path)
            .Cast<RouteHandlerBuilder>()
            .WithName(name)
            .WithSummary(summary)
            .WithDescription(description)
            .Produces(StatusCodes.Status101SwitchingProtocols)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("GameState");
        return app;
    }
}
