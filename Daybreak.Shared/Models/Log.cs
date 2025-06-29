using Microsoft.Extensions.Logging;

namespace Daybreak.Shared.Models;

public sealed class Log
{
    public string? Message { get; set; }
    public string? Category { get; set; }
    public LogLevel LogLevel { get; set; }
    public string? CorrelationVector { get; set; }
    public string? EventId { get; set; }
    public DateTimeOffset LogTime { get; set; }
}
