namespace Daybreak.API.Hosting;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder WithHosting(this WebApplicationBuilder builder, int port)
    {
        builder.WebHost.UseUrls($"http://127.0.0.1:{port}");

        return builder;
    }
}
