namespace Daybreak.API.Services;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder WithDaybreakServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<MemoryScanningService>();
        builder.Services.AddSingleton<GameThreadService>();
        builder.Services.AddHostedService(sp => sp.GetRequiredService<GameThreadService>());
        return builder;
    }
}
