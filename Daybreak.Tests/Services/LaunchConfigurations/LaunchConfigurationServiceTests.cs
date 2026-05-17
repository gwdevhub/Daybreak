using Daybreak.Configuration.Options;
using Daybreak.Services.LaunchConfigurations;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Services.Credentials;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.Options;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Daybreak.Tests.Services.LaunchConfigurations;

[TestClass]
public sealed class LaunchConfigurationServiceTests
{
    private readonly IOptionsMonitor<LaunchConfigurationServiceOptions> liveOptions = Substitute.For<IOptionsMonitor<LaunchConfigurationServiceOptions>>();
    private readonly IOptionsProvider optionsProvider = Substitute.For<IOptionsProvider>();
    private readonly ICredentialManager credentialManager = Substitute.For<ICredentialManager>();
    private readonly IGuildWarsExecutableManager executableManager = Substitute.For<IGuildWarsExecutableManager>();
    private readonly LaunchConfigurationServiceOptions options = new();
    private readonly LaunchConfigurationService service;

    public LaunchConfigurationServiceTests()
    {
        this.liveOptions.CurrentValue.Returns(this.options);
        this.service = new LaunchConfigurationService(
            this.liveOptions, this.optionsProvider, this.credentialManager, this.executableManager);
    }

    private void SeedCredentials(params LoginCredentials[] creds)
    {
        foreach (var c in creds)
        {
            var local = c;
            this.credentialManager.TryGetCredentialsByIdentifier(c.Identifier!, out Arg.Any<LoginCredentials?>())
                .Returns(call => { call[1] = local; return true; });
        }
    }

    [TestMethod]
    public void CreateConfiguration_ReturnsConfigWithFreshGuidIdentifier()
    {
        var cfg = this.service.CreateConfiguration();

        cfg.Should().NotBeNull();
        Guid.TryParse(cfg!.Identifier, out _).Should().BeTrue();
    }

    [TestMethod]
    public void GetLaunchConfigurations_DropsConfigsWithMissingCredentials()
    {
        this.options.LaunchConfigurations =
        [
            new LaunchConfiguration { Identifier = "cfg-1", CredentialsIdentifier = "missing" },
        ];

        this.service.GetLaunchConfigurations().Should().BeEmpty();
    }

    [TestMethod]
    public void GetLaunchConfigurations_DropsConfigsWithInvalidExecutable()
    {
        var cred = new LoginCredentials { Identifier = "cred-1" };
        this.SeedCredentials(cred);
        this.executableManager.IsValidExecutable("bad.exe").Returns(false);
        this.options.LaunchConfigurations =
        [
            new LaunchConfiguration { Identifier = "cfg-1", CredentialsIdentifier = "cred-1", Executable = "bad.exe" },
        ];

        this.service.GetLaunchConfigurations().Should().BeEmpty();
    }

    [TestMethod]
    public void GetLaunchConfigurations_ValidConfig_ReturnsItWithCredentials()
    {
        var cred = new LoginCredentials { Identifier = "cred-1", Username = "u" };
        this.SeedCredentials(cred);
        this.executableManager.IsValidExecutable("good.exe").Returns(true);
        this.options.LaunchConfigurations =
        [
            new LaunchConfiguration { Identifier = "cfg-1", CredentialsIdentifier = "cred-1", Executable = "good.exe", Name = "Main", SteamSupport = false }
        ];

        var configs = this.service.GetLaunchConfigurations().ToList();

        configs.Should().ContainSingle();
        configs[0].Identifier.Should().Be("cfg-1");
        configs[0].Credentials.Should().BeSameAs(cred);
        configs[0].ExecutablePath.Should().Be("good.exe");
        configs[0].SteamSupport.Should().BeFalse();
    }

    [TestMethod]
    public void SaveConfiguration_MissingCredentials_ReturnsFalseAndDoesNotPersist()
    {
        var config = new LaunchConfigurationWithCredentials
        {
            Identifier = "cfg-1",
            Credentials = new LoginCredentials { Identifier = "missing" },
        };

        this.service.SaveConfiguration(config).Should().BeFalse();
        this.optionsProvider.DidNotReceiveWithAnyArgs().SaveOption(Arg.Any<object>());
    }

    [TestMethod]
    public void SaveConfiguration_InvalidExecutable_ReturnsFalse()
    {
        var cred = new LoginCredentials { Identifier = "cred-1" };
        this.SeedCredentials(cred);
        this.executableManager.IsValidExecutable("bad.exe").Returns(false);
        var config = new LaunchConfigurationWithCredentials
        {
            Identifier = "cfg-1",
            Credentials = cred,
            ExecutablePath = "bad.exe",
        };

        this.service.SaveConfiguration(config).Should().BeFalse();
        this.optionsProvider.DidNotReceiveWithAnyArgs().SaveOption(Arg.Any<object>());
    }

