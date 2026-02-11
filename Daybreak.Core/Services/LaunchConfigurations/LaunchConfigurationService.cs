using Daybreak.Configuration.Options;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Services.Credentials;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.LaunchConfigurations;
using Daybreak.Shared.Services.Options;
using Microsoft.Extensions.Options;
using System.Core.Extensions;
using System.Extensions;

namespace Daybreak.Services.LaunchConfigurations;

internal sealed class LaunchConfigurationService(
    IOptionsMonitor<LaunchConfigurationServiceOptions> liveUpdateableOptions,
    IOptionsProvider optionsProvider,
    ICredentialManager credentialManager,
    IGuildWarsExecutableManager guildWarsExecutableManager) : ILaunchConfigurationService
{
    private readonly ICredentialManager credentialManager = credentialManager.ThrowIfNull();
    private readonly IGuildWarsExecutableManager guildWarsExecutableManager = guildWarsExecutableManager.ThrowIfNull();
    private readonly IOptionsMonitor<LaunchConfigurationServiceOptions> liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull();

    public IEnumerable<LaunchConfigurationWithCredentials> GetLaunchConfigurations()
    {
        return this.liveUpdateableOptions.CurrentValue.LaunchConfigurations
            .Select(this.ConvertToConfigurationWithCredentials)
            .OfType<LaunchConfigurationWithCredentials>()
            .Where(this.IsValidInternal);
    }

    public LaunchConfigurationWithCredentials? CreateConfiguration()
    {
        var config = new LaunchConfigurationWithCredentials
        {
            Identifier = Guid.NewGuid().ToString()
        };

        return config;
    }

    public bool SaveConfiguration(LaunchConfigurationWithCredentials launchConfigurationWithCredentials)
    {
        launchConfigurationWithCredentials.ThrowIfNull();
        if (!this.credentialManager.TryGetCredentialsByIdentifier(launchConfigurationWithCredentials.Credentials?.Identifier!, out var _))
        {
            return false;
        }

        if (!this.IsValid(launchConfigurationWithCredentials))
        {
            return false;
        }

        return this.SaveConfigurationInternal(launchConfigurationWithCredentials);
    }

    public bool DeleteConfiguration(LaunchConfigurationWithCredentials launchConfigurationWithCredentials)
    {
        launchConfigurationWithCredentials.ThrowIfNull();
        var configs = this.liveUpdateableOptions.CurrentValue.LaunchConfigurations;
        var maybeConfig = configs
            .FirstOrDefault(l => l.Identifier == launchConfigurationWithCredentials.Identifier);

        if (maybeConfig is null)
        {
            return false;
        }

        configs.Remove(maybeConfig);
        var options = this.liveUpdateableOptions.CurrentValue;
        options.LaunchConfigurations = configs;
        this.optionsProvider.SaveOption(options);
        return true;
    }

    public bool IsValid(LaunchConfigurationWithCredentials launchConfiguration)
    {
        launchConfiguration.ThrowIfNull();

        return this.IsValidInternal(launchConfiguration);
    }

    public LaunchConfigurationWithCredentials? GetLastLaunchConfigurationWithCredentials()
    {
        return this.GetLaunchConfigurations().LastOrDefault();
    }

    public void SetLastLaunchConfigurationWithCredentials(LaunchConfigurationWithCredentials launchConfigurationWithCredentials)
    {
        var configs = this.liveUpdateableOptions.CurrentValue.LaunchConfigurations;
        var maybeConfig = configs
            .FirstOrDefault(l => l.CredentialsIdentifier == launchConfigurationWithCredentials.Credentials?.Identifier &&
                                 l.Executable == launchConfigurationWithCredentials.ExecutablePath) ?? throw new InvalidOperationException("Provided launch configuration is not part of the known list of launch configurations");
        configs.Remove(maybeConfig);
        configs.Add(maybeConfig);
        var options = this.liveUpdateableOptions.CurrentValue;
        options.LaunchConfigurations = configs;
        this.optionsProvider.SaveOption(options);
    }

    private bool SaveConfigurationInternal(LaunchConfigurationWithCredentials launchConfigurationWithCredentials)
    {
        var configs = this.liveUpdateableOptions.CurrentValue.LaunchConfigurations.ToList();
        if (configs.FirstOrDefault(c => c.Identifier == launchConfigurationWithCredentials.Identifier) is
            LaunchConfiguration config)
        {
            config.Name = launchConfigurationWithCredentials.Name;
            config.Identifier = launchConfigurationWithCredentials.Identifier;
            config.CredentialsIdentifier = launchConfigurationWithCredentials.Credentials?.Identifier;
            config.Executable = launchConfigurationWithCredentials.ExecutablePath;
            config.Arguments = launchConfigurationWithCredentials.Arguments;
            config.SteamSupport = launchConfigurationWithCredentials.SteamSupport;
            config.EnabledMods = launchConfigurationWithCredentials.EnabledMods;
            config.CustomModLoadoutEnabled = launchConfigurationWithCredentials.CustomModLoadoutEnabled;
            config.Color = launchConfigurationWithCredentials.Color;
            var options = this.liveUpdateableOptions.CurrentValue;
            options.LaunchConfigurations = configs;
            this.optionsProvider.SaveOption(options);
            return true;
        }

        configs.Insert(0, new LaunchConfiguration
        {
            Name = launchConfigurationWithCredentials.Name,
            CredentialsIdentifier = launchConfigurationWithCredentials.Credentials?.Identifier,
            Executable = launchConfigurationWithCredentials.ExecutablePath,
            Identifier = launchConfigurationWithCredentials.Identifier,
            Arguments = launchConfigurationWithCredentials.Arguments,
            SteamSupport = launchConfigurationWithCredentials.SteamSupport,
            EnabledMods = launchConfigurationWithCredentials.EnabledMods,
            CustomModLoadoutEnabled = launchConfigurationWithCredentials.CustomModLoadoutEnabled,
            Color = launchConfigurationWithCredentials.Color
        });
        var optionsNew = this.liveUpdateableOptions.CurrentValue;
        optionsNew.LaunchConfigurations = configs;
        this.optionsProvider.SaveOption(optionsNew);
        return true;
    }

    private bool IsValidInternal(LaunchConfigurationWithCredentials launchConfiguration)
    {
        return launchConfiguration.ExecutablePath is null ||
                this.guildWarsExecutableManager.IsValidExecutable(launchConfiguration.ExecutablePath);
    }

    private LaunchConfigurationWithCredentials? ConvertToConfigurationWithCredentials(LaunchConfiguration launchConfiguration)
    {
        if (!this.credentialManager.TryGetCredentialsByIdentifier(launchConfiguration.CredentialsIdentifier!, out var credentials))
        {
            return default;
        }

        return new LaunchConfigurationWithCredentials
        {
            Identifier = launchConfiguration.Identifier,
            Credentials = credentials,
            ExecutablePath = launchConfiguration.Executable,
            Arguments = launchConfiguration.Arguments,
            Name = launchConfiguration.Name,
            SteamSupport = launchConfiguration.SteamSupport ?? true,
            EnabledMods = launchConfiguration.EnabledMods,
            CustomModLoadoutEnabled = launchConfiguration.CustomModLoadoutEnabled ?? false,
            Color = launchConfiguration.Color
        };
    }
}
