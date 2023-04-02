using Daybreak.Models.Guildwars;
using Daybreak.Services.Pathfinding.Models;
using Daybreak.Utils;
using System.Collections.Generic;
using System.Extensions;
using System.Windows;

namespace Daybreak.Services.Pathfinding;

/// <summary>
/// Pathfinder based on Euclidean distance with discrete space.
/// </summary>
public sealed class StupidPathfinder : IPathfinder
{
    private const double PathStep = 1;

    public Result<PathfindingResponse, PathfindingFailure> CalculatePath(PathingData map, Point startPoint, Point endPoint)
    {
        if (GetContainingTrapezoid(map, startPoint) is not Trapezoid startTrapezoid)
        {
            return new PathfindingFailure.StartPointNotInMap(startPoint);
        }

        if (GetContainingTrapezoid(map, endPoint) is not Trapezoid endTrapezoid)
        {
            return new PathfindingFailure.StartPointNotInMap(endPoint);
        }

        if (GetTrapezoidPath(map, startTrapezoid, endTrapezoid) is not List<int> pathList)
        {
            return new PathfindingFailure.NoPathFound();
        }

        var pathfinding = new List<PathSegment>();
        var currentPoint = startPoint;
        var currentDirection = endPoint - currentPoint;
        currentDirection.Normalize();
        for(var i = 0; i < pathList.Count - 1; i++)
        {
            var currentTrapezoid = map.Trapezoids[pathList[i]];
            var nextTrapezoid = map.Trapezoids[pathList[i + 1]];
            var currentTrajectoryEndPoint = new Point(currentDirection.X * 10e6, currentDirection.Y * 10e6);
            if (LineSegmentIntersectsTrapezoid(currentPoint, currentTrajectoryEndPoint, currentTrapezoid) is not Point intersectionPoint)
            {
                return new PathfindingFailure.NoPathFound();
            }

            var validPoint = false;
            intersectionPoint += (currentDirection * PathStep);
            for(var j = i + 1; j < pathList.Count; j++)
            {
                var subsequentTrapezoid = map.Trapezoids[pathList[j]];
                if (MathUtils.PointInsideTrapezoid(subsequentTrapezoid, intersectionPoint))
                {
                    validPoint = true;
                    i = j - 1;
                    break;
                }
            }

            if (!validPoint)
            {
                var newCurrentPoint = GetClosestPointInTrapezoid(intersectionPoint, nextTrapezoid);
                currentDirection = endPoint - newCurrentPoint;
                currentDirection.Normalize();
                newCurrentPoint = new Point(newCurrentPoint.X, newCurrentPoint.Y);
                pathfinding.Add(new PathSegment { StartPoint = currentPoint, EndPoint = newCurrentPoint });
                currentPoint = newCurrentPoint;
            }
            else
            {
                var newCurrentPoint = intersectionPoint;
                pathfinding.Add(new PathSegment { StartPoint = currentPoint, EndPoint = newCurrentPoint });
                currentPoint = newCurrentPoint;
            }
        }

        pathfinding.Add(new PathSegment { StartPoint = currentPoint, EndPoint = endPoint });
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
        var trapezoidPoints = GetTrapezoidPoints(destination);

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

    private static Point? LineSegmentIntersectsTrapezoid(Point p1, Point p2, Trapezoid trapezoid)
    {
        var trapezoidPoints = GetTrapezoidPoints(trapezoid);
        for(var i = 0; i < trapezoidPoints.Length; i++)
        {
            var p3 = trapezoidPoints[i];
            var p4 = trapezoidPoints[(i + 1) % trapezoidPoints.Length];
            if (MathUtils.LineSegmentsIntersect(p1, p2, p3, p4, out var intersectionPoint, epsilon: 0.1))
            {
                return intersectionPoint;
            }
        }

        return default;
    }
    
    private static double DistanceSquaredBetweenTwoTrapezoidCenters(Trapezoid trapezoid1, Trapezoid trapezoid2)
    {
        var y1 = (trapezoid1.YT + trapezoid1.YB) / 2;
        var x1 = (((trapezoid1.XTL + trapezoid1.XBL) / 2) + ((trapezoid1.XTR + trapezoid1.XBR) / 2)) / 2;
        var y2 = (trapezoid2.YT + trapezoid2.YB) / 2;
        var x2 = (((trapezoid2.XTL + trapezoid2.XBL) / 2) + ((trapezoid2.XTR + trapezoid2.XBR) / 2)) / 2;
        return (new Point(x1, y1) - new Point(x2, y2)).LengthSquared;
    }

    private static Point[] GetTrapezoidPoints(Trapezoid trapezoid)
    {
        return new Point[]
        {
            new Point(trapezoid.XTL, trapezoid.YT),
            new Point(trapezoid.XTR, trapezoid.YT),
            new Point(trapezoid.XBR, trapezoid.YB),
            new Point(trapezoid.XBL, trapezoid.YB)
        };
    }
}
