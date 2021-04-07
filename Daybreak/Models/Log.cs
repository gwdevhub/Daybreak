using System;

namespace Daybreak.Models
{
    public class Log
    {
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
