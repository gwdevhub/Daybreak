using Daybreak.Shared.Utils;
using FluentAssertions;

namespace Daybreak.Tests.Utils;

[TestClass]
public sealed class ComputerNameUtilsTests
{
    [TestMethod]
    [DataRow("zephyrs-cachyos-x8664", "ZEPHYRS-CACHYOS")]
    [DataRow("bazzite-gaming-rig-01", "BAZZITE-GAMING")]
    [DataRow("alex-desktop-machine", "ALEX-DESKTOP-MA")]
    public void SanitizeComputerName_TooLong_TruncatesToMaxLength(string input, string expected)
    {
        var result = ComputerNameUtils.SanitizeComputerName(input);

        result.Should().Be(expected);
        result.Length.Should().BeLessThanOrEqualTo(ComputerNameUtils.MaxComputerNameLength);
    }

    [TestMethod]
    [DataRow("cachyos-x8664", "CACHYOS-X8664")]
    [DataRow("steamdeck", "STEAMDECK")]
    [DataRow("ZEPHYRS-CACHYOS", "ZEPHYRS-CACHYOS")]
    public void SanitizeComputerName_ValidName_UpperCasesAndPreserves(string input, string expected)
    {
        ComputerNameUtils.SanitizeComputerName(input).Should().Be(expected);
    }

    [TestMethod]
    [DataRow("my.host.local", "MYHOSTLOCAL")]
    [DataRow("hôtel de ville", "HTELDEVILLE")]
    [DataRow("machine!@#$name", "MACHINENAME")]
    public void SanitizeComputerName_InvalidCharacters_AreRemoved(string input, string expected)
    {
        ComputerNameUtils.SanitizeComputerName(input).Should().Be(expected);
    }

    [TestMethod]
    [DataRow("--host--", "HOST")]
    [DataRow("_host_", "HOST")]
    public void SanitizeComputerName_LeadingOrTrailingSeparators_AreTrimmed(string input, string expected)
    {
        ComputerNameUtils.SanitizeComputerName(input).Should().Be(expected);
    }

    [TestMethod]
    public void SanitizeComputerName_TruncationEndingOnSeparator_TrimsSeparator()
    {
        // 'THIS-IS-A-LONG' would be followed by '-' at index 15.
        ComputerNameUtils.SanitizeComputerName("this-is-a-long--name").Should().Be("THIS-IS-A-LONG");
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("...")]
    [DataRow("---")]
    public void SanitizeComputerName_NoUsableCharacters_ReturnsFallback(string? input)
    {
        ComputerNameUtils.SanitizeComputerName(input).Should().Be(ComputerNameUtils.FallbackComputerName);
    }

    [TestMethod]
    public void SanitizeComputerName_FallbackIsItselfValid()
    {
        ComputerNameUtils.FallbackComputerName.Length
            .Should().BeLessThanOrEqualTo(ComputerNameUtils.MaxComputerNameLength);
        ComputerNameUtils.SanitizeComputerName(ComputerNameUtils.FallbackComputerName)
            .Should().Be(ComputerNameUtils.FallbackComputerName);
    }

    [TestMethod]
    public void SanitizeComputerName_CurrentMachineName_IsAlwaysValid()
    {
        var result = ComputerNameUtils.SanitizeComputerName(Environment.MachineName);

        result.Should().NotBeNullOrEmpty();
        result.Length.Should().BeLessThanOrEqualTo(ComputerNameUtils.MaxComputerNameLength);
    }
}
