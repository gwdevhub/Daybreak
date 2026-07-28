using Serilog.Events;
using Serilog.Parsing;
using System.Buffers;
using System.Text.RegularExpressions;

namespace Daybreak.Services.Logging;

/// <summary>
/// Rewrites <see cref="LogEvent"/> instances so that sensitive user information (usernames,
/// e-mail addresses and passwords) is masked before it reaches a sink.
/// <para>
/// It relies on two complementary strategies:
/// <list type="bullet">
/// <item>Regex matching against the command-line style tokens (<c>-email &lt;value&gt;</c>,
/// <c>-password &lt;value&gt;</c>) that appear in rendered messages and property values.</item>
/// <item>Property-name matching, which masks any structured property named after a credential
/// field (Username, Password, Email) regardless of its value.</item>
/// </list>
/// </para>
/// </summary>
public static partial class LogEventRedactor
{
    public const string RedactedValue = "[REDACTED]";

    // Structured property names whose value must always be masked.
    private static readonly HashSet<string> SensitivePropertyNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "Username",
        "Password",
        "Email",
    };

    // Cheap, vectorized pre-filter: the (comparatively expensive) look-behind regex patterns are
    // skipped unless one of these credential flags is present in the text. SearchValues performs a
    // single multi-pattern scan (Teddy/Aho-Corasick internally) that outperforms both individual
    // Contains calls and a hand-rolled automaton, and scales as the marker set grows. Every secret
    // pattern below must be gated by one of these markers.
    private static readonly SearchValues<string> SecretMarkers = SearchValues.Create(
        ["-email", "-password"],
        StringComparison.OrdinalIgnoreCase);

    // Patterns whose single match is the secret value to mask. The credential flag itself is kept
    // in a look-behind so only the value is replaced.
    private static readonly Regex[] SecretPatterns =
    [
        EmailArgumentRegex(),
        PasswordArgumentRegex(),
    ];

    /// <summary>
    /// Returns a copy of <paramref name="logEvent"/> with sensitive information masked. The original
    /// event is returned unchanged when nothing needs to be redacted.
    /// </summary>
    public static LogEvent Redact(LogEvent logEvent)
    {
        ArgumentNullException.ThrowIfNull(logEvent);

        var redactedTemplate = RedactTemplate(logEvent.MessageTemplate);
        var redactedProperties = logEvent.Properties
            .Select(p => new LogEventProperty(p.Key, RedactPropertyValue(p.Key, p.Value)))
            .ToList();

        return new LogEvent(
            logEvent.Timestamp,
            logEvent.Level,
            logEvent.Exception,
            redactedTemplate,
            redactedProperties);
    }

    /// <summary>
    /// Masks the credential values embedded in an arbitrary string using the configured regex patterns.
    /// </summary>
    public static string RedactText(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text ?? string.Empty;
        }

        // Fast path: avoid running the look-behind regexes over the overwhelming majority of log
        // text that cannot contain a credential flag. This keeps redaction global (fail-safe) while
        // costing only a vectorized substring scan for events that carry no secrets.
        if (!MightContainSecret(text))
        {
            return text;
        }

        foreach (var pattern in SecretPatterns)
        {
            text = pattern.Replace(text, RedactedValue);
        }

        return text;
    }

    private static bool MightContainSecret(string text)
    {
        return text.AsSpan().ContainsAny(SecretMarkers);
    }

    private static MessageTemplate RedactTemplate(MessageTemplate template)
    {
        var tokens = template.Tokens
            .Select(token => token is TextToken textToken
                ? new TextToken(RedactText(textToken.Text))
                : token)
            .ToList();

        return new MessageTemplate(tokens);
    }

    private static LogEventPropertyValue RedactPropertyValue(string propertyName, LogEventPropertyValue value)
    {
        if (SensitivePropertyNames.Contains(propertyName))
        {
            return new ScalarValue(RedactedValue);
        }

        return RedactPropertyValue(value);
    }

    private static LogEventPropertyValue RedactPropertyValue(LogEventPropertyValue value)
    {
        switch (value)
        {
            case ScalarValue { Value: string stringValue }:
                return new ScalarValue(RedactText(stringValue));

            case SequenceValue sequence:
                return new SequenceValue(sequence.Elements.Select(RedactPropertyValue));

            case StructureValue structure:
                var structureProperties = structure.Properties
                    .Select(p => new LogEventProperty(p.Name, RedactPropertyValue(p.Name, p.Value)));
                return new StructureValue(structureProperties, structure.TypeTag);

            case DictionaryValue dictionary:
                var elements = dictionary.Elements
                    .Select(kvp => new KeyValuePair<ScalarValue, LogEventPropertyValue>(
                        kvp.Key,
                        RedactPropertyValue(kvp.Value)));
                return new DictionaryValue(elements);

            default:
                return value;
        }
    }

    [GeneratedRegex(@"(?<=-email[=\s])(""[^""]*""|'[^']*'|\S+)", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex EmailArgumentRegex();

    [GeneratedRegex(@"(?<=-password[=\s])(""[^""]*""|'[^']*'|\S+)", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex PasswordArgumentRegex();
}
