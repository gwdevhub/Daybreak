using System.IO;
using System.Reflection;

namespace Daybreak.Shared.Utils;
public static class PathUtils
{
    private static readonly Lazy<string> RootPath = new(() => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException("Unable to obtain application root path"));

    public static string GetRootFolder()
    {
        return RootPath.Value;
    }

    public static string GetAbsolutePathFromRoot(params string[] subPaths)
    {
        var paths = subPaths.Prepend(GetRootFolder()).ToArray();
        return Path.GetFullPath(Path.Combine(paths));
    }
}
