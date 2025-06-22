using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace Daybreak.API.Swagger;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder WithSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddTransient<ISerializerDataContractResolver>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<JsonOptions>>().Value?.SerializerOptions
                       ?? new JsonSerializerOptions(JsonSerializerDefaults.Web);

            return new JsonSerializerDataContractResolver(opts);
        });
        builder.Services.Configure<RouteOptions>(static options =>
        {
            options.SetParameterPolicy<RegexInlineRouteConstraint>("regex");
        });
        builder.Services.AddOpenApi();

        return builder;
    }
}
