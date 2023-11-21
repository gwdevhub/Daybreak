using Daybreak.Configuration.Options;
using Daybreak.Models.LaunchConfigurations;
using Daybreak.Services.Credentials;
using Daybreak.Services.ExecutableManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;

namespace Daybreak.Services.LaunchConfigurations;
internal sealed class LaunchConfigurationService : ILaunchConfigurationService
{
    private readonly ICredentialManager credentialManager;
    private readonly IGuildWarsExecutableManager guildWarsExecutableManager;
    private readonly ILiveUpdateableOptions<LaunchConfigurationServiceOptions> liveUpdateableOptions;

    public LaunchConfigurationService(
        ICredentialManager credentialManager,
        IGuildWarsExecutableManager guildWarsExecutableManager,
        ILiveUpdateableOptions<LaunchConfigurationServiceOptions> liveUpdateableOptions)
    {
        this.credentialManager = credentialManager.ThrowIfNull();
        this.guildWarsExecutableManager = guildWarsExecutableManager.ThrowIfNull();
        this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
    }

    public IEnumerable<LaunchConfigurationWithCredentials> GetLaunchConfigurations()
    {
        return this.liveUpdateableOptions.Value.LaunchConfigurations
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
        var configs = this.liveUpdateableOptions.Value.LaunchConfigurations;
        var maybeConfig = configs
            .FirstOrDefault(l => l.Identifier == launchConfigurationWithCredentials.Identifier);

        if (maybeConfig is null)
        {
            return false;
        }

        configs.Remove(maybeConfig);
        this.liveUpdateableOptions.Value.LaunchConfigurations = configs;
        this.liveUpdateableOptions.UpdateOption();
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
        var configs = this.liveUpdateableOptions.Value.LaunchConfigurations;
        var maybeConfig = configs
            .FirstOrDefault(l => l.CredentialsIdentifier == launchConfigurationWithCredentials.Credentials?.Identifier &&
                                 l.Executable == launchConfigurationWithCredentials.ExecutablePath);

        if (maybeConfig is null)
        {
            throw new InvalidOperationException("Provided launch configuration is not part of the known list of launch configurations");
        }

        configs.Remove(maybeConfig);
        configs.Add(maybeConfig);
        this.liveUpdateableOptions.Value.LaunchConfigurations = configs;
        this.liveUpdateableOptions.UpdateOption();
    }

    private bool SaveConfigurationInternal(LaunchConfigurationWithCredentials launchConfigurationWithCredentials)
    {
        var configs = this.liveUpdateableOptions.Value.LaunchConfigurations.ToList();
        if (configs.FirstOrDefault(c => c.Identifier == launchConfigurationWithCredentials.Identifier) is 
            LaunchConfiguration config)
        {
            config.Identifier = launchConfigurationWithCredentials.Identifier;
            config.CredentialsIdentifier = launchConfigurationWithCredentials.Credentials!.Identifier;
            config.Executable = launchConfigurationWithCredentials.ExecutablePath;
            this.liveUpdateableOptions.Value.LaunchConfigurations = configs;
            this.liveUpdateableOptions.UpdateOption();
            return true;
        }

        configs.Add(new LaunchConfiguration
        {
            CredentialsIdentifier = launchConfigurationWithCredentials.Credentials!.Identifier,
            Executable = launchConfigurationWithCredentials.ExecutablePath,
            Identifier = launchConfigurationWithCredentials.Identifier
        });
        this.liveUpdateableOptions.Value.LaunchConfigurations = configs;
        this.liveUpdateableOptions.UpdateOption();
        return true;
    }

    private bool IsValidInternal(LaunchConfigurationWithCredentials launchConfiguration)
    {
        return launchConfiguration.ExecutablePath?.IsNullOrWhiteSpace() is false && this.guildWarsExecutableManager.IsValidExecutable(launchConfiguration.ExecutablePath!);
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
            ExecutablePath = launchConfiguration.Executable
        };
    }
}
