using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json.Serialization;

namespace Daybreak.API.Models;

public sealed class HealthCheckResponse
{
    public required int ProcessId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter<HealthStatus>))]
    public required HealthStatus Status { get; set; }
    public required TimeSpan TotalDuration { get; set; }
    public required Dictionary<string, HealthCheckEntryResponse> Entries { get; set; }
}
