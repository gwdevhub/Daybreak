using Daybreak.Configuration.Options;
using Daybreak.Services.Graph;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Core.Extensions;
using System.Extensions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daybreak.Services.Options;

public sealed class OptionsSynchronizationService(
    INotificationService notificationService,
    IOptionsProvider optionsProvider,
    IGraphClient graphClient,
    IOptionsMonitor<LauncherOptions> liveOptions,
    ILogger<OptionsSynchronizationService> logger) : IOptionsSynchronizationService, IHostedService
{
    private static readonly TimeSpan StartupDelay = TimeSpan.FromMinutes(1);
    private static readonly TimeSpan BackupFrequency = TimeSpan.FromSeconds(15);

    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull();
    private readonly IGraphClient graphClient = graphClient.ThrowIfNull();
    private readonly IOptionsMonitor<LauncherOptions> liveOptions = liveOptions.ThrowIfNull();
    private readonly ILogger<OptionsSynchronizationService> logger = logger.ThrowIfNull();

    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(StartupDelay, cancellationToken);
        while (true)
        {
            await Task.Delay(BackupFrequency, cancellationToken);
            var remoteOptions = await this.GetRemoteOptionsInternal(cancellationToken);
            var remoteOptionsSerialized = JsonSerializer.Serialize(remoteOptions);
            var currentOptions = JsonSerializer.Serialize(this.GetCurrentOptionsInternal());
            if (remoteOptions is not null &&
                currentOptions != remoteOptionsSerialized &&
                this.liveOptions.CurrentValue.AutoBackupSettings)
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

    public Task<Dictionary<string, JsonDocument>> GetLocalOptions(CancellationToken cancellationToken)
    {
        return Task.FromResult(this.GetCurrentOptionsInternal());
    }

    public async Task<Dictionary<string, JsonDocument>?> GetRemoteOptions(CancellationToken cancellationToken)
    {
        return await this.GetRemoteOptionsInternal(cancellationToken);
    }

    public async Task BackupOptions(CancellationToken cancellationToken)
    {
        var currentOptions = this.GetCurrentOptionsInternal();
        var serializedOptions = JsonSerializer.Serialize(currentOptions);
        var result = await this.graphClient.UploadSettings(serializedOptions, cancellationToken);
        if (result is false)
        {
            this.notificationService.NotifyError(
                title: "Failed to backup settings",
                description: "Encountered an error while backing up your settings to the cloud. Check logs for details.");
        }
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

    private async Task<Dictionary<string, JsonDocument>?> GetRemoteOptionsInternal(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetRemoteOptionsInternal), string.Empty);
        var maybeContent = await this.graphClient.DownloadSettings(cancellationToken);
        if (maybeContent is null)
        {
            scopedLogger.LogError("Failed to download remote settings");
            return default;
        }

        return JsonSerializer.Deserialize<Dictionary<string, JsonDocument>>(maybeContent);
    }

    private Dictionary<string, JsonDocument> GetCurrentOptionsInternal()
    {
        return this.optionsProvider.GetRegisteredOptionInstances()
            .Where(o => o.Type.IsSynchronized)
            .Select(o => (o, JsonSerializer.SerializeToDocument(o.Reference)))
            .Select(t =>
            {
                var jsonDocument = t.Item2;
                var objectType = t.o.Type.Type;
                var properties = t.o.Type.Properties;

                // Create a mutable dictionary from the JSON
                var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonDocument.RootElement.GetRawText())
                    ?? [];

                foreach (var property in properties)
                {
                    if (property.Type.GetCustomAttributes().Any(a => a is JsonIgnoreAttribute))
                    {
                        continue;
                    }

                    if (!property.IsSynchronized)
                    {
                        var name = property.Name;
                        if (property.Type.GetCustomAttribute<JsonPropertyNameAttribute>() is JsonPropertyNameAttribute jsonPropertyNameAttribute)
                        {
                            name = jsonPropertyNameAttribute.Name;
                        }

                        dict.Remove(name);
                    }
                }

                // Convert back to JsonDocument
                var filteredDocument = JsonSerializer.SerializeToDocument(dict);
                return (t.o.Type.Name, filteredDocument);
            })
            .ToDictionary();
    }
}
