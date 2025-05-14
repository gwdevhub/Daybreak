namespace Daybreak.Shared.Services.Registry;

public interface IRegistryService
{
    bool SaveValue<T>(string key, T value);
    bool TryGetValue<T>(string key, out T? value);
    bool DeleteValue(string key);
}
