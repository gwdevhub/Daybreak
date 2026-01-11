using Daybreak.Models;
using Daybreak.Shared.Attributes;
using Daybreak.Shared.Models.Options;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Utils;
using Daybreak.Shared.Validators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Core.Extensions;
using System.Extensions;
using System.Reflection;

namespace Daybreak.Services.Options;

internal sealed class OptionsManager : IOptionsProvider
{
    private const string OptionsFileSubPath = "Daybreak.options";

    private static readonly string OptionsFile = PathUtils.GetAbsolutePathFromRoot(OptionsFileSubPath);

    private readonly Dictionary<string, string> optionsCache = [];
    private readonly Dictionary<Type, List<Action>> optionsUpdateHooks = [];
    private readonly HashSet<Type> optionsTypes = [];
    private readonly List<OptionType> optionDefinitions = [];

    public OptionsManager(
        IEnumerable<OptionEntry> registeredOptions)
    {
        if (File.Exists(OptionsFile) is false)
        {
            File.WriteAllText(OptionsFile, string.Empty);
        }

        var optionsFileContent = File.ReadAllText(OptionsFile);
        if (optionsFileContent.IsNullOrWhiteSpace())
        {
            this.optionsCache = [];
        }
        else
        {
            this.optionsCache = JsonConvert.DeserializeObject<Dictionary<string, string>>(optionsFileContent) ??
            throw new InvalidOperationException("Unable to load options. Operation failed during deserialization");
        }

        foreach(var option in registeredOptions)
        {
            this.RegisterOptions(option.OptionType);
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

    public void RegisterHook<TOptionsType>(Action action)
        where TOptionsType : class
    {
        if (this.optionsUpdateHooks.TryGetValue(typeof(TOptionsType), out var hooks) is false ||
            hooks is null)
        {
            hooks = [];
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

    public IEnumerable<OptionInstance> GetRegisteredOptionInstances()
    {
        foreach (var type in this.optionDefinitions)
        {
            var optionsName = GetOptionsName(type.Type);
            if (this.optionsCache.TryGetValue(optionsName, out var value) is false)
            {
                continue;
            }

            var instance = JsonConvert.DeserializeObject(value, type.Type) ?? Activator.CreateInstance(type.Type);
            yield return new OptionInstance { Reference = instance!, Type = type };
        }
    }

    public IEnumerable<OptionType> GetRegisteredOptionDefinitions()
    {
        return this.optionDefinitions.Where(o => o.IsVisible);
    }

    public OptionInstance GetRegisteredOptionInstance(string optionName)
    {
        if (this.optionDefinitions.FirstOrDefault(t => t.Name == optionName) is not OptionType type)
        {
            throw new InvalidOperationException($"No registered options found for {optionName}");
        }

        if (this.optionsCache.TryGetValue(optionName, out var value) is false)
        {
            throw new InvalidOperationException($"No existing options found for {optionName}");
        }

        var instance = JsonConvert.DeserializeObject(value, type.Type) ?? Activator.CreateInstance(type.Type);
        return new OptionInstance { Reference = instance!, Type = type };
    }

    public void SaveOption<TOptions>(TOptions options)
        where TOptions : notnull
    {
        this.SaveRegisteredOptions(options);
    }

    public void SaveRegisteredOptions(object options)
    {
        options.ThrowIfNull();

        this.SaveOptions(options.GetType(), options);
    }

    public void SaveRegisteredOptions(OptionInstance optionInstance)
    {
        this.SaveOptions(optionInstance.Type.Type, optionInstance.Reference);
    }

    public void SaveRegisteredOptions(string name, JObject options)
    {
        options.ThrowIfNull();

        var registeredType = this.optionsTypes.FirstOrDefault(t =>
        {
            var typeName = t.Name;
            if (t.GetCustomAttribute<OptionsNameAttribute>() is OptionsNameAttribute optionsName)
            {
                typeName = optionsName.Name;
            }

            return name == typeName;
        });

        if (registeredType is null)
        {
            return;
        }

        this.SaveOptions(registeredType, options);
    }

    public JObject? TryGetKeyedOptions(string key)
    {
        if (!this.optionsCache.TryGetValue(key, out var value))
        {
            return default;
        }

        return JsonConvert.DeserializeObject<JObject>(value);
    }

    private void RegisterOptions(Type optionType)
    {
        var optionsName = GetOptionsName(optionType);
        if (this.optionsCache.ContainsKey(optionsName) is false)
        {
            this.optionsCache.Add(optionsName, JsonConvert.SerializeObject(Activator.CreateInstance(optionType)));
        }

        this.optionsTypes.Add(optionType);
        this.optionDefinitions.Add(new OptionType
        {
            Name = optionsName,
            Description = GetOptionsToolTip(optionType),
            Type = optionType,
            IsVisible = IsOptionsVisible(optionType),
            IsSynchronized = IsSynchronizedOption(optionType),
            Properties = [.. GetOptionProperties(optionType, optionsName)]
        });
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

    private static IEnumerable<OptionProperty> GetOptionProperties(Type type, string optionsName)
    {
        foreach (var propertyInfo in type.GetProperties())
        {
            var propertyType = propertyInfo.PropertyType;
            (var name, var description) = GetNameAndDescription(propertyInfo, optionsName);
            var validator = GetValidator(propertyInfo);
            var possibleValues = GetValuesFactory(propertyInfo);
            var converter = TypeDescriptor.GetConverter(propertyType);
            var getter = new Func<OptionInstance, object?>(instance => propertyInfo.GetValue(instance.Reference));
            var setter = new Action<OptionInstance, object?>((instance, newValue) =>
            {
                if (newValue is null && !propertyType.IsValueType)
                {
                    propertyInfo.SetValue(instance.Reference, newValue);
                    return;
                }

                if (newValue is not null && propertyType.IsAssignableFrom(newValue.GetType()))
                {
                    propertyInfo.SetValue(instance.Reference, newValue);
                    return;
                }

                if (newValue is not null)
                {
                    propertyInfo.SetValue(instance.Reference, converter.ConvertFrom(newValue));
                    return;
                }

                throw new InvalidOperationException("Unable to set null to value type property");
            });

            var optionProperty = new OptionProperty
            {
                Name = name,
                Description = description,
                ValuesFactory = possibleValues,
                Validator = validator ?? new AllGoesValidator(),
                Setter = setter,
                Getter = getter,
                Converter = converter,
                Type = propertyType,
                IsSynchronized = IsSynchronizedProperty(propertyInfo),
                IsVisible = IsPropertyVisible(propertyInfo)
            };

            yield return optionProperty;
        }
    }

    private static Func<List<object>>? GetValuesFactory(PropertyInfo propertyInfo)
    {
        if (propertyInfo.GetCustomAttribute<OptionValuesAttribute>() is OptionValuesAttribute optionValuesAttribute)
        {
            return () => [.. optionValuesAttribute.Values];
        }

        if (propertyInfo.GetCustomAttribute<OptionValuesFactoryAttribute>() is OptionValuesFactoryAttribute optionValuesFactoryAttribute)
        {
            return optionValuesFactoryAttribute.ValuesFactory;
        }

        if (propertyInfo.PropertyType.IsEnum)
        {
            var values = Enum.GetValues(propertyInfo.PropertyType).Cast<object>().ToList();
            return () => values;
        }

        return default;
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

    private static string GetOptionsToolTip(Type type)
    {
        var name = GetOptionsName(type);
        return $"{name} settings";
    }

    private static bool IsOptionsVisible(Type optionType)
    {
        if (optionType.GetCustomAttribute<OptionsIgnoreAttribute>() is not null)
        {
            return false;
        }

        return true;
    }

    private static bool IsSynchronizedOption(Type optionType)
    {
        if (optionType.GetCustomAttribute<OptionsSynchronizationIgnoreAttribute>() is not null)
        {
            return false;
        }

        return true;
    }

    private static bool IsSynchronizedProperty(PropertyInfo propertyInfo)
    {
        if (propertyInfo.GetCustomAttribute<OptionSynchronizationIgnoreAttribute>() is not null)
        {
            return false;
        }

        return true;
    }

    private static bool IsPropertyVisible(PropertyInfo propertyInfo)
    {
        return propertyInfo.GetCustomAttribute<OptionIgnoreAttribute>() is null;
    }

    private static IValidator? GetValidator(PropertyInfo propertyInfo)
    {
        // If property has a custom validator, return that validator
        if (propertyInfo.GetCustomAttributes()
                .FirstOrDefault(a =>
                {
                    var attributeType = a.GetType();
                    if (!attributeType.IsGenericType)
                    {
                        return false;
                    }

                    return a.GetType().GetGenericTypeDefinition() == typeof(OptionCustomValidatorAttribute<>);
                }) is object customValidatorAttribute)
        {
            var validatorPropertyInfo = customValidatorAttribute.GetType().GetProperty(nameof(OptionCustomValidatorAttribute<>.Validator));
            return validatorPropertyInfo?.GetValue(customValidatorAttribute) as IValidator;
        }

        if (propertyInfo.PropertyType.IsPrimitive ||
            propertyInfo.PropertyType == typeof(string))
        {
            return GetPrimitiveValidator(propertyInfo);
        }
        else if (propertyInfo.PropertyType.IsEnum)
        {
            return Activator.CreateInstance(typeof(EnumValidator<>).MakeGenericType(propertyInfo.PropertyType)) as IValidator;
        }

        return default;
    }

    private static IValidator? GetPrimitiveValidator(PropertyInfo propertyInfo)
    {
        if (propertyInfo.PropertyType == typeof(byte) ||
            propertyInfo.PropertyType == typeof(byte?))
        {
            var validator = GetClampedValidator(propertyInfo, byte.MinValue, byte.MaxValue, out _);
            return validator;
        }
        else if (propertyInfo.PropertyType == typeof(sbyte) ||
                propertyInfo.PropertyType == typeof(sbyte?))
        {
            var validator = GetClampedValidator(propertyInfo, sbyte.MinValue, sbyte.MaxValue, out _);
            return validator;
        }
        else if (propertyInfo.PropertyType == typeof(short) ||
                propertyInfo.PropertyType == typeof(short?))
        {
            var validator = GetClampedValidator(propertyInfo, short.MinValue, short.MaxValue, out _);
            return validator;
        }
        else if (propertyInfo.PropertyType == typeof(ushort) ||
                propertyInfo.PropertyType == typeof(ushort?))
        {
            var validator = GetClampedValidator(propertyInfo, ushort.MinValue, ushort.MaxValue, out _);
            return validator;
        }
        else if (propertyInfo.PropertyType == typeof(int) ||
                propertyInfo.PropertyType == typeof(int?))
        {
            var validator = GetClampedValidator(propertyInfo, int.MinValue, int.MaxValue, out _);
            return validator;
        }
        else if (propertyInfo.PropertyType == typeof(uint) ||
                propertyInfo.PropertyType == typeof(uint?))
        {
            var validator = GetClampedValidator(propertyInfo, uint.MinValue, uint.MaxValue, out _);
            return validator;
        }
        else if (propertyInfo.PropertyType == typeof(long) ||
                propertyInfo.PropertyType == typeof(long?))
        {
            var validator = GetClampedValidator(propertyInfo, long.MinValue, long.MaxValue, out _);
            return validator;
        }
        else if (propertyInfo.PropertyType == typeof(ulong) ||
                propertyInfo.PropertyType == typeof(ulong?))
        {
            var validator = GetClampedValidator(propertyInfo, ulong.MinValue, ulong.MaxValue, out _);
            return validator;
        }
        else if (propertyInfo.PropertyType == typeof(float) ||
                propertyInfo.PropertyType == typeof(float?))
        {
            var validator = GetClampedValidator(propertyInfo, float.MinValue, float.MaxValue, out _);
            return validator;
        }
        else if (propertyInfo.PropertyType == typeof(double) ||
                propertyInfo.PropertyType == typeof(double?))
        {
            var validator = GetClampedValidator(propertyInfo, double.MinValue, double.MaxValue, out _);
            return validator;
        }
        else if (propertyInfo.PropertyType == typeof(bool) ||
                propertyInfo.PropertyType == typeof(bool?))
        {
            return new BooleanValidator();
        }
        else if (propertyInfo.PropertyType == typeof(string))
        {
            return new AllGoesValidator();
        }

        return default;
    }

    private static ClampedValidator<T>? GetClampedValidator<T>(PropertyInfo propertyInfo, T defaultMinValue, T defaultMaxValue, out (bool IsRange, T Min, T Max) clampDetails)
        where T : IComparable<T>
    {
        var maybeOptionRangeAttribute = propertyInfo.GetCustomAttribute<OptionRangeAttribute<T>>();
        var minValue = maybeOptionRangeAttribute is not null ?
            maybeOptionRangeAttribute.MinValue :
            defaultMinValue;

        var maxValue = maybeOptionRangeAttribute is not null ?
            maybeOptionRangeAttribute.MaxValue :
            defaultMaxValue;

        clampDetails = maybeOptionRangeAttribute is not null ?
            (true, minValue, maxValue) :
            (false, minValue, maxValue);

        return new ClampedValidator<T>(minValue, maxValue);
    }

    private static (string Name, string Description) GetNameAndDescription(PropertyInfo propertyInfo, string optionName)
    {
        string? name;
        var description = string.Empty;
        if (propertyInfo.GetCustomAttribute<OptionNameAttribute>() is not OptionNameAttribute optionNameAttribute)
        {
            name = propertyInfo.Name;
        }
        else
        {
            name = optionNameAttribute.Name;
            description = optionNameAttribute.Description;
        }

        if (description.IsNullOrWhiteSpace())
        {
            description = GetDefaultDescription(name, optionName);
        }

        return (name, description);
    }

    private static string GetDefaultDescription(string optionName, string optionSectionName)
    {
        return $"{optionName} property of {optionSectionName}";
    }
}
