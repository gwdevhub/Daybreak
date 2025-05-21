using Daybreak.API.Interop;

namespace Daybreak.API.Models;

public sealed class AddressState
{
    public required string Name { get; init; }
    public required PointerValue Address { get; init; }
}
