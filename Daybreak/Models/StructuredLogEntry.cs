using Serilog.Events;
using static Daybreak.Services.Logging.StructuredLogFormatter;

namespace Daybreak.Models;

public sealed record StructuredLogEntry(LogEvent Log, IReadOnlyList<LogToken> Tokens, string formattedText)
{
}
