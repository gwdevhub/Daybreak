using Daybreak.Attributes;
using Daybreak.Configuration.Options;
using Daybreak.Services.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Core.Extensions;
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
        if (this.optionsProvider.TryGetKeyedOptions(OldOptionsKey) is not JObject options)
        {
            return;
        }

        var newOptionsKey = GetNewOptionsKey();
        this.logger.LogInformation($"Found [{OldOptionsKey}]. Migrating options to [{newOptionsKey}]");
        this.optionsProvider.SaveRegisteredOptions(newOptionsKey, options);
    }

    private static string GetNewOptionsKey()
    {
        return typeof(CredentialManagerOptions).GetCustomAttribute<OptionsNameAttribute>()?.Name ?? throw new InvalidOperationException("Unable to retrieve new credentials options key");
    }
}
