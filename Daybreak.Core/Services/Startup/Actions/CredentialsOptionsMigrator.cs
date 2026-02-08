using Daybreak.Configuration.Options;
using Daybreak.Shared.Attributes;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Options;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;
using System.Reflection;
using System.Text.Json;

namespace Daybreak.Services.Startup.Actions;
internal sealed class CredentialsOptionsMigrator(
    IOptionsProvider optionsProvider,
    ILogger<CredentialsOptionsMigrator> logger) : StartupActionBase
{
    private const string OldOptionsKey = "CredentialManagerOptions";

    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull();
    private readonly ILogger<CredentialsOptionsMigrator> logger = logger.ThrowIfNull();

    public override void ExecuteOnStartup()
    {
        var newOptionsKey = GetNewOptionsKey();
        var scopedLogger = this.logger.CreateScopedLogger();
        if (this.optionsProvider.TryGetKeyedOptions(newOptionsKey) is not null)
        {
            scopedLogger.LogDebug("Found [{newOptionsKey}]. No migration needed", newOptionsKey);
            return;
        }

        if (this.optionsProvider.TryGetKeyedOptions(OldOptionsKey) is not JsonDocument options)
        {
            scopedLogger.LogDebug("Could not find [{OldOptionsKey}]. No migration possible", OldOptionsKey);
            return;
        }

        this.logger.LogDebug("Found [{OldOptionsKey}]. Migrating options to [{newOptionsKey}]", OldOptionsKey, newOptionsKey);
        this.optionsProvider.SaveRegisteredOptions(newOptionsKey, options);
    }

    private static string GetNewOptionsKey()
    {
        return typeof(CredentialManagerOptions).GetCustomAttribute<OptionsNameAttribute>()?.Name ?? throw new InvalidOperationException("Unable to retrieve new credentials options key");
    }
}
