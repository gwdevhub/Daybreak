using Daybreak.Services.Graph;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Options;
using Microsoft.Extensions.Logging;
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
    ILogger<OptionsSynchronizationService> logger) : IOptionsSynchronizationService
{
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull();
    private readonly IGraphClient graphClient = graphClient.ThrowIfNull();
    private readonly ILogger<OptionsSynchronizationService> logger = logger.ThrowIfNull();

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
        if (!result)
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

        foreach (var tuple in remoteOptions)
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
