using Daybreak.Configuration.Options;
using Daybreak.Services.Mods;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Models.UMod;
using Daybreak.Shared.Services.LaunchConfigurations;
using Daybreak.Shared.Services.Options;
using FluentAssertions;
using NSubstitute;
using System.Text.Json;

namespace Daybreak.Tests.Services;

[TestClass]
public sealed class ModSettingsProviderTests
{
    private readonly IOptionsProvider optionsProvider = Substitute.For<IOptionsProvider>();
    private readonly ILaunchConfigurationService configurations = Substitute.For<ILaunchConfigurationService>();
    private readonly ModSettingsProvider provider;

    public ModSettingsProviderTests()
    {
        this.configurations.SaveConfiguration(Arg.Any<LaunchConfigurationWithCredentials>()).Returns(true);
        this.provider = new ModSettingsProvider(this.optionsProvider, this.configurations);
    }

    [TestMethod]
    public void Get_UncustomizedLoadout_InheritsLiveGlobalsWithoutCreatingOverrides()
    {
        var configuration = new LaunchConfigurationWithCredentials { CustomModLoadoutEnabled = true };
        var global = new GuildWarsScreenPlacerOptions { DesiredScreen = 1 };

        this.provider.Get("Screen placer", global, configuration).DesiredScreen.Should().Be(1);
        global.DesiredScreen = 2;
        this.provider.Get("Screen placer", global, configuration).DesiredScreen.Should().Be(2);

        configuration.ModLoadout.Should().BeEmpty();
        this.configurations.DidNotReceive().SaveConfiguration(Arg.Any<LaunchConfigurationWithCredentials>());
    }

    [TestMethod]
    public void Save_OverridesRuntimeSettingsOnly_AndKeepsEnabledState()
    {
        var configuration = new LaunchConfigurationWithCredentials
        {
            CustomModLoadoutEnabled = true,
            ModLoadout = new() { ["DXVK"] = new ModLoadoutEntry { IsEnabled = true } },
        };

        this.provider.Save("DXVK", new DXVKOptions { Version = "2.0", Enabled = false }, configuration);

        configuration.ModLoadout["DXVK"].IsEnabled.Should().BeTrue();
        configuration.ModLoadout["DXVK"].Settings.Should().BeEmpty();
        this.optionsProvider.DidNotReceive().SaveOption(Arg.Any<DXVKOptions>());
        this.configurations.Received(1).SaveConfiguration(Arg.Is<LaunchConfigurationWithCredentials>(c => ReferenceEquals(c, configuration)));
    }

    [TestMethod]
    public void Get_OnlyAppliesRuntimeOverrides_EvenWhenStoredDataContainsGlobalFields()
    {
        var configuration = new LaunchConfigurationWithCredentials
        {
            CustomModLoadoutEnabled = true,
            ModLoadout = new()
            {
                ["Screen placer"] = new ModLoadoutEntry
                {
                    Settings = new()
                    {
                        [nameof(GuildWarsScreenPlacerOptions.DesiredScreen)] = JsonSerializer.SerializeToElement(2),
                        [nameof(GuildWarsScreenPlacerOptions.Enabled)] = JsonSerializer.SerializeToElement(false),
                    },
                },
            },
        };
        var global = new GuildWarsScreenPlacerOptions { Enabled = true, DesiredScreen = 0 };

        var settings = this.provider.Get("Screen placer", global, configuration);

        settings.DesiredScreen.Should().Be(2);
        settings.Enabled.Should().BeTrue();
        global.DesiredScreen.Should().Be(0);
    }

    [TestMethod]
    public async Task Settings_AreIsolatedAcrossConcurrentLoadouts_AndSurviveSerialization()
    {
        var first = new LaunchConfigurationWithCredentials { Identifier = "first", CustomModLoadoutEnabled = true };
        var second = new LaunchConfigurationWithCredentials { Identifier = "second", CustomModLoadoutEnabled = true };
        var global = new GuildWarsScreenPlacerOptions { DesiredScreen = 0 };

        await Task.WhenAll(
            Task.Run(() => this.provider.Save("Screen placer", new GuildWarsScreenPlacerOptions { DesiredScreen = 1 }, first)),
            Task.Run(() => this.provider.Save("Screen placer", new GuildWarsScreenPlacerOptions { DesiredScreen = 2 }, second)));
        var restored = JsonSerializer.Deserialize<LaunchConfigurationWithCredentials>(JsonSerializer.Serialize(first))!;

        this.provider.Get("Screen placer", global, restored).DesiredScreen.Should().Be(1);
        this.provider.Get("Screen placer", global, second).DesiredScreen.Should().Be(2);
        this.provider.Get("Screen placer", global).DesiredScreen.Should().Be(0);
        global.DesiredScreen.Should().Be(0);
        restored.ModLoadout["Screen placer"].IsEnabled.Should().BeFalse();
    }

