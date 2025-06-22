namespace Daybreak.API.Logging;

public static class WebApplicationExtensions
{
    public static WebApplication UseLogging(this WebApplication app)
    {
        app.UseMiddleware<LoggingEnrichmentMiddleware>();
        return app;
    }
}
