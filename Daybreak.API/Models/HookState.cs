using Daybreak.API.Interop;

namespace Daybreak.API.Models;

public sealed class HookState
{
    public required bool Hooked { get; init; }
    public required string Name { get; init; }
    public required PointerValue TargetAddress { get; init; }
    public required PointerValue ContinueAddress { get; init; }
    public required PointerValue DetourAddress { get; init; }
}
