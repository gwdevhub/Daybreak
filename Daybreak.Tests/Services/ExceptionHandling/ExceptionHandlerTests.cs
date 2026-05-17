using Daybreak.Services.ExceptionHandling;
using Daybreak.Services.Notifications.Handlers;
using Daybreak.Shared.Exceptions;
using Daybreak.Shared.Services.ExceptionHandling;
using Daybreak.Shared.Services.Notifications;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Daybreak.Tests.Services.ExceptionHandling;

[TestClass]
public sealed class ExceptionHandlerTests
{
    private readonly ICrashDumpService crashDumpService = Substitute.For<ICrashDumpService>();
    private readonly INotificationService notificationService = Substitute.For<INotificationService>();
    private readonly ExceptionHandler handler;

    public ExceptionHandlerTests()
    {
        this.handler = new ExceptionHandler(
            this.crashDumpService,
            this.notificationService,
            Substitute.For<ILogger<ExceptionHandler>>());
    }

    [TestMethod]
    public void HandleException_TaskCanceledException_IsHandledAndNoNotification()
    {
        var handled = this.handler.HandleException(new TaskCanceledException());

        handled.Should().BeTrue();
        this.notificationService.DidNotReceiveWithAnyArgs().NotifyError<MessageBoxHandler>(default!, default!);
    }

    [TestMethod]
    public void HandleException_OperationCanceledException_IsHandled()
    {
        var handled = this.handler.HandleException(new OperationCanceledException());

        handled.Should().BeTrue();
        this.notificationService.DidNotReceiveWithAnyArgs().NotifyError<MessageBoxHandler>(default!, default!);
    }

    [TestMethod]
    public void HandleException_AggregateOfOperationCancelled_IsHandled()
    {
        var aggregate = new AggregateException(new OperationCanceledException());

        var handled = this.handler.HandleException(aggregate);

        handled.Should().BeTrue();
        this.notificationService.DidNotReceiveWithAnyArgs().NotifyError<MessageBoxHandler>(default!, default!);
    }

    [TestMethod]
    public void HandleException_GuildwarsProcessIdArgumentException_IsHandled()
    {
        var ex = new ArgumentException("Process with an Id of 12345 was not found.");

        var handled = this.handler.HandleException(ex);

        handled.Should().BeTrue();
        this.notificationService.DidNotReceiveWithAnyArgs().NotifyError<MessageBoxHandler>(default!, default!);
    }

    [TestMethod]
    public void HandleException_UnhandledException_NotifiesAndReturnsTrue()
    {
        var ex = new InvalidOperationException("boom");

        var handled = this.handler.HandleException(ex);

        handled.Should().BeTrue();
        this.notificationService.Received(1).NotifyError<MessageBoxHandler>(
            nameof(InvalidOperationException),
            Arg.Is<string>(s => s.Contains("boom")),
            Arg.Any<string?>(), Arg.Any<DateTime?>(), Arg.Any<bool>(), Arg.Any<bool>());
        this.crashDumpService.DidNotReceiveWithAnyArgs().WriteCrashDump(default!);
    }

    [TestMethod]
    public void HandleException_FatalException_WritesCrashDumpAndReturnsFalse()
    {
        // Allow crash file path to be writable in test output directory.
        try
        {
            var ex = new FatalException("fatal");

            var handled = this.handler.HandleException(ex);

            handled.Should().BeFalse();
            this.crashDumpService.ReceivedWithAnyArgs(1).WriteCrashDump(default!);
        }
        finally
        {
            var crashes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Crashes");
            if (Directory.Exists(crashes))
            {
                Directory.Delete(crashes, recursive: true);
            }
        }
    }
}
