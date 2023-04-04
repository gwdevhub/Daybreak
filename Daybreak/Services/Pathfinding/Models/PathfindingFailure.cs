using System.Windows;

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

    public sealed class StartPointNotInMap : PathfindingFailure
    {
        public override string Reason => "Starting point is not inside the map";
        public Point Point { get; }

        public StartPointNotInMap(Point point)
        {
            this.Point = point;
        }
    }

    public sealed class DestinationPointNotInMap : PathfindingFailure
    {
        public override string Reason => "Destination point is not inside the map";
        public Point Point { get; }

        public DestinationPointNotInMap(Point point)
        {
            this.Point = point;
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
