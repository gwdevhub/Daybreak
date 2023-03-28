using Daybreak.Models.Guildwars;
using Daybreak.Services.Pathfinding.Models;
using Daybreak.Utils;
using Microsoft.VisualBasic;
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
        var currentPoint = startPoint;
        for(var i = 1; i < trapezoidPath.Count; i++)
        {
            var nextTrapezoid = map.Trapezoids[trapezoidPath[i]];
            var closestNextPoint = GetClosestPointInTrapezoid(currentPoint, nextTrapezoid);
            pathfinding.Add(new PathSegment
            {
                StartPoint = currentPoint,
                EndPoint = closestNextPoint
            });

            currentPoint = closestNextPoint;
        }

        // Add the last straight line from the edge of the trapezoid to the destination point
        pathfinding.Add(new PathSegment
        {
            StartPoint = currentPoint,
            EndPoint = endPoint
        });

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
        var minDistance = double.MaxValue;
        var visited = new int[map.Trapezoids.Count];
        var distanceFromVisited = new double[map.Trapezoids.Count];
        var visitationQueue = new Queue<(Trapezoid Trapezoid, double CurrentPathDistance)>();
        visitationQueue.Enqueue((startTrapezoid, 0));
        visited[startTrapezoid.Id] = (int)startTrapezoid.Id + 1;

        while(visitationQueue.TryDequeue(out var tuple))
        {
            var currentTrapezoid = tuple.Trapezoid;
            var currentDistance = tuple.CurrentPathDistance;
            if (currentDistance > minDistance)
            {
                continue;
            }

            if (currentTrapezoid.Id == endTrapezoid.Id)
            {
                minDistance = currentDistance;
                continue;
            }

            foreach(var adjacentTrapezoidId in map.AdjacencyArray[currentTrapezoid.Id])
            {
                var nextTrapezoid = map.Trapezoids[adjacentTrapezoidId];
                var distanceToNextTrapezoid = DistanceSquaredBetweenTwoTrapezoidCenters(currentTrapezoid, nextTrapezoid);
                if (currentDistance + distanceToNextTrapezoid > minDistance)
                {
                    continue;
                }

                if (visited[adjacentTrapezoidId] > 0)
                {
                    continue;
                }

                visited[adjacentTrapezoidId] = currentTrapezoid.Id + 1;
                distanceFromVisited[adjacentTrapezoidId] = distanceToNextTrapezoid;
                visitationQueue.Enqueue((nextTrapezoid, currentDistance + distanceToNextTrapezoid));
            }
        }

        if (minDistance == double.MaxValue)
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

    private static Point GetClosestPointInTrapezoid(Point startingPoint, Trapezoid destination)
    {
        var trapezoidPoints = new Point[]
        {
            new Point(destination.XTL, destination.YT),
            new Point(destination.XTR, destination.YT),
            new Point(destination.XBR, destination.YB),
            new Point(destination.XBL, destination.YB)
        };

        var closestPoint = new Point();
        var closestDistance = double.MaxValue;
        for(var i = 0; i < trapezoidPoints.Length - 1; i++)
        {
            var closestPointToSegment = MathUtils.ClosestPointOnLineSegment(trapezoidPoints[i], trapezoidPoints[i + 1], startingPoint);
            var distanceToSegment = (startingPoint - closestPointToSegment).LengthSquared;
            if (distanceToSegment < closestDistance &&
                distanceToSegment > 0)
            {
                closestDistance = distanceToSegment;
                closestPoint = closestPointToSegment;
            }
        }

        return closestPoint;
    }

    private static double DistanceSquaredBetweenTwoTrapezoidCenters(Trapezoid trapezoid1, Trapezoid trapezoid2)
    {
        var y1 = (trapezoid1.YT + trapezoid1.YB) / 2;
        var x1 = (((trapezoid1.XTL + trapezoid1.XBL) / 2) + ((trapezoid1.XTR + trapezoid1.XBR) / 2)) / 2;
        var y2 = (trapezoid2.YT + trapezoid2.YB) / 2;
        var x2 = (((trapezoid2.XTL + trapezoid2.XBL) / 2) + ((trapezoid2.XTR + trapezoid2.XBR) / 2)) / 2;
        return (new Point(x1, y1) - new Point(x2, y2)).LengthSquared;
    }
}
