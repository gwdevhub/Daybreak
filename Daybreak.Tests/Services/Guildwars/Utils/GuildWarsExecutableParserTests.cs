using Daybreak.Services.Guildwars.Utils;
using FluentAssertions;

namespace Daybreak.Tests.Services.Guildwars.Utils;

/// <summary>
/// Fixtures are real Gw.exe binaries committed under
/// <c>Daybreak.Tests/Fixtures/Gw/</c> and copied to the test output directory
/// at build time. Each binary is named <c>Gw.&lt;version&gt;.exe</c> where
/// <c>&lt;version&gt;</c> is the value the parser is expected to extract.
/// To point the tests at a different directory (e.g. a local cache outside the
/// repo), set <c>DAYBREAK_TEST_GW_FIXTURES</c>. Missing fixtures are reported
/// <c>Inconclusive</c> rather than failing. Future game updates add new
/// fixtures and a matching <c>DataRow</c>.
/// </summary>
[TestClass]
public sealed class GuildWarsExecutableParserTests
{
    private const string LegacyFixture = "Gw.381488.exe";
    private const int LegacyExpectedVersion = 381488;

    private const string ReforgedFixture1 = "Gw.387585.exe";
    private const int Reforged1ExpectedFileId = 387585;

    private const string ReforgedFixture2 = "Gw.388118.exe";
    private const int Reforged2ExpectedFileId = 388118;

    private static string RequireFixture(string fileName)
    {
        // Default: <test-output>/Fixtures/Gw/<fileName> (committed in the repo and
        // copied to bin/ via CopyToOutputDirectory). Override with
        // DAYBREAK_TEST_GW_FIXTURES if you want to point at a different directory.
        var dir = Environment.GetEnvironmentVariable("DAYBREAK_TEST_GW_FIXTURES")
            ?? Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gw");

        var path = Path.Combine(dir, fileName);
        if (!File.Exists(path))
        {
            Assert.Inconclusive($"Fixture '{fileName}' not found under '{dir}'.");
        }

        return path;
    }

    [TestMethod]
    public void TryParse_MissingFile_ReturnsNull()
    {
        GuildWarsExecutableParser.TryParse(Path.Combine(Path.GetTempPath(), $"definitely-not-here-{Guid.NewGuid():N}.exe"))
            .Should().BeNull();
    }

    [TestMethod]
    public void TryParse_NotAPeFile_ReturnsNull()
    {
        var path = Path.Combine(Path.GetTempPath(), $"daybreak-tests-{Guid.NewGuid():N}.bin");
        File.WriteAllBytes(path, [0x00, 0x01, 0x02, 0x03, 0x04, 0x05]);
        try
        {
            GuildWarsExecutableParser.TryParse(path).Should().BeNull();
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    [DataRow(ReforgedFixture1, Reforged1ExpectedFileId)]
    [DataRow(ReforgedFixture2, Reforged2ExpectedFileId)]
    public async Task GetFileId_ReforgedBuild_ReturnsExpectedFileId(string fileName, int expectedFileId)
    {
        var parser = GuildWarsExecutableParser.TryParse(RequireFixture(fileName));
        parser.Should().NotBeNull();

        var fileId = await parser!.GetFileId(CancellationToken.None);

        fileId.Should().Be(expectedFileId);
    }

    [TestMethod]
    [DataRow(ReforgedFixture1)]
    [DataRow(ReforgedFixture2)]
    public async Task GetVersionLegacy_ReforgedBuild_ThrowsBecauseLegacyPatternIsAbsent(string fileName)
    {
        var parser = GuildWarsExecutableParser.TryParse(RequireFixture(fileName));
        parser.Should().NotBeNull();

        var act = async () => await parser!.GetVersionLegacy(CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();
    }

    [TestMethod]
    public async Task GetVersionLegacy_LegacyExe_ReturnsExpectedVersion()
    {
        var parser = GuildWarsExecutableParser.TryParse(RequireFixture(LegacyFixture));
        parser.Should().NotBeNull();

        var version = await parser!.GetVersionLegacy(CancellationToken.None);

        version.Should().Be(LegacyExpectedVersion);
    }

    [TestMethod]
    public async Task GetFileId_LegacyExe_ThrowsBecauseReforgedPatternIsAbsent()
    {
        var parser = GuildWarsExecutableParser.TryParse(RequireFixture(LegacyFixture));
        parser.Should().NotBeNull();

        var act = async () => await parser!.GetFileId(CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();
    }
}
