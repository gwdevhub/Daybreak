using Daybreak.Shared.Services.Mods;

namespace Daybreak.Shared.Services.Initialization;

public interface IModsProducer
{
    void RegisterMod<TInterface, TImplementation>(bool singleton = false)
        where TInterface : class, IModService
        where TImplementation : class, TInterface;
}
