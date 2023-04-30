namespace Daybreak.Services.Pathfinding.Models;

public abstract class PathfindingFailure
{
    public abstract string Reason { get; }

    private PathfindingFailure()
    {
    }

    public sealed class PathfindingDisabled : PathfindingFailure
    {
        public override string Reason => "Pathfinding is disabled";

        public PathfindingDisabled()
        {
        }
    }

    public sealed class NoPathFound : PathfindingFailure
    {
        public override string Reason => "No path found";

        public NoPathFound()
        {
        }
    }

    public sealed class UnexpectedFailure : PathfindingFailure
    {
        public override string Reason => "Encountered unexpected failure";

        public UnexpectedFailure()
        {
        }
    }
}
