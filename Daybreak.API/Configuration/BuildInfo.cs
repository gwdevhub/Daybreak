namespace Daybreak.API.Configuration;

public static class BuildInfo
{
    public const string Configuration =
#if DEBUG
            "Debug";
#else
            "Release";
#endif
}
