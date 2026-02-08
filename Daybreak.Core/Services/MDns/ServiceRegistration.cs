namespace Daybreak.Services.MDns;

internal sealed class ServiceRegistration
{
    public required string Name { get; init; }
    public required DateTime Expiration { get; init; }
    public required IReadOnlyList<Uri> Uris { get; init; }
}
