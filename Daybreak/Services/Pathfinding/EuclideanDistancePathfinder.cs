using Daybreak.Models.Guildwars;
using Daybreak.Services.Pathfinding.Models;
using Daybreak.Utils;
using System.Collections.Generic;
using System.Extensions;
using System.Windows;

namespace Daybreak.Services.Pathfinding;

/// <summary>
/// Pathfinder based on euclidean distance with discrete space.
/// </summary>
public sealed class EuclideanDistancePathfinder : IPathfinder
{
    public Result<PathfindingResponse, PathfindingFailure> CalculatePath(PathingData map, Point startPoint, Point endPoint)
    {
        /*
         * Find the start and end trapezoids.
         * If one of them can not be found, it means that one of the points is invalid.
         * If both points are in the same trapezoid, simply return the path between them.
         * Perform a shortest path search from the starting trapezoid to the ending trapezoid.
         */
        var maybeStartTrapezoid = GetContainingTrapezoid(map, startPoint);
        if (maybeStartTrapezoid is not Trapezoid startTrapezoid)
        {
            return new PathfindingFailure.StartPointNotInMap(startPoint);
        }

        var maybeEndTrapezoid = GetContainingTrapezoid(map, endPoint);
        if (maybeEndTrapezoid is not Trapezoid endTrapezoid)
        {
            return new PathfindingFailure.DestinationPointNotInMap(endPoint);
        }

        if (startTrapezoid.Id == endTrapezoid.Id)
        {
            return new PathfindingResponse
            {
                Pathing = new List<PathSegment>
                {
                    new PathSegment
                    {
                        StartPoint = startPoint,
                        EndPoint = endPoint
                    }
                }
            };
        }

        var maybeTrapezoidPath = GetTrapezoidPath(map, startTrapezoid, endTrapezoid);
        if (maybeTrapezoidPath is not List<int> trapezoidPath)
        {
            return new PathfindingFailure.NoPathFound();
        }

        var pathfinding = new List<PathSegment>();
        for(var i = 0; i < trapezoidPath.Count - 1; i++)
        {
            var currentTrapezoid = map.Trapezoids[trapezoidPath[i]];
            var nextTrapezoid = map.Trapezoids[trapezoidPath[i + 1]];
            var currentY = (currentTrapezoid.YT + currentTrapezoid.YB) / 2;
            var currentX = (((currentTrapezoid.XTL + currentTrapezoid.XBL) / 2) + ((currentTrapezoid.XTR + currentTrapezoid.XBR) / 2)) / 2;
            var nextY = (nextTrapezoid.YT + nextTrapezoid.YB) / 2;
            var nextX = (((nextTrapezoid.XTL + nextTrapezoid.XBL) / 2) + ((nextTrapezoid.XTR + nextTrapezoid.XBR) / 2)) / 2;
            pathfinding.Add(new PathSegment
            {
                StartPoint = new Point(currentX, currentY),
                EndPoint = new Point(nextX, nextY)
            });
        }

        return new PathfindingResponse
        {
            Pathing = pathfinding
        };
    }

    private static Trapezoid? GetContainingTrapezoid(PathingData map, Point point)
    {
        foreach(var trapezoid in map.Trapezoids)
        {
            if (MathUtils.PointInsideTrapezoid(trapezoid, point))
            {
                return trapezoid;
            }
        }

        return default;
    }

    private static List<int>? GetTrapezoidPath(PathingData map, Trapezoid startTrapezoid, Trapezoid endTrapezoid)
    {
        bool found = false;
        var visited = new int[map.Trapezoids.Count];
        var visitationQueue = new Queue<Trapezoid>();
        visitationQueue.Enqueue(startTrapezoid);
        visited[startTrapezoid.Id] = (int)startTrapezoid.Id + 1;

        while(visitationQueue.TryDequeue(out var currentTrapezoid))
        {
            if (currentTrapezoid.Id == endTrapezoid.Id)
            {
                found = true;
                break;
            }

            foreach(var adjacentTrapezoidId in map.AdjacencyArray[currentTrapezoid.Id])
            {
                if (visited[adjacentTrapezoidId] > 0)
                {
                    continue;
                }

                visited[adjacentTrapezoidId] = (int)currentTrapezoid.Id + 1;
                visitationQueue.Enqueue(map.Trapezoids[adjacentTrapezoidId]);
            }
        }
        if (!found)
        {
            return default;
        }

        var backTrackingList = new List<int>();
        var currentTrapezoidId = (int)endTrapezoid.Id;
        while (true)
        {
            backTrackingList.Add(currentTrapezoidId);
            if (currentTrapezoidId == startTrapezoid.Id)
            {
                backTrackingList.Reverse();
                return backTrackingList;
            }

            currentTrapezoidId = visited[currentTrapezoidId] - 1;
        }
    }
}
