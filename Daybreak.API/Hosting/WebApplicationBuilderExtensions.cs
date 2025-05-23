namespace Daybreak.API.Hosting;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder WithHosting(this WebApplicationBuilder builder, int port)
    {
        builder.WebHost.ConfigureKestrel(o => o.ListenAnyIP(port));

        return builder;
    }
}
