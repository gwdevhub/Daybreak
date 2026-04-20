using Scalar.AspNetCore;

namespace Daybreak.API.Swagger;

public static class WebApplicationExtensions
{
    public static WebApplication UseSwaggerWithUI(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
        return app;
    }
}
