using Daybreak.Services.Logging;
using FluentAssertions;
using Serilog.Events;
using Serilog.Parsing;

namespace Daybreak.Tests.Services.Logging;

[TestClass]
public sealed class StructuredLogFormatterTests
{
    private static LogEvent BuildEvent(
        LogEventLevel level = LogEventLevel.Information,
        string template = "hello",
        Exception? exception = null,
        IEnumerable<LogEventProperty>? properties = null,
        DateTimeOffset? timestamp = null)
    {
        return new LogEvent(
            timestamp ?? new DateTimeOffset(2024, 1, 2, 3, 4, 5, TimeSpan.Zero),
            level,
            exception,
            new MessageTemplateParser().Parse(template),
            properties ?? []);
    }

    [TestMethod]
    public void Format_LiteralTextOnly_EmitsSingleTextToken()
    {
        var formatter = new StructuredLogFormatter("literal");

        var tokens = formatter.Format(BuildEvent()).ToList();

        tokens.Should().ContainSingle();
        tokens[0].Type.Should().Be(StructuredLogFormatter.LogTokenType.Text);
        tokens[0].Value.Should().Be("literal");
        tokens[0].PropertyName.Should().BeNull();
    }

    [TestMethod]
    public void Format_NewLineProperty_EmitsEnvironmentNewLine()
    {
        var formatter = new StructuredLogFormatter("{NewLine}");

        var token = formatter.Format(BuildEvent()).Single();

        token.Type.Should().Be(StructuredLogFormatter.LogTokenType.NewLine);
        token.Value.Should().Be(Environment.NewLine);
        token.PropertyName.Should().Be("NewLine");
    }

    [TestMethod]
    public void Format_TimestampProperty_UsesDefaultFormatWhenNoneSpecified()
    {
        var formatter = new StructuredLogFormatter("{Timestamp}");

        var token = formatter.Format(BuildEvent()).Single();

        token.Type.Should().Be(StructuredLogFormatter.LogTokenType.Timestamp);
        token.Value.Should().Be("2024-01-02 03:04:05");
    }

    [TestMethod]
    public void Format_TimestampProperty_UsesExplicitFormat()
    {
        var formatter = new StructuredLogFormatter("{Timestamp:yyyy/MM/dd}");

        var token = formatter.Format(BuildEvent()).Single();

        token.Value.Should().Be("2024/01/02");
    }

    [TestMethod]
    [DataRow(LogEventLevel.Verbose, "VRB")]
    [DataRow(LogEventLevel.Debug, "DBG")]
    [DataRow(LogEventLevel.Information, "INF")]
    [DataRow(LogEventLevel.Warning, "WRN")]
    [DataRow(LogEventLevel.Error, "ERR")]
    [DataRow(LogEventLevel.Fatal, "FTL")]
    public void Format_LevelProperty_U3Format_AbbreviatesToThreeChars(LogEventLevel level, string expected)
    {
        var formatter = new StructuredLogFormatter("{Level:u3}");

        formatter.Format(BuildEvent(level)).Single().Value.Should().Be(expected);
    }

    [TestMethod]
    [DataRow(LogEventLevel.Information, "INFO")]
    [DataRow(LogEventLevel.Warning, "WARN")]
    public void Format_LevelProperty_U4Format_AbbreviatesToFourChars(LogEventLevel level, string expected)
    {
        var formatter = new StructuredLogFormatter("{Level:u4}");

        formatter.Format(BuildEvent(level)).Single().Value.Should().Be(expected);
    }

    [TestMethod]
    public void Format_LevelProperty_NoFormat_ReturnsEnumName()
    {
        var formatter = new StructuredLogFormatter("{Level}");

        formatter.Format(BuildEvent(LogEventLevel.Warning)).Single().Value.Should().Be("Warning");
    }

    [TestMethod]
    public void Format_MessageProperty_RendersTemplate()
    {
        var formatter = new StructuredLogFormatter("{Message}");
        var properties = new[] { new LogEventProperty("Name", new ScalarValue("world")) };

        var token = formatter.Format(BuildEvent(template: "hello {Name}", properties: properties)).Single();

        token.Type.Should().Be(StructuredLogFormatter.LogTokenType.Message);
        token.Value.Should().Be("hello \"world\"");
    }

    [TestMethod]
    public void Format_ExceptionProperty_OmittedWhenNoException()
    {
        var formatter = new StructuredLogFormatter("{Exception}");

        formatter.Format(BuildEvent()).Should().BeEmpty();
    }

    [TestMethod]
    public void Format_ExceptionProperty_EmitsExceptionToString()
    {
        var ex = new InvalidOperationException("boom");
        var formatter = new StructuredLogFormatter("{Exception}");

        var token = formatter.Format(BuildEvent(exception: ex)).Single();

        token.Type.Should().Be(StructuredLogFormatter.LogTokenType.Exception);
        token.Value.Should().Contain("boom").And.Contain(nameof(InvalidOperationException));
    }

    [TestMethod]
    public void Format_EnrichedProperty_StripsSurroundingQuotes()
    {
        var formatter = new StructuredLogFormatter("{ThreadId}");
        var properties = new[] { new LogEventProperty("ThreadId", new ScalarValue("42")) };

        var token = formatter.Format(BuildEvent(properties: properties)).Single();

        token.Type.Should().Be(StructuredLogFormatter.LogTokenType.Property);
        token.Value.Should().Be("42");
        token.PropertyName.Should().Be("ThreadId");
    }

    [TestMethod]
    public void Format_UnknownProperty_EmitsNothing()
    {
        var formatter = new StructuredLogFormatter("{NotPresent}");

        formatter.Format(BuildEvent()).Should().BeEmpty();
    }

    [TestMethod]
    public void Format_CombinedTemplate_PreservesTokenOrder()
    {
        var formatter = new StructuredLogFormatter("[{Level:u3}] {Message}{NewLine}");
        var properties = new[] { new LogEventProperty("Who", new ScalarValue("you")) };

        var values = formatter.Format(BuildEvent(LogEventLevel.Error, template: "hi {Who}", properties: properties))
            .Select(t => t.Value)
            .ToList();

        values.Should().Equal("[", "ERR", "] ", "hi \"you\"", Environment.NewLine);
    }
}
