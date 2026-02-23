using System.Diagnostics.CodeAnalysis;

namespace Daybreak.API.WebSockets;

public static class WebApplicationExtensions
{
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code")]
    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.")]
    public static WebApplication UseWebSocketRoutes(this WebApplication app)
    {
        return app;
    }
}
