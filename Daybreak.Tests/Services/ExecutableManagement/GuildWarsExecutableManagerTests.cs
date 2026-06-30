using Daybreak.Configuration.Options;
using Daybreak.Services.ExecutableManagement;
using Daybreak.Shared.Services.Options;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Daybreak.Tests.Services.ExecutableManagement;

[TestClass]
public sealed class GuildWarsExecutableManagerTests
{
    private readonly IOptionsMonitor<GuildwarsExecutableOptions> liveOptions = Substitute.For<IOptionsMonitor<GuildwarsExecutableOptions>>();
    private readonly IOptionsProvider optionsProvider = Substitute.For<IOptionsProvider>();
    private readonly GuildwarsExecutableOptions options = new();
    private readonly GuildWarsExecutableManager manager;
    private string tempDir = string.Empty;

    public GuildWarsExecutableManagerTests()
    {
        this.liveOptions.CurrentValue.Returns(this.options);
        this.manager = new GuildWarsExecutableManager(
            this.optionsProvider, this.liveOptions, NullLogger<GuildWarsExecutableManager>.Instance);
    }

    [TestInitialize]
    public void Setup()
    {
        this.tempDir = Path.Combine(Path.GetTempPath(), "db-exe-tests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(this.tempDir);
    }

    [TestCleanup]
    public void Cleanup()
    {
        try
        {
            if (Directory.Exists(this.tempDir))
            {
                Directory.Delete(this.tempDir, recursive: true);
            }
        }
        catch
        {
            // Best effort cleanup.
        }
    }

    private string CreateValidExecutable(string name)
    {
        var path = Path.Combine(this.tempDir, name);
        File.WriteAllText(path, "stub");
        return path;
    }

    // A path under a directory that does not exist - mirrors a file on an unmounted/removed
    // volume or a genuinely deleted executable (indistinguishable by path alone).
    private static string MissingExecutable(string name)
        => Path.Combine(Path.GetTempPath(), "db-missing-" + Guid.NewGuid().ToString("N"), name);

    [TestMethod]
    public void AddExecutable_InsertsNewPathAtFront()
    {
        var existing = this.CreateValidExecutable("Gw1.exe");
        var added = this.CreateValidExecutable("Gw2.exe");
        this.options.ExecutablePaths.Add(existing);

        this.manager.AddExecutable(added);

        this.options.ExecutablePaths.Should().Equal(added, existing);
        this.optionsProvider.Received().SaveOption(this.options);
    }

    [TestMethod]
    public void AddExecutable_DoesNotInsertDuplicate()
    {
        var exe = this.CreateValidExecutable("Gw.exe");
        this.options.ExecutablePaths.Add(exe);

        this.manager.AddExecutable(exe);

        this.options.ExecutablePaths.Should().ContainSingle().Which.Should().Be(exe);
    }

    [TestMethod]
    public void AddExecutable_UnderCapacity_DoesNotEvictInvalidEntries()
    {
        // An invalid entry (e.g. an unmounted drive) must survive while under the cap.
        var unavailable = MissingExecutable("Gw.exe");
        this.options.ExecutablePaths.Add(unavailable);
        var added = this.CreateValidExecutable("Gw2.exe");

        this.manager.AddExecutable(added);

        this.options.ExecutablePaths.Should().Contain(unavailable);
        this.options.ExecutablePaths.Should().Contain(added);
    }

    [TestMethod]
    public void AddExecutable_OverCapacity_EvictsInvalidAndKeepsValid()
    {
        // (cap - 1) valid + 1 invalid == cap stored entries, then add one more valid -> over cap by 1.
        var valids = new List<string>();
        for (var i = 0; i < GuildWarsExecutableManager.MaxExecutables - 1; i++)
        {
            valids.Add(this.CreateValidExecutable($"valid{i}.exe"));
        }

        var stale = MissingExecutable("stale.exe");
        this.options.ExecutablePaths.AddRange(valids);
        this.options.ExecutablePaths.Add(stale);

        var added = this.CreateValidExecutable("new.exe");
        this.manager.AddExecutable(added);

        this.options.ExecutablePaths.Should().HaveCount(GuildWarsExecutableManager.MaxExecutables);
        this.options.ExecutablePaths.Should().NotContain(stale);
        this.options.ExecutablePaths.Should().Contain(added);
        this.options.ExecutablePaths.Should().Contain(valids);
    }

    [TestMethod]
    public void AddExecutable_OverCapacityAllValid_DoesNotEvictValidEntries()
    {
        var valids = new List<string>();
        for (var i = 0; i < GuildWarsExecutableManager.MaxExecutables; i++)
        {
            valids.Add(this.CreateValidExecutable($"valid{i}.exe"));
        }

        this.options.ExecutablePaths.AddRange(valids);

        var added = this.CreateValidExecutable("new.exe");
        this.manager.AddExecutable(added);

        // Valid executables are never evicted, even when that keeps the list above the cap.
        this.options.ExecutablePaths.Should().HaveCount(GuildWarsExecutableManager.MaxExecutables + 1);
        this.options.ExecutablePaths.Should().Contain(added);
        this.options.ExecutablePaths.Should().Contain(valids);
    }

    [TestMethod]
    public void AddExecutable_OverCapacity_EvictsOldestInvalidFirstAndOnlyAsNeeded()
    {
        // (cap - 2) valid + 2 invalid == cap, then add one more -> over by 1 -> evict only the oldest invalid.
        var valids = new List<string>();
        for (var i = 0; i < GuildWarsExecutableManager.MaxExecutables - 2; i++)
        {
            valids.Add(this.CreateValidExecutable($"valid{i}.exe"));
        }

        var newerStale = MissingExecutable("newer-stale.exe");
        var olderStale = MissingExecutable("older-stale.exe");
        this.options.ExecutablePaths.AddRange(valids);
        this.options.ExecutablePaths.Add(newerStale);
        this.options.ExecutablePaths.Add(olderStale); // appended last -> oldest (highest index)

        var added = this.CreateValidExecutable("new.exe");
        this.manager.AddExecutable(added);

        this.options.ExecutablePaths.Should().HaveCount(GuildWarsExecutableManager.MaxExecutables);
        this.options.ExecutablePaths.Should().NotContain(olderStale);
        this.options.ExecutablePaths.Should().Contain(newerStale);
        this.options.ExecutablePaths.Should().Contain(added);
    }

    [TestMethod]
    public void GetExecutableList_ReturnsOnlyValidExecutables()
    {
        var valid = this.CreateValidExecutable("Gw.exe");
        var missing = MissingExecutable("Gw.exe");
        this.options.ExecutablePaths.Add(valid);
        this.options.ExecutablePaths.Add(missing);

        var list = this.manager.GetExecutableList().ToList();

        list.Should().ContainSingle().Which.Should().Be(valid);
    }

    [TestMethod]
    public void GetExecutableList_DoesNotRemoveInvalidEntriesFromStorage()
    {
        // Listing must be non-destructive: a temporarily unavailable executable stays persisted.
        var missing = MissingExecutable("Gw.exe");
        this.options.ExecutablePaths.Add(missing);

        this.manager.GetExecutableList();

        this.options.ExecutablePaths.Should().Contain(missing);
        this.optionsProvider.DidNotReceive().SaveOption(Arg.Any<GuildwarsExecutableOptions>());
    }

    [TestMethod]
    public void IsValidExecutable_TrueForExistingFile_FalseForMissingFile()
    {
        var valid = this.CreateValidExecutable("Gw.exe");

        this.manager.IsValidExecutable(valid).Should().BeTrue();
        this.manager.IsValidExecutable(MissingExecutable("Gw.exe")).Should().BeFalse();
    }

    [TestMethod]
    public void RemoveExecutable_RemovesEntryAndSaves()
    {
        var exe = this.CreateValidExecutable("Gw.exe");
        this.options.ExecutablePaths.Add(exe);

        this.manager.RemoveExecutable(exe);

        this.options.ExecutablePaths.Should().NotContain(exe);
        this.optionsProvider.Received().SaveOption(this.options);
    }
}
