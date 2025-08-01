﻿using Daybreak.API.Controllers.WebSocket.MainPlayer;
using Net.Sdk.Web;

namespace Daybreak.API.WebSockets;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder WithWebSocketRoutes(this WebApplicationBuilder builder)
    {
        builder.WithWebSocketRoute<MainPlayerStateRoute>();
        return builder;
    }
}
