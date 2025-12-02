namespace Daybreak.Shared.Models.FocusView;
public sealed class VanquishComponentContext
{
    public required uint FoesToKill { get; init; }
    public required uint FoesKilled { get; init; }
    public required bool HardMode { get; init; }
    public required bool Vanquishing { get; init; } 
    public required PointsDisplay Display { get; init; }
}
