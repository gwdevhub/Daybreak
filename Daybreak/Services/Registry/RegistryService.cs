using Daybreak.Shared.Services.Registry;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Core.Extensions;
using System.Extensions;
using System.IO;

namespace Daybreak.Services.Registry;

internal sealed class RegistryService : IRegistryService
{
    private const string RegistryKey = "Daybreak";

    public bool DeleteValue(string key)
    {
        key.ThrowIfNull();
        return RegistryKeyExecute(key, (destinationRegistryKey, mappingKey) =>
        {
            if (destinationRegistryKey.OpenSubKey(mappingKey) is RegistryKey registryKey)
            {
                registryKey.Close();
                registryKey.Dispose();
                destinationRegistryKey.DeleteSubKey(mappingKey);
            }
            else
            {
                destinationRegistryKey.DeleteValue(mappingKey);
            }

            return true;
        });
    }

    public bool SaveValue<T>(string key, T value)
    {
        key.ThrowIfNull();
        if (value is null)
        {
            return false;
        }

        return RegistryKeyExecute(key, (destinationRegistryKey, mappingKey) =>
        {
            destinationRegistryKey.SetValue(mappingKey, JsonConvert.SerializeObject(value));
            return true;
        });
    }

    public bool TryGetValue<T>(string key, out T? value)
    {
        key.ThrowIfNull();

        var serializedValue = string.Empty;
        var operationResult = RegistryKeyExecute(key, (destinationRegistryKey, mappingKey) =>
        {
            var returnedValue = destinationRegistryKey.GetValue(mappingKey);
            if (returnedValue is not string returnedString)
            {
                return false;
            }

            serializedValue = returnedString;
            return true;
        });

        if (serializedValue.IsNullOrWhiteSpace())
        {
            value = default;
            return false;
        }

        value = JsonConvert.DeserializeObject<T>(serializedValue);
        return value is not null;
    }

    private static bool RegistryKeyExecute(string key, Func<RegistryKey, string, bool> execute)
    {
        var pathTokens = key.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        if (pathTokens.Length < 1)
        {
            return false;
        }

        var disposableKeys = new List<RegistryKey>();
        var homeRegistryKey = GetOrCreateHomeKey();
        var destinationRegistryKey = homeRegistryKey;
        disposableKeys.Add(destinationRegistryKey);
        if (pathTokens.Length > 1)
        {
            foreach (var token in pathTokens.Take(pathTokens.Length - 1))
            {
                var maybeSubKey = GetOrCreateSubKey(destinationRegistryKey, token);
                if (maybeSubKey is null)
                {
                    return false;
                }

                destinationRegistryKey = maybeSubKey;
                disposableKeys.Add(destinationRegistryKey);
            }
        }

        var finalKey = pathTokens.Last();
        var returnValue = execute(destinationRegistryKey, finalKey);
        foreach (var registryKey in disposableKeys)
        {
            try
            {
                registryKey.Close();
                registryKey.Dispose();
            }
            catch (ObjectDisposedException)
            {
            }
        }

        return returnValue;
    }

    private static RegistryKey GetOrCreateHomeKey()
    {
        var homeRegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software", true)?.OpenSubKey(RegistryKey, true);
        homeRegistryKey ??= Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software", true)!.CreateSubKey(RegistryKey, true);

        return homeRegistryKey;
    }

    private static RegistryKey GetOrCreateSubKey(RegistryKey registryKey, string subKey)
    {
        var maybeSubKey = registryKey?.OpenSubKey(subKey, true);
        maybeSubKey ??= registryKey?.CreateSubKey(subKey, true) ?? throw new InvalidOperationException($"Unable to create subkey [{subKey}]");

        return maybeSubKey;
    }
}