    [TestMethod]
    public void TextureSettings_AreDetachedUntilSaved_AndDoNotChangeOtherLoadouts()
    {
        var global = new UModOptions
        {
            Mods =
            [
                new UModEntry { Name = "First", PathToFile = "first.tpf", Enabled = true },
                new UModEntry { Name = "Second", PathToFile = "second.tpf", Enabled = true },
            ],
        };
        var configuration = new LaunchConfigurationWithCredentials { CustomModLoadoutEnabled = true };
        var settings = this.provider.Get("gMod", global, configuration);
        settings.Mods.Reverse();
        settings.Mods[0].Enabled = false;

        this.provider.Save("gMod", settings, configuration);
        var restored = this.provider.Get("gMod", global, configuration);

        restored.Mods.Select(m => m.Name).Should().Equal("Second", "First");
        restored.Mods[0].Enabled.Should().BeFalse();
        global.Mods.Select(m => m.Name).Should().Equal("First", "Second");
        global.Mods.Should().OnlyContain(m => m.Enabled);
        restored.Mods.Clear();
        this.provider.Get("gMod", global, configuration).Mods.Should().HaveCount(2);
    }

    [TestMethod]
    public void DisabledLoadout_UsesGlobals_WithoutDiscardingOverrides()
    {
        var configuration = new LaunchConfigurationWithCredentials { CustomModLoadoutEnabled = true };
        var global = new GuildWarsScreenPlacerOptions { DesiredScreen = 0 };
        this.provider.Save("Screen placer", new GuildWarsScreenPlacerOptions { DesiredScreen = 2 }, configuration);

        configuration.CustomModLoadoutEnabled = false;
        this.provider.Get("Screen placer", global, configuration).DesiredScreen.Should().Be(0);
        configuration.CustomModLoadoutEnabled = true;
        this.provider.Get("Screen placer", global, configuration).DesiredScreen.Should().Be(2);
    }

    [TestMethod]
    public void ClearedOverrides_InheritGlobalsAgain_WithoutDisablingMod()
    {
        var configuration = new LaunchConfigurationWithCredentials
        {
            CustomModLoadoutEnabled = true,
            ModLoadout = new() { ["Screen placer"] = new ModLoadoutEntry { IsEnabled = true } },
        };
        this.provider.Save("Screen placer", new GuildWarsScreenPlacerOptions { DesiredScreen = 2 }, configuration);

        configuration.ModLoadout["Screen placer"].Settings.Clear();

        this.provider.Get("Screen placer", new GuildWarsScreenPlacerOptions { DesiredScreen = 1 }, configuration)
            .DesiredScreen.Should().Be(1);
        configuration.ModLoadout["Screen placer"].IsEnabled.Should().BeTrue();
    }

    [TestMethod]
    public void Save_WithoutLoadout_PersistsGlobalOptions()
    {
        var settings = new GuildWarsScreenPlacerOptions { DesiredScreen = 1 };

        this.provider.Save("Screen placer", settings);

        this.optionsProvider.Received(1).SaveOption(settings);
        this.configurations.DidNotReceive().SaveConfiguration(Arg.Any<LaunchConfigurationWithCredentials>());
    }

    [TestMethod]
    public void Save_PersistenceFailure_RestoresPreviousOverridesAndThrows()
    {
        var previous = new ModLoadoutEntry { IsEnabled = true };
        var configuration = new LaunchConfigurationWithCredentials
        {
            CustomModLoadoutEnabled = true,
            ModLoadout = new() { ["Screen placer"] = previous },
        };
        this.configurations.SaveConfiguration(Arg.Any<LaunchConfigurationWithCredentials>()).Returns(false);

        var save = () => this.provider.Save("Screen placer", new GuildWarsScreenPlacerOptions { DesiredScreen = 2 }, configuration);

        save.Should().Throw<InvalidOperationException>();
        configuration.ModLoadout["Screen placer"].Should().BeSameAs(previous);
        this.optionsProvider.DidNotReceive().SaveOption(Arg.Any<GuildWarsScreenPlacerOptions>());
    }
}
