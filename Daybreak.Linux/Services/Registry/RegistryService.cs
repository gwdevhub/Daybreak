using Daybreak.Shared.Services.Registry;
using Daybreak.Shared.Utils;
using System.Text.Json;

namespace Daybreak.Linux.Services.Registry;

/// <summary>
/// Linux implementation of IRegistryService that uses a JSON file instead of Windows Registry.
/// Values are stored in a JSON file at the application root.
/// </summary>
internal sealed class RegistryService : IRegistryService
{
    private static readonly string RegistryFilePath = PathUtils.GetAbsolutePathFromRoot("Daybreak.registry.json");
    private readonly object fileLock = new();

    public bool SaveValue<T>(string key, T value)
    {
        if (value is null)
        {
            return false;
        }

        lock (this.fileLock)
        {
            var store = this.LoadStore();
            store[key] = JsonSerializer.Serialize(value);
            this.SaveStore(store);
            return true;
        }
    }

    public bool TryGetValue<T>(string key, out T? value)
    {
        lock (this.fileLock)
        {
            var store = this.LoadStore();
            if (store.TryGetValue(key, out var serialized))
            {
                value = JsonSerializer.Deserialize<T>(serialized);
                return value is not null;
            }

            value = default;
            return false;
        }
    }

    public bool DeleteValue(string key)
    {
        lock (this.fileLock)
        {
            var store = this.LoadStore();
            var removed = store.Remove(key);
            if (removed)
            {
                this.SaveStore(store);
            }

            return removed;
        }
    }

    private Dictionary<string, string> LoadStore()
    {
        if (!File.Exists(RegistryFilePath))
        {
            return [];
        }

        try
        {
            var json = File.ReadAllText(RegistryFilePath);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? [];
        }
        catch
        {
            return [];
        }
    }

    private void SaveStore(Dictionary<string, string> store)
    {
        var json = JsonSerializer.Serialize(store, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(RegistryFilePath, json);
    }
}