    [TestMethod]
    public void SaveConfiguration_NewConfig_InsertsAtFrontAndPersists()
    {
        var cred = new LoginCredentials { Identifier = "cred-1" };
        this.SeedCredentials(cred);
        this.executableManager.IsValidExecutable("gw.exe").Returns(true);
        this.options.LaunchConfigurations =
        [
            new LaunchConfiguration { Identifier = "old", CredentialsIdentifier = "cred-1", Executable = "gw.exe" }
        ];
        var fresh = new LaunchConfigurationWithCredentials
        {
            Identifier = "new",
            Credentials = cred,
            ExecutablePath = "gw.exe",
            Name = "New",
        };

        var ok = this.service.SaveConfiguration(fresh);

        ok.Should().BeTrue();
        this.options.LaunchConfigurations.Select(c => c.Identifier)
            .Should().BeEquivalentTo(["new", "old"], opt => opt.WithStrictOrdering());
        this.optionsProvider.Received(1).SaveOption(this.options);
    }

    [TestMethod]
    public void SaveConfiguration_ExistingIdentifier_UpdatesInPlace()
    {
        var cred = new LoginCredentials { Identifier = "cred-1" };
        this.SeedCredentials(cred);
        this.executableManager.IsValidExecutable("gw.exe").Returns(true);
        this.options.LaunchConfigurations =
        [
            new LaunchConfiguration { Identifier = "cfg-1", CredentialsIdentifier = "cred-1", Executable = "gw.exe", Name = "Old" }
        ];
        var updated = new LaunchConfigurationWithCredentials
        {
            Identifier = "cfg-1",
            Credentials = cred,
            ExecutablePath = "gw.exe",
            Name = "Updated",
        };

        this.service.SaveConfiguration(updated).Should().BeTrue();

        this.options.LaunchConfigurations.Should().ContainSingle()
            .Which.Name.Should().Be("Updated");
    }

    [TestMethod]
    public void DeleteConfiguration_Missing_ReturnsFalse()
    {
        var cfg = new LaunchConfigurationWithCredentials { Identifier = "nope" };

        this.service.DeleteConfiguration(cfg).Should().BeFalse();
        this.optionsProvider.DidNotReceiveWithAnyArgs().SaveOption(Arg.Any<object>());
    }

    [TestMethod]
    public void DeleteConfiguration_Existing_RemovesAndPersists()
    {
        this.options.LaunchConfigurations =
        [
            new LaunchConfiguration { Identifier = "cfg-1" },
            new LaunchConfiguration { Identifier = "cfg-2" },
        ];
        var cfg = new LaunchConfigurationWithCredentials { Identifier = "cfg-1" };

        this.service.DeleteConfiguration(cfg).Should().BeTrue();

        this.options.LaunchConfigurations.Should().ContainSingle()
            .Which.Identifier.Should().Be("cfg-2");
        this.optionsProvider.Received(1).SaveOption(this.options);
    }

    [TestMethod]
    public void SaveLaunchConfigurations_FiltersInvalidAndConfigsWithMissingCredentials()
    {
        var cred = new LoginCredentials { Identifier = "cred-1" };
        this.SeedCredentials(cred);
        this.executableManager.IsValidExecutable("gw.exe").Returns(true);
        this.executableManager.IsValidExecutable("bad.exe").Returns(false);

        var configs = new List<LaunchConfigurationWithCredentials>
        {
            new() { Identifier = "valid", Credentials = cred, ExecutablePath = "gw.exe" },
            new() { Identifier = "invalid-exe", Credentials = cred, ExecutablePath = "bad.exe" },
            new() { Identifier = "no-cred", Credentials = new LoginCredentials { Identifier = "missing" }, ExecutablePath = "gw.exe" },
        };

        this.service.SaveLaunchConfigurations(configs);

        this.options.LaunchConfigurations.Should().ContainSingle()
            .Which.Identifier.Should().Be("valid");
        this.optionsProvider.Received(1).SaveOption(this.options);
    }

    [TestMethod]
    public void IsValid_NullExecutable_IsValid()
    {
        var cfg = new LaunchConfigurationWithCredentials { Identifier = "x", ExecutablePath = null };

        this.service.IsValid(cfg).Should().BeTrue();
    }
}
