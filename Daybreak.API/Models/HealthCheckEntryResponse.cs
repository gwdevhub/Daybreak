using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json.Serialization;

namespace Daybreak.API.Models;

public sealed class HealthCheckEntryResponse
{
    [JsonConverter(typeof(JsonStringEnumConverter<HealthStatus>))]
    public required HealthStatus Status { get; set; }
    public required string Description { get; set; }
    public required Dictionary<string, string> Data { get; set; }
    public required List<string> Tags { get; set; }
}
