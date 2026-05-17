using Daybreak.Services.Notifications;
using Daybreak.Services.Notifications.Models;
using FluentAssertions;

namespace Daybreak.Tests.Services.Notifications;

[TestClass]
public sealed class InMemoryNotificationStorageTests
{
    private readonly InMemoryNotificationStorage storage = new();

    private static NotificationDTO MakeDTO(string id, bool closed = false) => new()
    {
        Id = id,
        Title = $"T-{id}",
        Description = $"D-{id}",
        CreationTime = DateTime.UtcNow.ToBinary(),
        ExpirationTime = DateTime.UtcNow.AddMinutes(1).ToBinary(),
        Closed = closed
    };

    [TestMethod]
    public async Task StoreNotification_ThenGetNotifications_ReturnsStoredItems()
    {
        var n1 = MakeDTO("1");
        var n2 = MakeDTO("2");

        await this.storage.StoreNotification(n1, default);
        await this.storage.StoreNotification(n2, default);

        var all = await this.storage.GetNotifications(default);

        all.Select(n => n.Id).Should().BeEquivalentTo(["1", "2"]);
    }

    [TestMethod]
    public async Task GetPendingNotifications_FiltersClosedOnes()
    {
        var open = MakeDTO("open");
        var closed = MakeDTO("closed", closed: true);
        await this.storage.StoreNotification(open, default);
        await this.storage.StoreNotification(closed, default);

        var pending = await this.storage.GetPendingNotifications(default);

        pending.Select(n => n.Id).Should().ContainSingle().Which.Should().Be("open");
    }

    [TestMethod]
    public async Task OpenNotification_MarksClosed()
    {
        var dto = MakeDTO("1");

        await this.storage.OpenNotification(dto, default);

        dto.Closed.Should().BeTrue();
    }

    [TestMethod]
    public async Task RemoveNotification_RemovesByReference()
    {
        var dto = MakeDTO("1");
        await this.storage.StoreNotification(dto, default);

        await this.storage.RemoveNotification(dto, default);

        (await this.storage.GetNotifications(default)).Should().BeEmpty();
    }

    [TestMethod]
    public async Task RemoveAllNotifications_ClearsStorage()
    {
        await this.storage.StoreNotification(MakeDTO("1"), default);
        await this.storage.StoreNotification(MakeDTO("2"), default);

        await this.storage.RemoveAllNotifications(default);

        (await this.storage.GetNotifications(default)).Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetNotifications_ReturnsSnapshot_NotLiveCollection()
    {
        await this.storage.StoreNotification(MakeDTO("1"), default);
        var snapshot = await this.storage.GetNotifications(default);

        await this.storage.StoreNotification(MakeDTO("2"), default);

        snapshot.Should().HaveCount(1);
    }
}
