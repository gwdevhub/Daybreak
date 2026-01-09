using Daybreak.Configuration.Options;
using Daybreak.Services.Graph;
using Daybreak.Shared.Services.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Reflection;

namespace Daybreak.Services.Options;

public sealed class OptionsSynchronizationService(
    IOptionsProvider optionsProvider,
    IGraphClient graphClient,
    ILiveOptions<LauncherOptions> liveOptions,
    ILogger<OptionsSynchronizationService> logger) : IOptionsSynchronizationService, IHostedService
{
    private static readonly TimeSpan StartupDelay = TimeSpan.FromMinutes(1);
    private static readonly TimeSpan BackupFrequency = TimeSpan.FromSeconds(15);

    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull();
    private readonly IGraphClient graphClient = graphClient.ThrowIfNull();
    private readonly ILiveOptions<LauncherOptions> liveOptions = liveOptions.ThrowIfNull();
    private readonly ILogger<OptionsSynchronizationService> logger = logger.ThrowIfNull();

    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(StartupDelay, cancellationToken);
        while (true)
        {
            await Task.Delay(BackupFrequency, cancellationToken);
            var remoteOptions = await this.GetRemoteOptionsInternal(cancellationToken);
            var remoteOptionsSerialized = JsonConvert.SerializeObject(remoteOptions);
            var currentOptions = JsonConvert.SerializeObject(this.GetCurrentOptionsInternal());
            if (remoteOptions is not null &&
                currentOptions != remoteOptionsSerialized &&
                this.liveOptions.Value.AutoBackupSettings)
            {
                try
                {
                    await this.BackupOptions(cancellationToken);
                }
                catch
                {
                }
            }
        }
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
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
        return this.optionsProvider.GetRegisteredOptionInstances()
            .Where(o => o.Type.IsSynchronized)
            .Select(o => (o, JObject.FromObject(o.Reference)))
            .Select(t =>
            {
                var jObject = t.Item2;
                var objectType = t.o.Type.Type;
                var properties = t.o.Type.Properties;
                foreach(var property in properties)
                {
                    if (property.Type.GetCustomAttributes().Any(a => a is JsonIgnoreAttribute))
                    {
                        continue;
                    }

                    if (!property.IsSynchronized)
                    {
                        var name = property.Name;
                        if (property.Type.GetCustomAttribute<JsonPropertyAttribute>() is JsonPropertyAttribute jsonPropertyAttribute &&
                            jsonPropertyAttribute.PropertyName is string propertyName)
                        {
                            name = propertyName;
                        }

                        jObject.Remove(name);
                    }
                }

                return (t.o.Type.Name, t.Item2);
            })
            .ToDictionary();
    }
}
