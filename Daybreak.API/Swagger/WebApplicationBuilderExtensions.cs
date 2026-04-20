using Microsoft.AspNetCore.Routing.Constraints;

namespace Daybreak.API.Swagger;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder WithSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.Configure<RouteOptions>(static options =>
        {
            options.SetParameterPolicy<RegexInlineRouteConstraint>("regex");
        });
        builder.Services.AddOpenApi();

        return builder;
    }
}
