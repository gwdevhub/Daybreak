using Microsoft.Extensions.Logging;
using System;

namespace Daybreak.Models
{
    public sealed class Log
    {
        public string? Message { get; set; }
        public string? Category { get; set; }
        public LogLevel LogLevel { get; set; }
        public string? CorrelationVector { get; set; }
        public string? EventId { get; set; }
        public DateTime LogTime { get; set; }
    }
}
