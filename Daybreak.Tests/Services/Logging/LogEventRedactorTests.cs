using Daybreak.Services.Logging;
using FluentAssertions;
using Serilog.Events;
using Serilog.Parsing;

namespace Daybreak.Tests.Services.Logging;

[TestClass]
public sealed class LogEventRedactorTests
{
    private static readonly MessageTemplateParser Parser = new();

    private static LogEvent BuildEvent(
        string template = "hello",
        IEnumerable<LogEventProperty>? properties = null,
        Exception? exception = null)
    {
        return new LogEvent(
            new DateTimeOffset(2024, 1, 2, 3, 4, 5, TimeSpan.Zero),
            LogEventLevel.Information,
            exception,
            Parser.Parse(template),
            properties ?? []);
    }

    [TestMethod]
    public void RedactText_MasksQuotedEmailArgument()
    {
        LogEventRedactor.RedactText("Launching -email \"user@example.com\" -character \"Daybreak\"")
            .Should().Be("Launching -email [REDACTED] -character \"Daybreak\"");
    }

    [TestMethod]
    public void RedactText_MasksQuotedPasswordArgument()
    {
        LogEventRedactor.RedactText("-password \"s3cr3t p@ss\"")
            .Should().Be("-password [REDACTED]");
    }

    [TestMethod]
    public void RedactText_MasksUnquotedArguments()
    {
        LogEventRedactor.RedactText("-email user -password hunter2")
            .Should().Be("-email [REDACTED] -password [REDACTED]");
    }

    [TestMethod]
    [DataRow("-EMAIL \"user\"", "-EMAIL [REDACTED]")]
    [DataRow("-Password \"pw\"", "-Password [REDACTED]")]
    public void RedactText_IsCaseInsensitive(string input, string expected)
    {
        LogEventRedactor.RedactText(input).Should().Be(expected);
    }

    [TestMethod]
    public void RedactText_DoesNotMatchLongerFlagNames()
    {
        LogEventRedactor.RedactText("-emailaddress kept").Should().Be("-emailaddress kept");
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void RedactText_ReturnsEmptyForNullOrEmpty(string? input)
    {
        LogEventRedactor.RedactText(input).Should().BeEmpty();
    }

    [TestMethod]
    public void Redact_MasksSecretsInsideRenderedMessage()
    {
        var properties = new[] { new LogEventProperty("Output", new ScalarValue("started with -email \"a@b.c\" -password \"pw\"")) };

        var redacted = LogEventRedactor.Redact(BuildEvent("Injector output: {Output}", properties));

        redacted.RenderMessage().Should().Be("Injector output: \"started with -email [REDACTED] -password [REDACTED]\"");
    }

    [TestMethod]
    public void Redact_MasksSecretsEmbeddedInTemplateText()
    {
        var redacted = LogEventRedactor.Redact(BuildEvent("Running -password \"literal\" now"));

        redacted.RenderMessage().Should().Be("Running -password [REDACTED] now");
    }

    [TestMethod]
    [DataRow("Username")]
    [DataRow("Password")]
    [DataRow("Email")]
    [DataRow("username")]
    public void Redact_MasksSensitivePropertyValuesByName(string propertyName)
    {
        var properties = new[] { new LogEventProperty(propertyName, new ScalarValue("john.doe")) };

        var redacted = LogEventRedactor.Redact(BuildEvent($"user {{{propertyName}}}", properties));

        redacted.Properties[propertyName].Should().BeEquivalentTo(new ScalarValue("[REDACTED]"));
        redacted.RenderMessage().Should().Be("user \"[REDACTED]\"");
    }

    [TestMethod]
    public void Redact_MasksSensitivePropertiesNestedInStructures()
    {
        var structure = new StructureValue(
        [
            new LogEventProperty("Identifier", new ScalarValue("acc-1")),
            new LogEventProperty("Username", new ScalarValue("john")),
            new LogEventProperty("Password", new ScalarValue("pw")),
        ]);
        var properties = new[] { new LogEventProperty("Credentials", structure) };

        var redacted = LogEventRedactor.Redact(BuildEvent("{@Credentials}", properties));

        var redactedStructure = (StructureValue)redacted.Properties["Credentials"];
        redactedStructure.Properties.Single(p => p.Name == "Identifier").Value.Should().BeEquivalentTo(new ScalarValue("acc-1"));
        redactedStructure.Properties.Single(p => p.Name == "Username").Value.Should().BeEquivalentTo(new ScalarValue("[REDACTED]"));
        redactedStructure.Properties.Single(p => p.Name == "Password").Value.Should().BeEquivalentTo(new ScalarValue("[REDACTED]"));
    }

    [TestMethod]
    public void Redact_PreservesNonSensitiveDataAndMetadata()
    {
        var exception = new InvalidOperationException("boom");
        var properties = new[] { new LogEventProperty("Count", new ScalarValue(42)) };

        var original = BuildEvent("processed {Count}", properties, exception);
        var redacted = LogEventRedactor.Redact(original);

        redacted.Timestamp.Should().Be(original.Timestamp);
        redacted.Level.Should().Be(original.Level);
        redacted.Exception.Should().BeSameAs(exception);
        redacted.RenderMessage().Should().Be("processed 42");
    }
}
