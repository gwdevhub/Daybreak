using Daybreak.Models.Guildwars;
using Daybreak.Services.Pathfinding.Models;
using Daybreak.Utils;
using SharpNav;
using SharpNav.Geometry;
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
        var navMesh = NavMesh.Generate(
            GetTriangles(map),
            NavMeshGenerationSettings.Default);

        var navMeshQuery = new NavMeshQuery(navMesh, 10000);
        var start = new Vector3((float)startPoint.X, (float)startPoint.Y, 0);
        var end = new Vector3((float)endPoint.X, (float)endPoint.Y, 0);


        return new PathfindingFailure.NoPathFound();
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

    private static List<Triangle3> GetTriangles(PathingData map)
    {
        var list = new List<Triangle3>();
        foreach(var trapezoid in map.Trapezoids)
        {
            var trapezoidPoints = new Point[]
            {
                new Point(trapezoid.XTL, trapezoid.YT),
                new Point(trapezoid.XTR, trapezoid.YT),
                new Point(trapezoid.XBR, trapezoid.YB),
                new Point(trapezoid.XBL, trapezoid.YB)
            };

            list.Add(new Triangle3
            {
                A = new Vector3((float)trapezoidPoints[0].X, (float)trapezoidPoints[0].Y, 0),
                B = new Vector3((float)trapezoidPoints[1].X, (float)trapezoidPoints[1].Y, 0),
                C = new Vector3((float)trapezoidPoints[2].X, (float)trapezoidPoints[2].Y, 0),
            });

            list.Add(new Triangle3
            {
                A = new Vector3((float)trapezoidPoints[1].X, (float)trapezoidPoints[1].Y, 0),
                B = new Vector3((float)trapezoidPoints[2].X, (float)trapezoidPoints[2].Y, 0),
                C = new Vector3((float)trapezoidPoints[3].X, (float)trapezoidPoints[3].Y, 0),
            });
        }

        return list;
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

    private static bool LineIntersectsWithTrapezoidVerticalSides(Point p1, Point p2, Trapezoid destination, out Point? intersectionPoint, out (Point?, Point?) intersectedSide)
    {
        intersectionPoint = default;
        intersectedSide = default;
        var trapezoidPoints = new Point[]
        {
            new Point(destination.XBL, destination.YB),
            new Point(destination.XBR, destination.YB),
            new Point(destination.XTL, destination.YT),
            new Point(destination.XTR, destination.YT)
        };

        var sides = new[]
        {
            (trapezoidPoints[0], trapezoidPoints[2]),
            (trapezoidPoints[1], trapezoidPoints[3])
        };

        foreach(var side in sides)
        {
            if (MathUtils.LineSegmentsIntersect(p1, p2, side.Item1, side.Item2, out intersectionPoint))
            {
                intersectedSide = side;
                return true;
            }
        }

        return false;
    }

    private static bool LineIntersectsWithTrapezoidHorizontalSides(Point p1, Point p2, Trapezoid destination, out Point? intersectionPoint, out (Point?, Point?) intersectedSide)
    {
        intersectionPoint = default;
        intersectedSide = default;
        var trapezoidPoints = new Point[]
        {
            new Point(destination.XBL, destination.YB),
            new Point(destination.XBR, destination.YB),
            new Point(destination.XTL, destination.YT),
            new Point(destination.XTR, destination.YT)
        };

        var sides = new[]
        {
            (trapezoidPoints[0], trapezoidPoints[1]),
            (trapezoidPoints[2], trapezoidPoints[3])
        };

        foreach (var side in sides)
        {
            if (MathUtils.LineSegmentsIntersect(p1, p2, side.Item1, side.Item2, out intersectionPoint))
            {
                intersectedSide = side;
                return true;
            }
        }

        return false;
    }

    private static double DistanceSquaredBetweenTwoTrapezoidCenters(Trapezoid trapezoid1, Trapezoid trapezoid2)
    {
        var y1 = (trapezoid1.YT + trapezoid1.YB) / 2;
        var x1 = (((trapezoid1.XTL + trapezoid1.XBL) / 2) + ((trapezoid1.XTR + trapezoid1.XBR) / 2)) / 2;
        var y2 = (trapezoid2.YT + trapezoid2.YB) / 2;
        var x2 = (((trapezoid2.XTL + trapezoid2.XBL) / 2) + ((trapezoid2.XTR + trapezoid2.XBR) / 2)) / 2;
        return (new Point(x1, y1) - new Point(x2, y2)).LengthSquared;
    }

    private static bool TrianglesAdjacent(Point[] trianglePoints, Point[] nextTrianglePoints)
    {
        for (var i = 0; i < trianglePoints.Length; i++)
        {
            for (var j = 0; j < nextTrianglePoints.Length; j++)
            {
                if (MathUtils.LineSegmentsIntersect(
                    trianglePoints[i],
                    trianglePoints[(i + 1) % trianglePoints.Length],
                    nextTrianglePoints[j],
                    nextTrianglePoints[(j + 1) % nextTrianglePoints.Length],
                    out _))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static double GetSlopeOfLine(Point p1, Point p2)
    {
        return (p2.Y - p1.Y) / (p2.X - p1.X);
    }
}
