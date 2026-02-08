using Daybreak.Shared.Services.UMod;
using Daybreak.Shared.Utils;

namespace Daybreak.Linux.Services.UMod;

/// <summary>
/// Linux implementation - converts native Linux paths to Wine-compatible paths (Z:\...)
/// since gMod.dll reads the modlist from inside the Wine process.
/// </summary>
public sealed class ModPathResolver : IModPathResolver
{
    public string ResolveForModLoader(string nativePath) => PathUtils.ToWinePath(nativePath);
}
