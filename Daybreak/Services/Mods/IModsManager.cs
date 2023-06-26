using System.Collections.Generic;

namespace Daybreak.Services.Mods;

public interface IModsManager
{
    void RegisterMod<TInterface, TImplementation>()
        where TInterface : class, IModService
        where TImplementation : TInterface;

    public IEnumerable<IModService> GetMods();
}
