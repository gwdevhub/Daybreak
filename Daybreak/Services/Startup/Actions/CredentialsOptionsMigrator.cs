using Daybreak.Attributes;
using Daybreak.Configuration.Options;
using Daybreak.Services.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Core.Extensions;
using System.Extensions;
using System.Reflection;

namespace Daybreak.Services.Startup.Actions;
internal sealed class CredentialsOptionsMigrator : StartupActionBase
{
    private const string OldOptionsKey = "CredentialManagerOptions";

    private readonly IOptionsProvider optionsProvider;
    private readonly ILogger<CredentialsOptionsMigrator> logger;

    public CredentialsOptionsMigrator(
        IOptionsProvider optionsProvider,
        ILogger<CredentialsOptionsMigrator> logger)
    {
        this.optionsProvider = optionsProvider.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public override void ExecuteOnStartup()
    {
        var newOptionsKey = GetNewOptionsKey();
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.ExecuteOnStartup), string.Empty);
        if (this.optionsProvider.TryGetKeyedOptions(newOptionsKey) is JObject)
        {
            scopedLogger.LogDebug($"Found [{newOptionsKey}]. No migration needed");
            return;
        }

        if (this.optionsProvider.TryGetKeyedOptions(OldOptionsKey) is not JObject options)
        {
            scopedLogger.LogDebug($"Could not find [{OldOptionsKey}]. No migration possible");
            return;
        }

        this.logger.LogInformation($"Found [{OldOptionsKey}]. Migrating options to [{newOptionsKey}]");
        this.optionsProvider.SaveRegisteredOptions(newOptionsKey, options);
    }

    private static string GetNewOptionsKey()
    {
        return typeof(CredentialManagerOptions).GetCustomAttribute<OptionsNameAttribute>()?.Name ?? throw new InvalidOperationException("Unable to retrieve new credentials options key");
    }
}
