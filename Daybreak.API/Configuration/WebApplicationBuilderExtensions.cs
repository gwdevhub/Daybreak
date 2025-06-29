namespace Daybreak.API.Configuration;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder WithConfiguration(this WebApplicationBuilder builder)
    {
        var config = BuildInfo.Configuration;
        var appSettingsStream = GetManifestResourceStream($"Daybreak.API.Configuration.appsettings.json") ?? throw new InvalidOperationException("Failed to load appsettings.json");
        var appSettingsConfigStream = GetManifestResourceStream($"Daybreak.API.Configuration.appsettings.{config}.json") ?? throw new InvalidOperationException($"Failed to load appsettings.{config}.json");
        builder.Configuration
            .AddJsonStream(appSettingsStream)
            .AddJsonStream(appSettingsConfigStream)
            .AddEnvironmentVariables();

        builder.WebHost.UseSetting(WebHostDefaults.PreventHostingStartupKey, "true");
        return builder;
    }

    private static Stream? GetManifestResourceStream(string name)
    {
        var assembly = typeof(WebApplicationBuilderExtensions).Assembly;
        var resourceName = assembly.GetManifestResourceNames()
            .FirstOrDefault(n => n.EndsWith(name, StringComparison.OrdinalIgnoreCase));
        if (resourceName is null)
        {
            return default;
        }

        return assembly.GetManifestResourceStream(resourceName);
    }
}
