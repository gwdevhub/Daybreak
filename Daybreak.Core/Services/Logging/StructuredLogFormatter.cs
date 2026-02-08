using Serilog.Events;
using Serilog.Parsing;

namespace Daybreak.Services.Logging;

public sealed class StructuredLogFormatter(string outputTemplate)
{
    private readonly MessageTemplate outputTemplate = new MessageTemplateParser().Parse(outputTemplate);

    public enum LogTokenType
    {
        Text,
        Timestamp,
        Level,
        Message,
        Exception,
        Property,
        NewLine
    }

    public record LogToken(LogTokenType Type, string Value, string? PropertyName);

    public IEnumerable<LogToken> Format(LogEvent logEvent)
    {
        foreach (var token in this.outputTemplate.Tokens)
        {
            switch (token)
            {
                case TextToken textToken:
                    yield return new LogToken(LogTokenType.Text, textToken.Text, default);
                    break;

                case PropertyToken propertyToken:
                    foreach (var logToken in FormatProperty(propertyToken, logEvent))
                    {
                        yield return logToken;
                    }

                    break;
            }
        }
    }

    private static IEnumerable<LogToken> FormatProperty(PropertyToken propertyToken, LogEvent logEvent)
    {
        var name = propertyToken.PropertyName;

        if (name == "NewLine")
        {
            yield return new LogToken(LogTokenType.NewLine, Environment.NewLine, name);
            yield break;
        }

        if (name == "Timestamp")
        {
            var formatted = logEvent.Timestamp.ToString(propertyToken.Format ?? "yyyy-MM-dd HH:mm:ss");
            yield return new LogToken(LogTokenType.Timestamp, formatted, name);
            yield break;
        }

        if (name == "Level")
        {
            var level = FormatLevel(logEvent.Level, propertyToken.Format);
            yield return new LogToken(LogTokenType.Level, level, name);
            yield break;
        }

        if (name == "Message")
        {
            using var writer = new StringWriter();
            logEvent.RenderMessage(writer);
            yield return new LogToken(LogTokenType.Message, writer.ToString(), name);
            yield break;
        }

        if (name == "Exception")
        {
            if (logEvent.Exception is not null)
            {
                yield return new LogToken(LogTokenType.Exception, logEvent.Exception.ToString(), name);
            }

            yield break;
        }

        // Handle enriched properties (ThreadId, SourceContext, etc.)
        if (logEvent.Properties.TryGetValue(name, out var propertyValue))
        {
            var value = propertyValue.ToString().Trim('"');
            yield return new LogToken(LogTokenType.Property, value, name);
        }
    }

    private static string FormatLevel(LogEventLevel level, string? format)
    {
        return format switch
        {
            "u3" => level switch
            {
                LogEventLevel.Verbose => "VRB",
                LogEventLevel.Debug => "DBG",
                LogEventLevel.Information => "INF",
                LogEventLevel.Warning => "WRN",
                LogEventLevel.Error => "ERR",
                LogEventLevel.Fatal => "FTL",
                _ => level.ToString().ToUpperInvariant()[..3]
            },
            "u4" => level switch
            {
                LogEventLevel.Verbose => "VRBS",
                LogEventLevel.Debug => "DBUG",
                LogEventLevel.Information => "INFO",
                LogEventLevel.Warning => "WARN",
                LogEventLevel.Error => "EROR",
                LogEventLevel.Fatal => "FATL",
                _ => level.ToString().ToUpperInvariant()[..4]
            },
            _ => level.ToString()
        };
    }
}
