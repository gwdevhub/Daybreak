using Daybreak.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Extensions;
using System.IO;
using System.Linq;

namespace Daybreak.Services.Options;

public sealed class OptionsManager : IOptionsManager, IOptionsProducer, IOptionsUpdateHook
{
    private const string OptionsFile = "Daybreak.options";

    private readonly Dictionary<string, string> optionsCache = new();
    private readonly Dictionary<Type, List<Action>> optionsUpdateHooks = new();

    public OptionsManager()
    {
        if (File.Exists(OptionsFile) is false)
        {
            File.WriteAllText(OptionsFile, string.Empty);
        }

        var optionsFileContent = File.ReadAllText(OptionsFile);
        if (optionsFileContent.IsNullOrWhiteSpace())
        {
            this.optionsCache = new Dictionary<string, string>();
        }
        else
        {
            this.optionsCache = JsonConvert.DeserializeObject<Dictionary<string, string>>(optionsFileContent) ??
            throw new InvalidOperationException("Unable to load options. Operation failed during deserialization");
        }
    }

    public T GetOptions<T>()
        where T : class
    {
        var optionsName = GetOptionsName<T>();
        if (this.optionsCache.TryGetValue(optionsName, out var value) is false)
        {
            throw new InvalidOperationException($"No registered or existing options found for {optionsName}");
        }

        return JsonConvert.DeserializeObject<T>(value) ??
            throw new InvalidOperationException($"Failed to deserialize options {optionsName}");
    }

    public void RegisterOptions<T>()
        where T : new()
    {
        var optionsName = GetOptionsName<T>();
        if (this.optionsCache.ContainsKey(optionsName) is false)
        {
            this.optionsCache.Add(optionsName, JsonConvert.SerializeObject(Activator.CreateInstance<T>()));
        }
    }

    public void RegisterHook<TOptionsType>(Action action)
        where TOptionsType : class
    {
        if (this.optionsUpdateHooks.TryGetValue(typeof(TOptionsType), out var hooks) is false ||
            hooks is null)
        {
            hooks = new List<Action>();
            this.optionsUpdateHooks[typeof(TOptionsType)] = hooks;
        }

        hooks.Add(action);
    }

    public void UpdateOptions<T>(T value)
        where T : class
    {
        var optionsName = GetOptionsName<T>();
        if (this.optionsCache.ContainsKey(optionsName) is false)
        {
            throw new InvalidOperationException($"No registered or existing options found for {optionsName}");
        }

        this.optionsCache[optionsName] = JsonConvert.SerializeObject(value);
        this.SaveOptions();
        this.CallHooks<T>();
    }

    private void SaveOptions()
    {
        File.WriteAllText(OptionsFile, JsonConvert.SerializeObject(this.optionsCache));
    }

    private void CallHooks<T>()
    {
        if (this.optionsUpdateHooks.TryGetValue(typeof(T), out var hooks))
        {
            foreach(var hook in hooks)
            {
                hook();
            }
        }
    }

    private static string GetOptionsName<T>()
    {
        var type = typeof(T);
        if (type.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(OptionsNameAttribute)) is not OptionsNameAttribute optionsNameAttribute)
        {
            return type.Name;
        }

        if (optionsNameAttribute.Name.IsNullOrWhiteSpace())
        {
            return type.Name;
        }

        return optionsNameAttribute.Name!;
    }
}
