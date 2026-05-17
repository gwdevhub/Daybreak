using Daybreak.Utils;
using FluentAssertions;

namespace Daybreak.Tests.Utils;

[TestClass]
public sealed class PeVersionReaderTests
{
    // Reuses the Gw.exe fixtures from GuildWarsExecutableParserTests
    // (committed under Daybreak.Tests/Fixtures/Gw/).
    private const string AnyFixture = "Gw.387585.exe";
    private const string ExpectedProductName = "Guild Wars";
    private const string ExpectedVersionString = "1, 0, 0, 1";

    private static string RequireFixture(string fileName)
    {
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
    public void GetProductVersion_MissingFile_ReturnsNull()
    {
        PeVersionReader.GetProductVersion(Path.Combine(Path.GetTempPath(), $"missing-{Guid.NewGuid():N}.exe"))
            .Should().BeNull();
    }

    [TestMethod]
    public void GetFileVersion_MissingFile_ReturnsNull()
    {
        PeVersionReader.GetFileVersion(Path.Combine(Path.GetTempPath(), $"missing-{Guid.NewGuid():N}.exe"))
            .Should().BeNull();
    }

    [TestMethod]
    public void GetProductName_MissingFile_ReturnsNull()
    {
        PeVersionReader.GetProductName(Path.Combine(Path.GetTempPath(), $"missing-{Guid.NewGuid():N}.exe"))
            .Should().BeNull();
    }

    [TestMethod]
    public void GetProductVersion_NotAPeFile_ReturnsNull()
    {
        var path = Path.Combine(Path.GetTempPath(), $"daybreak-tests-{Guid.NewGuid():N}.bin");
        File.WriteAllBytes(path, [0x00, 0x01, 0x02, 0x03]);
        try
        {
            PeVersionReader.GetProductVersion(path).Should().BeNull();
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public void GetProductVersion_GwExe_ReturnsExpectedVersionString()
    {
        PeVersionReader.GetProductVersion(RequireFixture(AnyFixture)).Should().Be(ExpectedVersionString);
    }

    [TestMethod]
    public void GetFileVersion_GwExe_ReturnsExpectedVersionString()
    {
        PeVersionReader.GetFileVersion(RequireFixture(AnyFixture)).Should().Be(ExpectedVersionString);
    }

    [TestMethod]
    public void GetProductName_GwExe_ReturnsGuildWars()
    {
        PeVersionReader.GetProductName(RequireFixture(AnyFixture)).Should().Be(ExpectedProductName);
    }
}
