using Daybreak.Services.Notifications;
using Daybreak.Services.Notifications.Handlers;
using Daybreak.Services.Notifications.Models;
using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Daybreak.Shared.Services.Notifications;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Daybreak.Tests.Services.Notifications;

[TestClass]
public sealed class NotificationServiceTests
{
    private readonly INotificationStorage storage = Substitute.For<INotificationStorage>();
    private readonly IServiceProvider serviceProvider = Substitute.For<IServiceProvider>();
    private readonly TaskCompletionSource<NotificationDTO> storedDto = new();
    private readonly NotificationService service;

    public NotificationServiceTests()
    {
        this.storage.When(s => s.StoreNotification(Arg.Any<NotificationDTO>(), Arg.Any<CancellationToken>()))
            .Do(call => this.storedDto.TrySetResult(call.Arg<NotificationDTO>()));

        this.service = new NotificationService(
            this.serviceProvider,
            this.storage,
            Substitute.For<ILogger<NotificationService>>());
    }

    [TestMethod]
    public async Task NotifyInformation_Persistent_StoresNotificationWithInformationLevel()
    {
        var token = this.service.NotifyInformation("title", "desc", metaData: "meta");

        var stored = await this.storedDto.Task.WaitAsync(TimeSpan.FromSeconds(2));
        stored.Title.Should().Be("title");
        stored.Description.Should().Be("desc");
        stored.MetaData.Should().Be("meta");
        stored.Level.Should().Be((int)LogLevel.Information);
        stored.HandlerType.Should().Contain(nameof(NoActionHandler));
        token.Closed.Should().BeFalse();
    }

    [TestMethod]
    public async Task NotifyError_WithHandlingType_StoresNotificationWithErrorLevelAndHandler()
    {
        this.service.NotifyError<TestHandler>("err", "boom");

        var stored = await this.storedDto.Task.WaitAsync(TimeSpan.FromSeconds(2));
        stored.Level.Should().Be((int)LogLevel.Error);
        stored.HandlerType.Should().Contain(nameof(TestHandler));
    }

    [TestMethod]
    public void NotifyInformation_NotPersistent_DoesNotCallStorage()
    {
        this.service.NotifyInformation("t", "d", persistent: false);

        this.storage.DidNotReceiveWithAnyArgs().StoreNotification(default!, default);
    }

    [TestMethod]
    public async Task OpenNotification_WithHandlingType_ResolvesAndInvokesHandler()
    {
        var handler = new TestHandler();
        this.serviceProvider.GetService(typeof(IEnumerable<INotificationHandler>))
            .Returns(new INotificationHandler[] { handler });
        var notification = (Notification)Activator.CreateInstance(
            typeof(Notification),
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
            binder: null, args: null, culture: null)!;
        typeof(Notification).GetProperty(nameof(Notification.Title))!.SetValue(notification, "t");
        typeof(Notification).GetProperty(nameof(Notification.HandlingType))!.SetValue(notification, typeof(TestHandler));

        await ((INotificationProducer)this.service).OpenNotification(notification, storeNotification: false);

        handler.OpenedWith.Should().BeSameAs(notification);
    }

    [TestMethod]
    public async Task GetAllNotifications_DelegatesToStorageAndMapsDtos()
    {
        var dto = new NotificationDTO
        {
            Id = "abc",
            Title = "t",
            Description = "d",
            MetaData = "meta",
            Level = (int)LogLevel.Warning,
            CreationTime = DateTime.UtcNow.ToBinary(),
            ExpirationTime = DateTime.UtcNow.AddMinutes(1).ToBinary(),
            HandlerType = typeof(NoActionHandler).AssemblyQualifiedName,
            Dismissible = true,
            Closed = false,
        };
        this.storage.GetNotifications(Arg.Any<CancellationToken>()).Returns(new[] { dto });

        var result = (await ((INotificationProducer)this.service).GetAllNotifications(default)).ToList();

        result.Should().ContainSingle();
        var n = result[0];
        n.Id.Should().Be("abc");
        n.Title.Should().Be("t");
        n.Description.Should().Be("d");
        n.Metadata.Should().Be("meta");
        n.Level.Should().Be(LogLevel.Warning);
        n.HandlingType.Should().Be(typeof(NoActionHandler));
        n.Dismissible.Should().BeTrue();
    }

    [TestMethod]
    public async Task RemoveAllNotifications_DelegatesToStorage()
    {
        await ((INotificationProducer)this.service).RemoveAllNotifications(default);

        await this.storage.Received(1).RemoveAllNotifications(Arg.Any<CancellationToken>());
    }

    private sealed class TestHandler : INotificationHandler
    {
        public Notification? OpenedWith { get; private set; }
        public void OpenNotification(Notification notification) => this.OpenedWith = notification;
    }
}
