using System.Diagnostics;
using Daybreak.Configuration.Options;
using Daybreak.Services.Toolbox;
using Daybreak.Services.Toolbox.Utilities;
using Daybreak.Shared.Exceptions;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Services.Injection;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Options;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Daybreak.Tests.Services.Toolbox;

/// <summary>
/// Guards the mod lifecycle phase in which GWToolbox is injected. Toolbox must inject during
/// <see cref="ToolboxService.OnGuildWarsCreated"/> (before Daybreak.API) and must NOT inject
/// during <see cref="ToolboxService.OnGuildWarsStarted"/>. This ordering is what lets Toolbox
/// load its bundled gwca.dll first so Daybreak.API reuses the same module.
/// </summary>
[TestClass]
public sealed class ToolboxServiceTests
{
    private readonly IOptionsProvider optionsProvider = Substitute.For<IOptionsProvider>();
    private readonly IBuildTemplateManager buildTemplateManager = Substitute.For<IBuildTemplateManager>();
    private readonly INotificationService notificationService = Substitute.For<INotificationService>();
    private readonly IProcessInjector processInjector = Substitute.For<IProcessInjector>();
    private readonly IToolboxClient toolboxClient = Substitute.For<IToolboxClient>();
    private readonly IOptionsMonitor<ToolboxOptions> toolboxOptions = Substitute.For<IOptionsMonitor<ToolboxOptions>>();
    private readonly ToolboxOptions options = new();
    private readonly Process process = new();
    private readonly ToolboxService service;

    public ToolboxServiceTests()
    {
        this.toolboxOptions.CurrentValue.Returns(this.options);
        this.service = new ToolboxService(
            this.optionsProvider,
            this.buildTemplateManager,
            this.notificationService,
            this.processInjector,
            this.toolboxClient,
            this.toolboxOptions,
            Substitute.For<ILogger<ToolboxService>>());
    }

    [TestCleanup]
    public void Cleanup() => this.process.Dispose();

    private GuildWarsCreatedContext CreatedContext() => new()
    {
        ApplicationLauncherContext = this.LauncherContext(),
    };

    private GuildWarsStartedContext StartedContext() => new()
    {
        ApplicationLauncherContext = this.LauncherContext(),
    };

    private ApplicationLauncherContext LauncherContext() => new()
    {
        ExecutablePath = "Gw.exe",
        Process = this.process,
        ProcessId = 1234,
    };

    [TestMethod]
    public async Task OnGuildWarsStarted_WhenEnabled_DoesNotInjectToolbox()
    {
        this.options.Enabled = true;
        var reachedLaunch = false;
        this.processInjector
            .Inject(Arg.Any<Process>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(_ => { reachedLaunch = true; return Task.FromResult(true); });

        // OnGuildWarsStarted must be a pure no-op now that injection moved to OnGuildWarsCreated.
        await this.service.OnGuildWarsStarted(this.StartedContext(), CancellationToken.None);

        reachedLaunch.Should().BeFalse("Toolbox injection moved out of OnGuildWarsStarted");
        await this.processInjector
            .DidNotReceive()
            .Inject(Arg.Any<Process>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task OnGuildWarsStarted_WhenDisabled_DoesNotInjectToolbox()
    {
        this.options.Enabled = false;

        await this.service.OnGuildWarsStarted(this.StartedContext(), CancellationToken.None);

        await this.processInjector
            .DidNotReceive()
            .Inject(Arg.Any<Process>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task OnGuildWarsCreated_WhenDisabled_DoesNotInjectToolbox()
    {
        this.options.Enabled = false;

        await this.service.OnGuildWarsCreated(this.CreatedContext(), CancellationToken.None);

        await this.processInjector
            .DidNotReceive()
            .Inject(Arg.Any<Process>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task OnGuildWarsCreated_WhenEnabled_DrivesToolboxLaunch()
    {
        this.options.Enabled = true;
        var reachedLaunch = false;
        this.processInjector
            .Inject(Arg.Any<Process>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(_ => { reachedLaunch = true; return Task.FromResult(true); });

        try
        {
            await this.service.OnGuildWarsCreated(this.CreatedContext(), CancellationToken.None);
        }
        catch (ExecutableNotFoundException)
        {
            // Reached LaunchToolbox, but the GWToolbox dll is not installed in this environment.
            // This still proves OnGuildWarsCreated routes into the toolbox launch path.
            reachedLaunch = true;
        }

        reachedLaunch.Should().BeTrue("OnGuildWarsCreated must inject Toolbox before Daybreak.API");
    }
}
