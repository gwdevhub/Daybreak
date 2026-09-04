using Daybreak.Shared.Utils;
using FluentAssertions;

namespace Daybreak.Tests.Utils;

[TestClass]
public sealed class CommandLineUtilsTests
{
    [TestMethod]
    public void SplitCommandLine_LaunchArguments_PreservesShellMetacharacters()
    {
        var commandLine =
            "\"Z:\\Injector.exe\" launch True \"Z:\\Games\\Guild Wars\\Gw.exe\" " +
            "-email \"user@example.com\" -password \"-MyP$SS\" -character \"Daybreak\"";

        CommandLineUtils.SplitCommandLine(commandLine).Should().Equal(
            "Z:\\Injector.exe",
            "launch",
            "True",
            "Z:\\Games\\Guild Wars\\Gw.exe",
            "-email",
            "user@example.com",
            "-password",
            "-MyP$SS",
            "-character",
            "Daybreak");
    }

    [TestMethod]
    [DataRow("-MyP$SS")]
    [DataRow("pass`id`word")]
    [DataRow("pass$(id -u)word")]
    [DataRow("pass*word")]
    [DataRow("pass word")]
    [DataRow("~pass;word&more|x")]
    [DataRow("$HOME")]
    public void SplitCommandLine_QuotedValue_IsReturnedVerbatim(string value)
    {
        CommandLineUtils.SplitCommandLine($"-password \"{value}\"").Should().Equal("-password", value);
    }

    [TestMethod]
    public void SplitCommandLine_QuotedPathWithSpaces_StaysOneArgument()
    {
        CommandLineUtils.SplitCommandLine("\"Z:\\Games\\Guild Wars\\Gw.exe\"")
            .Should().Equal("Z:\\Games\\Guild Wars\\Gw.exe");
    }

    [TestMethod]
    public void SplitCommandLine_WindowsPathTrailingBackslashes_AreLiteral()
    {
        CommandLineUtils.SplitCommandLine(@"C:\dir\ next").Should().Equal(@"C:\dir\", "next");
    }

    [TestMethod]
    public void SplitCommandLine_EscapedQuote_IsLiteral()
    {
        CommandLineUtils.SplitCommandLine(@"a\""b").Should().Equal("a\"b");
    }

    [TestMethod]
    public void SplitCommandLine_DoubledQuoteInsideQuotes_IsLiteral()
    {
        CommandLineUtils.SplitCommandLine("\"a\"\"b\"").Should().Equal("a\"b");
    }

    [TestMethod]
    public void SplitCommandLine_RepeatedAndTabWhitespace_IsCollapsed()
    {
        CommandLineUtils.SplitCommandLine("a  \t b").Should().Equal("a", "b");
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void SplitCommandLine_NoArguments_ReturnsEmpty(string? commandLine)
    {
        CommandLineUtils.SplitCommandLine(commandLine).Should().BeEmpty();
    }
}
