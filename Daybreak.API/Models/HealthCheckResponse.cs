namespace Daybreak.API.Models;

public sealed record HealthCheckResponse(int ProcessId, IEnumerable<AddressHealth> AddressHealth);