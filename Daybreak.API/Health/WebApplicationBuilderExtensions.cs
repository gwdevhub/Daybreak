namespace Daybreak.API.Health;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder WithHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks();
        return builder;
    }
}
