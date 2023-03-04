using Daybreak.Models.Versioning;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Version = Daybreak.Models.Versioning.Version;

namespace Daybreak.Tests.Models;

[TestClass]
public class VersionTests
{
    [DataRow("v0.1.0.0.1", "v0.1.0.0.1")]
    [DataRow("v0.1.0.0.1.0.0.0.0", "v0.1.0.0.1")]
    [DataRow("v0.1.0", "v0.1")]
    [DataRow("0.1.0", "0.1")]
    [DataRow("v0.1.0.0.0", "v0.1")]
    [DataRow("0.1.0.0.0", "0.1")]
    [DataRow("v0", "v0")]
    [DataRow("0", "0")]
    [DataRow("v0.1", "v0.1")]
    [DataRow("0.1-release", "0.1-release")]
    [DataRow("v0.1-release", "v0.1-release")]
    [TestMethod]
    public void VersionToString(string version, string expectedString)
    {
        var parsedVersion = Version.Parse(version);
        parsedVersion.ToString().Should().Be(expectedString);
    }

    [DataRow("v0", false)]
    [DataRow("0", false)]
    [DataRow("v0.1", false)]
    [DataRow("0.1-release", true)]
    [DataRow("v0.1-release", true)]
    [TestMethod]
    public void VersionHasStringSuffix(string version, bool hasSuffix)
    {
        var parsedVersion = Version.Parse(version);
        if (hasSuffix)
        {
            parsedVersion.VersionTokens.Last().Should().BeOfType<VersionStringToken>();
        }
        else
        {
            parsedVersion.VersionTokens.Last().Should().BeOfType<VersionNumberToken>();
        }
    }

    [DataRow("v0", true)]
    [DataRow("0", false)]
    [DataRow("v0.1", true)]
    [DataRow("0.1", false)]
    [TestMethod]
    public void VersionTokensHasPrefix(string version, bool hasPrefix)
    {
        var parsedVersion = Version.Parse(version);
        parsedVersion.HasPrefix.Should().Be(hasPrefix);
    }

    [DataRow(null, false)]
    [DataRow("", false)]
    [DataRow("asd", false)]
    [DataRow("ads.pls.er", false)]
    [DataRow("0.0.1", true)]
    [DataRow("1", true)]
    [DataRow("0.1.asd", false)]
    [DataRow("0.", false)]
    [DataRow(".1", false)]
    [DataRow("0.1-", false)]
    [DataRow("0.1-release", true)]
    [DataRow("0..1", false)]
    [TestMethod]
    public void TryParseVersionTest(string version, bool result)
    {
        var success = Version.TryParse(version, out var parsedVersion);
        success.Should().Be(result);
        if (success is true)
        {
            parsedVersion.Should().NotBeNull();
        }
    }

    [DataRow(null, false)]
    [DataRow("", false)]
    [DataRow("asd", false)]
    [DataRow("ads.pls.er", false)]
    [DataRow("0.0.1", true)]
    [DataRow("1", true)]
    [DataRow("0.1.asd", false)]
    [DataRow("0.", false)]
    [DataRow(".1", false)]
    [DataRow("0.1-", false)]
    [DataRow("0.1-release", true)]
    [DataRow("0..1", false)]
    [TestMethod]
    public void ParseVersionTest(string version, bool shouldNotThrow)
    {
        var action = new Action(() => Version.Parse(version));
        if (shouldNotThrow is false)
        {
            action.Should().Throw<InvalidOperationException>();
        }
        else
        {
            action.Should().NotThrow<InvalidOperationException>();
        }
    }

    [DataRow(null, false)]
    [DataRow("", false)]
    [DataRow("asd", false)]
    [DataRow("ads.pls.er", false)]
    [DataRow("0.0.1", true)]
    [DataRow("1", true)]
    [DataRow("0.1.asd", false)]
    [DataRow("0.", false)]
    [DataRow(".1", false)]
    [DataRow("0.1-", false)]
    [DataRow("0.1-release", true)]
    [DataRow("0..1", false)]
    [TestMethod]
    public void VersionConstructorTest(string version, bool shouldNotThrow)
    {
        var action = new Action(() => new Version(version));
        if (shouldNotThrow is false)
        {
            action.Should().Throw<ArgumentException>();
        }
        else
        {
            action.Should().NotThrow<ArgumentException>();
        }
    }

    [DataRow("v0.1.0.0.1", "v0.1.0.0.1", true)]
    [DataRow("v0.1.0.0.1.0.0.0.0", "v0.1.0.0.1", true)]
    [DataRow("v0.1.0", "v0.1", true)]
    [DataRow("0.1.0", "0.1", true)]
    [DataRow("v0.1.0.0.0", "v0.1", true)]
    [DataRow("0.1.0.0.0", "0.1", true)]
    [DataRow("v0", "v0", true)]
    [DataRow("0", "0", true)]
    [DataRow("v0.1", "v0.1", true)]
    [DataRow("0.1-release", "0.1-release", true)]
    [DataRow("v0.1-release", "v0.1-release", true)]
    [DataRow("v0.1.0.0", "v0.1.0.0.1", false)]
    [DataRow("v0.1.0.0.1", "v0.1.0.0", false)]
    [DataRow("v0.9.8.9", "v0.9.8.10", false)]
    [DataRow("0.1-release", "0.2-release", false)]
    [DataRow("v1.1-release", "v0.1-release", false)]
    [TestMethod]
    public void Equals(string version, string version2, bool result)
    {
        var parsedVersion1 = Version.Parse(version);
        var parsedVersion2 = Version.Parse(version2);

        parsedVersion1.Equals(parsedVersion2).Should().Be(result);
    }

    [DataRow("v0.1.0.0.1", "v0.1.0.0.1", 0)]
    [DataRow("v0.1.0.0.1.0.0.0.0", "v0.1.0.0.1", 0)]
    [DataRow("v0.1.0", "v0.1", 0)]
    [DataRow("0.1.0", "0.1", 0)]
    [DataRow("v0.1.0.0.0", "v0.1", 0)]
    [DataRow("0.1.0.0.0", "0.1", 0)]
    [DataRow("v0", "v0", 0)]
    [DataRow("0", "0", 0)]
    [DataRow("v0.1", "v0.1", 0)]
    [DataRow("0.1-release", "0.1-release", 0)]
    [DataRow("v0.1-release", "v0.1-release", 0)]
    [DataRow("v0.1.0.0", "v0.1.0.0.1", -1)]
    [DataRow("v0.1.0.0.1", "v0.1.0.0", 1)]
    [DataRow("v0.9.8.9", "v0.9.8.10", -1)]
    [DataRow("v0.9.8.10", "v0.9.8.9", 1)]
    [DataRow("0.1-release", "0.2-release", -1)]
    [DataRow("v1.1-release", "v0.1-release", 1)]
    [TestMethod]
    public void CompareTo(string version, string version2, int comparisonResult)
    {
        var parsedVersion1 = Version.Parse(version);
        var parsedVersion2 = Version.Parse(version2);

        parsedVersion1.CompareTo(parsedVersion2).Should().Be(comparisonResult);
    }
}
