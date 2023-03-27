using Daybreak.Models.Guildwars;
using Daybreak.Services.Pathfinding.Models;
using Daybreak.Utils;
using System.Collections.Generic;
using System.Extensions;
using System.Linq;
using System.Windows;

namespace Daybreak.Services.Pathfinding;

/// <summary>
/// Pathfinder based on euclidean distance with discrete space.
/// </summary>
public sealed class EuclideanDistancePathfinder : IPathfinder
{
    public Result<PathfindingResponse, PathfindingFailure> CalculatePath(List<Trapezoid> map, Point startPoint, Point endPoint)
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

        var trapezoidPath = GetTrapezoidPath(map, startTrapezoid, endTrapezoid);
        var pathfinding = new List<PathSegment>();
        for(var i = 0; i < trapezoidPath.Count - 2; i++)
        {
            var currentTrapezoid = map[trapezoidPath[i]];
            var nextTrapezoid = map[trapezoidPath[i + 1]];
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

    private static Trapezoid? GetContainingTrapezoid(List<Trapezoid> map, Point point)
    {
        foreach(var trapezoid in map)
        {
            if (MathUtils.PointInsideTrapezoid(trapezoid, point))
            {
                return trapezoid;
            }
        }

        return default;
    }

    private static List<int> GetTrapezoidPath(List<Trapezoid> trapezoids, Trapezoid startTrapezoid, Trapezoid endTrapezoid)
    {
        var visited = new int[trapezoids.Count];
        var visitationQueue = new Queue<Trapezoid>();
        visitationQueue.Enqueue(startTrapezoid);
        visited[startTrapezoid.Id] = (int)startTrapezoid.Id + 1;

        while(visitationQueue.TryDequeue(out var currentTrapezoid))
        {
            if (currentTrapezoid.Id == endTrapezoid.Id)
            {
                break;
            }

            foreach(var adjacentTrapezoidId in currentTrapezoid.AdjacentTrapezoidIds)
            {
                if (visited[adjacentTrapezoidId] > 0)
                {
                    continue;
                }

                visited[adjacentTrapezoidId] = (int)currentTrapezoid.Id + 1;
                visitationQueue.Enqueue(trapezoids[adjacentTrapezoidId]);
            }
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
