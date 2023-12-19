using Daybreak.Attributes;
using Daybreak.Services.Graph;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Options;

public sealed class OptionsSynchronizationService : IOptionsSynchronizationService
{
    private readonly IOptionsProvider optionsProvider;
    private readonly IGraphClient graphClient;
    private readonly ILogger<OptionsSynchronizationService> logger;

    public OptionsSynchronizationService(
        IOptionsProvider optionsProvider,
        IGraphClient graphClient,
        ILogger<OptionsSynchronizationService> logger)
    {
        this.optionsProvider = optionsProvider.ThrowIfNull();
        this.graphClient = graphClient.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public Task<Dictionary<string, JObject>> GetLocalOptions(CancellationToken cancellationToken)
    {
        return Task.FromResult(this.GetCurrentOptionsInternal());
    }

    public async Task<Dictionary<string, JObject>?> GetRemoteOptions(CancellationToken cancellationToken)
    {
        return await this.GetRemoteOptionsInternal(cancellationToken);
    }

    public async Task BackupOptions(CancellationToken cancellationToken)
    {
        var currentOptions = this.GetCurrentOptionsInternal();
        var serializedOptions = JsonConvert.SerializeObject(currentOptions);
        var result = await this.graphClient.UploadSettings(serializedOptions, cancellationToken);
        result.DoAny(
            onFailure: exception =>
            {
                throw exception;
            });
    }

    public async Task RestoreOptions(CancellationToken cancellationToken)
    {
        var remoteOptions = await this.GetRemoteOptions(cancellationToken);
        if (remoteOptions is null)
        {
            return;
        }

        foreach(var tuple in remoteOptions)
        {
            this.optionsProvider.SaveRegisteredOptions(tuple.Key, tuple.Value);
        }
    }

    private async Task<Dictionary<string, JObject>?> GetRemoteOptionsInternal(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetRemoteOptionsInternal), string.Empty);
        var maybeContent = await this.graphClient.DownloadSettings(cancellationToken);
        return maybeContent.Switch(
            onSuccess: content =>
            {
                try
                {
                    return JsonConvert.DeserializeObject<Dictionary<string, JObject>>(content);
                }
                catch(Exception e)
                {
                    scopedLogger.LogError(e, "Encountered exception when deserializing content");
                    return default;
                }
            },
            onFailure: ex =>
            {
                scopedLogger.LogError(ex, "Encountered exception when retrieving remote options");
                return default;
            });
    }

    private Dictionary<string, JObject> GetCurrentOptionsInternal()
    {
        return this.optionsProvider.GetRegisteredOptions()
            .Where(o => o.GetType().GetCustomAttributes(true).None(a => a is OptionsSynchronizationIgnoreAttribute))
            .Select(o => (GetOptionsName(o.GetType()), JObject.FromObject(o), o.GetType()))
            .Select(t =>
            {
                var jObject = t.Item2;
                var objectType = t.Item3;
                var properties = objectType.GetProperties().Where(p => p.GetCustomAttribute<JsonIgnoreAttribute>() is null);
                foreach(var property in properties)
                {
                    if (property.GetCustomAttributes().Any(a => a is OptionSynchronizationIgnoreAttribute))
                    {
                        var name = property.Name;
                        if (property.GetCustomAttribute<JsonPropertyAttribute>() is JsonPropertyAttribute jsonPropertyAttribute &&
                            jsonPropertyAttribute.PropertyName is string propertyName)
                        {
                            name = propertyName;
                        }

                        jObject.Remove(name);
                    }
                }

                return (t.Item1, t.Item2);
            })
            .ToDictionary();
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
