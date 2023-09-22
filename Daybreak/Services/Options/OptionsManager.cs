using Daybreak.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.IO;
using System.Linq;

namespace Daybreak.Services.Options;

public sealed class OptionsManager : IOptionsManager, IOptionsProducer, IOptionsUpdateHook, IOptionsProvider
{
    private const string OptionsFile = "Daybreak.options";

    private readonly Dictionary<string, string> optionsCache = new();
    private readonly Dictionary<Type, List<Action>> optionsUpdateHooks = new();
    private readonly HashSet<Type> optionsTypes = new();

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

        this.optionsTypes.Add(typeof(T));
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

    public void UnregisterHook<TOptionsType>(Action action)
        where TOptionsType : class
    {
        if (this.optionsUpdateHooks.TryGetValue(typeof(TOptionsType), out var hooks))
        {
            hooks?.Remove(action);
        }
    }

    public void UpdateOptions<T>(T value)
        where T : class
    {
        this.SaveOptions(typeof(T), value);
    }

    public IEnumerable<object> GetRegisteredOptions()
    {
        foreach(var type in this.optionsTypes)
        {
            var optionsName = GetOptionsName(type);
            if (this.optionsCache.TryGetValue(optionsName, out var value) is false)
            {
                continue;
            }

            yield return JsonConvert.DeserializeObject(value, type) ?? Activator.CreateInstance(type)!;
        }
    }

    public IEnumerable<Type> GetRegisteredOptionsTypes()
    {
        return this.optionsTypes;
    }

    public void SaveRegisteredOptions(object options)
    {
        options.ThrowIfNull();

        this.SaveOptions(options.GetType(), options);
    }

    private void SaveOptions(Type type, object value)
    {
        var optionsName = GetOptionsName(type);
        if (this.optionsCache.ContainsKey(optionsName) is false)
        {
            throw new InvalidOperationException($"No registered or existing options found for {optionsName}");
        }

        this.optionsCache[optionsName] = JsonConvert.SerializeObject(value);
        this.SaveOptions();
        this.CallHooks(type);
    }

    private void SaveOptions()
    {
        File.WriteAllText(OptionsFile, JsonConvert.SerializeObject(this.optionsCache));
    }

    private void CallHooks(Type type)
    {
        if (this.optionsUpdateHooks.TryGetValue(type, out var hooks))
        {
            foreach (var hook in hooks)
            {
                hook();
            }
        }
    }

    private static string GetOptionsName<T>()
    {
        return GetOptionsName(typeof(T));
    }

    private static string GetOptionsName(Type type)
    {
        if (type.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(OptionsNameAttribute)) is not OptionsNameAttribute optionsNameAttribute)
        {
            return type.Name;
        }

        if (optionsNameAttribute.Name!.IsNullOrWhiteSpace())
        {
            return type.Name;
        }

        return optionsNameAttribute.Name!;
    }
}
