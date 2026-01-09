namespace Daybreak.Shared.Services.Mods;

public interface IModsManager
{
    public IEnumerable<IModService> GetMods();
}
