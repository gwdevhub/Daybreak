using Daybreak.Shared.Services.Mods;

namespace Daybreak.Services.Mods;

internal sealed class ModsManager(
    IEnumerable<IModService> mods) : IModsManager
{
    private readonly IEnumerable<IModService> mods = mods;

    public IEnumerable<IModService> GetMods()
    {
        return this.mods;
    }
}
