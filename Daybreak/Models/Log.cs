using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace Daybreak.Models
{
    public sealed class Log
    {
        [JsonProperty("DateTime")]
        public DateTime LogTime { get; set; }
        [JsonProperty("Category")]
        public string Category { get; set; }
        [JsonProperty("EventId")]
        public string EventId { get; set; }
        [JsonProperty("LogLevel")]
        public LogLevel LogLevel { get; set; }
        [JsonProperty("Message")]
        public string Message { get; set; }
    }
}
