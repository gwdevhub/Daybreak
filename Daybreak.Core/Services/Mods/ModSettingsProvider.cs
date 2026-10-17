using Daybreak.Shared.Attributes;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.LaunchConfigurations;
using Daybreak.Shared.Services.Mods;
using Daybreak.Shared.Services.Options;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Daybreak.Services.Mods;

internal sealed class ModSettingsProvider(
    IOptionsProvider optionsProvider,
    ILaunchConfigurationService launchConfigurationService) : IModSettingsProvider
{
    private static readonly ConcurrentDictionary<Type, string[]> RuntimeProperties = new();

    public T Get<T>(string modName, T globalSettings, LaunchConfigurationWithCredentials? configuration = null)
        where T : class
    {
        var settings = JsonSerializer.SerializeToNode(globalSettings)!.AsObject();
        if (configuration?.CustomModLoadoutEnabled is true &&
            configuration.ModLoadout.TryGetValue(modName, out var entry))
        {
            foreach (var name in GetRuntimeProperties<T>())
            {
                if (entry.Settings.TryGetValue(name, out var value))
                {
                    settings[name] = JsonNode.Parse(value.GetRawText());
                }
            }
        }

        return settings.Deserialize<T>() ?? throw new JsonException($"Invalid settings for {modName}");
    }

    public void Save<T>(string modName, T settings, LaunchConfigurationWithCredentials? configuration = null)
        where T : class
    {
        if (configuration?.CustomModLoadoutEnabled is not true)
        {
            optionsProvider.SaveOption(settings);
            return;
        }

        var serialized = JsonSerializer.SerializeToElement(settings);
        var overrides = GetRuntimeProperties<T>()
            .ToDictionary(name => name, name => serialized.GetProperty(name).Clone());
        var hadEntry = configuration.ModLoadout.TryGetValue(modName, out var previous);
        configuration.ModLoadout[modName] = new ModLoadoutEntry
        {
            IsEnabled = previous?.IsEnabled ?? false,
            Settings = overrides,
        };
        if (!launchConfigurationService.SaveConfiguration(configuration))
        {
            if (hadEntry)
            {
                configuration.ModLoadout[modName] = previous!;
            }
            else
            {
                configuration.ModLoadout.Remove(modName);
            }

            throw new InvalidOperationException($"Failed to save settings for {modName} in launch configuration {configuration.Identifier}");
        }
    }

    private static IEnumerable<string> GetRuntimeProperties<T>() =>
        RuntimeProperties.GetOrAdd(typeof(T), static type => type.GetProperties()
            .Where(property => property.IsDefined(typeof(ModSettingAttribute)))
            .Select(property => property.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? property.Name)
            .ToArray());
}
