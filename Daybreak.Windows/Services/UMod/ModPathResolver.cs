using Daybreak.Shared.Services.UMod;

namespace Daybreak.Windows.Services.UMod;

/// <summary>
/// Windows implementation - paths are used as-is since mods run natively.
/// </summary>
public sealed class ModPathResolver : IModPathResolver
{
    public string ResolveForModLoader(string nativePath) => nativePath;
}
