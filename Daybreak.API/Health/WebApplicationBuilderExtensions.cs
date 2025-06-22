namespace Daybreak.API.Health;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder WithHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddCheck<HooksHealthCheck>(nameof(HooksHealthCheck))
            .AddCheck<AddressHealthCheck>(nameof(AddressHealthCheck));
        return builder;
    }
}
