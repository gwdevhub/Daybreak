namespace Daybreak.Utils;

public static class WebRootUtil
{
    public static string GetWebRootPath()
    {
        var releasePath = Path.Combine(AppContext.BaseDirectory, "wwwroot");
        if (Directory.Exists(releasePath))
        {
            return releasePath;
        }

        var debugPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "wwwroot");
        if (Directory.Exists(debugPath))
        {
            return debugPath;
        }

        throw new InvalidOperationException("Could not find web root folder");
    }
}
