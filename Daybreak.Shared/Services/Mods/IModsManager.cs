using System.Collections.Generic;

namespace Daybreak.Shared.Services.Mods;

public interface IModsManager
{
    void RegisterMod<TInterface, TImplementation>(bool singleton = false)
        where TInterface : class, IModService
        where TImplementation : TInterface;

    public IEnumerable<IModService> GetMods();
}
