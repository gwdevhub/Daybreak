using Daybreak.Services.Startup;
using Daybreak.Shared.Models;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Daybreak.Tests.Services.Startup;

[TestClass]
public sealed class StartupActionManagerTests
{
    [TestMethod]
    public async Task StartAsync_InvokesExecuteOnStartupAndExecuteOnStartupAsync_OnEveryAction()
    {
        var a = new RecordingStartupAction();
        var b = new RecordingStartupAction();
        var manager = new StartupActionManager([a, b], Substitute.For<ILogger<StartupActionManager>>());

        await ((IHostedService)manager).StartAsync(default);

        a.SyncCalled.Should().BeTrue();
        b.SyncCalled.Should().BeTrue();
        a.AsyncCalled.Should().BeTrue();
        b.AsyncCalled.Should().BeTrue();
    }

    [TestMethod]
    public async Task StartAsync_AsyncActionThrows_LogsAndCompletesWithoutPropagating()
    {
        var ok = new RecordingStartupAction();
        var failing = new ThrowingStartupAction();
        var logger = Substitute.For<ILogger<StartupActionManager>>();
        var manager = new StartupActionManager([ok, failing], logger);

        Func<Task> act = () => ((IHostedService)manager).StartAsync(default);

        await act.Should().NotThrowAsync();
        ok.AsyncCalled.Should().BeTrue();
    }

    [TestMethod]
    public async Task StopAsync_IsNoOpAndCompletes()
    {
        var manager = new StartupActionManager([], Substitute.For<ILogger<StartupActionManager>>());

        await ((IHostedService)manager).StopAsync(default);
    }

    private sealed class RecordingStartupAction : StartupActionBase
    {
        public bool SyncCalled { get; private set; }
        public bool AsyncCalled { get; private set; }

        public override void ExecuteOnStartup() => this.SyncCalled = true;
        public override Task ExecuteOnStartupAsync(CancellationToken cancellationToken)
        {
            this.AsyncCalled = true;
            return Task.CompletedTask;
        }
    }

    private sealed class ThrowingStartupAction : StartupActionBase
    {
        public override Task ExecuteOnStartupAsync(CancellationToken cancellationToken) =>
            Task.FromException(new InvalidOperationException("boom"));
    }
}
