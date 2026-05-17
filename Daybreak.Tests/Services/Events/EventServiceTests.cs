using Daybreak.Services.Events;
using Daybreak.Shared.Models.Guildwars;
using FluentAssertions;

namespace Daybreak.Tests.Services.Events;

[TestClass]
public sealed class EventServiceTests
{
    private readonly EventService service = new();

    [TestMethod]
    public void GetActiveEvents_DuringHalloween_ReturnsHalloween()
    {
        // Halloween: 10/18 19:00 UTC → 11/2 19:00 UTC
        var midHalloween = new DateTime(2024, 10, 25, 12, 0, 0, DateTimeKind.Utc);

        var active = this.service.GetActiveEvents(midHalloween);

        active.Should().Contain(e => e.Title == "Halloween");
    }

    [TestMethod]
    public void GetActiveEvents_BeforeEventStart_DoesNotIncludeEvent()
    {
        // Halloween starts at 10/18 19:00 UTC; 10/18 18:59 UTC is still before.
        var justBefore = new DateTime(2024, 10, 18, 18, 59, 0, DateTimeKind.Utc);

        var active = this.service.GetActiveEvents(justBefore);

        active.Should().NotContain(e => e.Title == "Halloween");
    }

    [TestMethod]
    public void GetActiveEvents_AtEventStartTime_IncludesEvent()
    {
        // Exactly at 10/18 19:00 UTC the event becomes active.
        var atStart = new DateTime(2024, 10, 18, 19, 0, 0, DateTimeKind.Utc);

        var active = this.service.GetActiveEvents(atStart);

        active.Should().Contain(e => e.Title == "Halloween");
    }

    [TestMethod]
    public void GetActiveEvents_AfterEventEnd_DoesNotIncludeEvent()
    {
        // Halloween ends 11/2 19:00 UTC; one minute later it's gone.
        var afterEnd = new DateTime(2024, 11, 2, 19, 1, 0, DateTimeKind.Utc);

        var active = this.service.GetActiveEvents(afterEnd);

        active.Should().NotContain(e => e.Title == "Halloween");
    }

    [TestMethod]
    public void GetActiveEvents_OverlappingWindow_ReturnsAllOverlapping()
    {
        // 11/1 — Breast Cancer Awareness Month (10/1-11/2) AND Halloween (10/18-11/2) overlap.
        var overlap = new DateTime(2024, 11, 1, 12, 0, 0, DateTimeKind.Utc);

        var active = this.service.GetActiveEvents(overlap);

        active.Select(e => e.Title).Should().Contain(["Halloween", "Breast Cancer Awareness Month"]);
    }

    [TestMethod]
    public void GetActiveEvents_QuietPeriod_ReturnsEmpty()
    {
        // Mid-February — between Canthan New Year (ends 2/7) and Lucky Treats Week (3/14).
        var quiet = new DateTime(2024, 2, 20, 12, 0, 0, DateTimeKind.Utc);

        var active = this.service.GetActiveEvents(quiet);

        active.Should().BeEmpty();
    }

    [TestMethod]
    public void GetEventStartTime_PastEventThisYear_RollsForwardOneYear()
    {
        // If today is past the event's From date this calendar year, GetEventStartTime
        // must reflect *next* year's occurrence.
        var halloween = Event.Halloween;

        var startTime = this.service.GetEventStartTime(halloween);

        startTime.Should().BeAfter(DateTime.UtcNow);
        startTime.Month.Should().Be(halloween.From.Month);
        startTime.Day.Should().Be(halloween.From.Day);
        startTime.Hour.Should().Be(19);
        startTime.Kind.Should().Be(DateTimeKind.Utc);
    }

    [TestMethod]
    public void GetUpcomingEvent_AlwaysReturnsAnEvent()
    {
        var upcoming = this.service.GetUpcomingEvent();

        upcoming.Should().NotBeNull();
        Event.Events.Should().Contain(upcoming);
    }

    [TestMethod]
    public void GetLocalizedEventStartTime_ConvertsFrom19UtcToLocal()
    {
        var today = DateTime.UtcNow.Date;
        var expected = TimeOnly.FromDateTime(
            new DateTime(today.Year, today.Month, today.Day, 19, 0, 0, DateTimeKind.Utc).ToLocalTime());

        var localized = this.service.GetLocalizedEventStartTime();

        localized.Should().Be(expected);
    }
}
