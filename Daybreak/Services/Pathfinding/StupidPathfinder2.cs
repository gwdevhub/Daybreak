using Daybreak.Models.Guildwars;
using Daybreak.Services.Pathfinding.Models;
using Daybreak.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Point = System.Windows.Point;

namespace Daybreak.Services.Pathfinding;
internal sealed class StupidPathfinder2 : IPathfinder
{
    private const double PathStep = 1;
    private const double MinHeight = 10e1;
    private const double MaxEdgeSize = 50e1;
    private const double IntersectionThreshold = 0.3;

    public Task<Result<PathfindingResponse, PathfindingFailure>> CalculatePath(PathingData map, Point startPoint, Point endPoint, CancellationToken cancellationToken)
    {
        return new TaskFactory().StartNew(() => this.CalculatePathInternal(map, startPoint, endPoint), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    public Task<object?> GenerateNavMesh(List<Trapezoid> trapezoids, List<List<int>> computedAdjacencyList, CancellationToken cancellationToken)
    {
        return new TaskFactory().StartNew<object?>(() => GenerateNavMeshInternal(trapezoids, computedAdjacencyList), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);   
    }

    private Result<PathfindingResponse, PathfindingFailure> CalculatePathInternal(PathingData map, Point startPoint, Point endPoint)
    {
        if (map is null ||
            map.Trapezoids is null ||
            map.NavMesh is not SuperOvercomplicatedNavmesh superOvercomplicatedNavmesh ||
            superOvercomplicatedNavmesh.Trapezoids is null)
        {
            return new PathfindingFailure.UnexpectedFailure();
        }

        if (GetContainingTrapezoid(superOvercomplicatedNavmesh.Trapezoids, startPoint) is not TrapezoidNode startTrapezoid)
        {
            return new PathfindingFailure.NoPathFound();
        }

        if (GetContainingTrapezoid(superOvercomplicatedNavmesh.Trapezoids, endPoint) is not TrapezoidNode endTrapezoid)
        {
            return new PathfindingFailure.NoPathFound();
        }

        var path = FindPath(superOvercomplicatedNavmesh, startTrapezoid, endTrapezoid);
        var pathfinding = new List<PathSegment>();
        var currentPoint = startPoint;
        var currentDirection = endPoint - currentPoint;
        currentDirection.Normalize();
        for (var i = 0; i < path.Count - 1; i++)
        {
            var currentTrapezoid = path[i];
            var nextTrapezoid = path[i + 1];
            var currentTrajectoryEndPoint = new Point(currentDirection.X * 10e6, currentDirection.Y * 10e6);
            if (LineSegmentIntersectsTrapezoid(currentPoint, currentTrajectoryEndPoint, currentTrapezoid.Trapezoid) is not Point intersectionPoint)
            {
                return new PathfindingFailure.NoPathFound();
            }

            var validPoint = false;
            intersectionPoint += currentDirection * PathStep;
            for (var j = i + 1; j < path.Count; j++)
            {
                var subsequentTrapezoid = path[j];
                if (MathUtils.PointInsideTrapezoid(subsequentTrapezoid.Trapezoid, intersectionPoint))
                {
                    validPoint = true;
                    i = j - 1;
                    break;
                }
            }

            if (!validPoint)
            {
                var newCurrentPoint = GetClosestPointInTrapezoid(intersectionPoint, nextTrapezoid.Trapezoid);
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
        pathfinding = OptimizePath(superOvercomplicatedNavmesh, pathfinding);
        return new PathfindingResponse
        {
            Pathing = pathfinding,
        };
    }

    private static List<PathSegment> OptimizePath(SuperOvercomplicatedNavmesh navmesh, List<PathSegment> pathSegments)
    {
        bool changed;
        do
        {
            changed = false;
            for (int i = 0; i < pathSegments.Count - 1; i++)
            {
                var currentSegment = pathSegments[i];
                var nextSegment = pathSegments[i + 1];

                if (CanDirectlyConnect(navmesh, currentSegment.StartPoint, nextSegment.EndPoint))
                {
                    // Update the endpoint of the current segment
                    pathSegments[i] = new PathSegment
                    {
                        StartPoint = currentSegment.StartPoint,
                        EndPoint = nextSegment.EndPoint
                    };
                    // Remove the next segment
                    pathSegments.RemoveAt(i + 1);
                    changed = true;
                }
            }
        } while (changed);

        return pathSegments;
    }

    private static bool CanDirectlyConnect(SuperOvercomplicatedNavmesh navmesh, Point start, Point end)
    {
        const int numSamples = 10; // Number of points to sample along the line
        for (int i = 0; i <= numSamples; i++)
        {
            float t = (float)i / numSamples;
            var samplePoint = Lerp(start, end, t);
            if (GetContainingTrapezoid(navmesh.Trapezoids, samplePoint) is null)
            {
                return false;
            }
        }

        return true;
    }

    private static List<TrapezoidNode> FindPath(SuperOvercomplicatedNavmesh navmesh, TrapezoidNode start, TrapezoidNode goal)
    {
        var openSet = new HashSet<TrapezoidNode> { start };
        var cameFrom = new Dictionary<TrapezoidNode, TrapezoidNode>();

        var gScore = new Dictionary<TrapezoidNode, double>();
        var fScore = new Dictionary<TrapezoidNode, double>();

        foreach (var node in navmesh.Trapezoids)
        {
            gScore[node] = double.MaxValue;
            fScore[node] = double.MaxValue;
        }

        gScore[start] = 0f;
        fScore[start] = Heuristic(start, goal);

        while (openSet.Count > 0)
        {
            var current = openSet.OrderBy(node => fScore[node]).First();

            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);

            foreach (var neighbor in current.AdjacencyList)
            {
                double tentativeGScore = gScore[current] + Distance(current, neighbor);

                if (tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return new List<TrapezoidNode>(); // Return empty path if no path is found
    }

    private static List<TrapezoidNode> ReconstructPath(Dictionary<TrapezoidNode, TrapezoidNode> cameFrom, TrapezoidNode current)
    {
        var totalPath = new List<TrapezoidNode> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }

    private static double Heuristic(TrapezoidNode a, TrapezoidNode b)
    {
        var centroidA = CalculateCentroid(a.Trapezoid);
        var centroidB = CalculateCentroid(b.Trapezoid);
        return EuclideanDistance(centroidA, centroidB);
    }

    private static double Distance(TrapezoidNode a, TrapezoidNode b)
    {
        return Heuristic(a, b); // Since distance and heuristic are the same in this case
    }

    private static TrapezoidNode? GetContainingTrapezoid(List<TrapezoidNode> nodes, Point point)
    {
        foreach (var trapezoid in nodes)
        {
            if (MathUtils.PointInsideTrapezoid(trapezoid.Trapezoid, point))
            {
                return trapezoid;
            }
        }

        return default;
    }

    private static SuperOvercomplicatedNavmesh? GenerateNavMeshInternal(List<Trapezoid> trapezoids, List<List<int>> computedAdjacencyList)
    {
        var sw = Stopwatch.StartNew();
        var nodes = trapezoids.Select((t, i) => new TrapezoidNode
        {
            Trapezoid = t,
            Id = i
        }).ToList();
        for(var i = 0; i < computedAdjacencyList.Count; i++)
        {
            var adjacencies = computedAdjacencyList[i];
            foreach(var id in adjacencies)
            {
                nodes[i].AdjacencyList.Add(nodes[id]);
            }
        }

        MergeTrapezoidsAlongHeight(nodes);
        CullOverlappingTrapezoids(nodes);
        SplitTrapezoidsAlongWidth(nodes);
        //CullOverlappingTrapezoids(nodes);

        var elapsedTime = sw.ElapsedMilliseconds;
        return new SuperOvercomplicatedNavmesh
        {
            Trapezoids = nodes
        };
    }

    private static void CullOverlappingTrapezoids(List<TrapezoidNode> nodes)
    {
        for(var i = 0; i < nodes.Count; i++)
        {
            var currentRect = GetBoundingRectangle(MathUtils.GetTrapezoidPoints(nodes[i].Trapezoid));
            var modified = false;
            for (var j = i + 1; j < nodes.Count; j++)
            {
                var otherRect = GetBoundingRectangle(MathUtils.GetTrapezoidPoints(nodes[j].Trapezoid));
                var intersection = CalculateIntersection(currentRect, otherRect);
                if (intersection.Width == intersection.Height &&
                    intersection.Height == 0)
                {
                    continue;
                }

                var currentArea = currentRect.Width * currentRect.Height;
                var otherArea = otherRect.Width * otherRect.Height;
                var intersectionArea = intersection.Width * intersection.Height;
                if (intersectionArea < currentArea * IntersectionThreshold &&
                    intersectionArea < otherArea * IntersectionThreshold)
                {
                    continue;
                }

                modified = true;
                var newTrapezoid = MergeTrapezoids(nodes[i].Trapezoid, nodes[j].Trapezoid);
                nodes[i].Trapezoid = newTrapezoid;
                for(var k = 0; k < nodes[j].AdjacencyList.Count; k++)
                {
                    var neighbor = nodes[j].AdjacencyList[k];
                    neighbor.AdjacencyList.Remove(nodes[j]);
                    nodes[j].AdjacencyList.RemoveAt(k);
                    k--;
                    if (neighbor == nodes[i])
                    {
                        continue;
                    }

                    if (!nodes[i].AdjacencyList.Contains(neighbor))
                    {
                        nodes[i].AdjacencyList.Add(neighbor);
                        neighbor.AdjacencyList.Add(nodes[i]);
                    }
                }

                nodes.RemoveAt(j);
                j--;
            }

            if (modified)
            {
                i--;
            }
        }
    }

    private static void MergeTrapezoidsAlongHeight(List<TrapezoidNode> nodes)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            var trapezoid = nodes[i].Trapezoid;
            var height = Math.Abs(trapezoid.YT - trapezoid.YB);
            if (height > MinHeight)
            {
                continue;
            }

            var neighborNodeToMerge = nodes[i].AdjacencyList
                .Where(n =>
                {
                    return n.Trapezoid.YT == trapezoid.YB ||
                            n.Trapezoid.YB == trapezoid.YT;
                })
                .FirstOrDefault();

            if (neighborNodeToMerge is not TrapezoidNode neighborNode)
            {
                continue;
            }

            nodes[i].Trapezoid = MergeTrapezoids(nodes[i].Trapezoid, neighborNode.Trapezoid);
            for (var j = 0; j < neighborNode.AdjacencyList.Count; j++)
            {
                var sndDegreeNode = neighborNode.AdjacencyList[j];
                if (!sndDegreeNode.AdjacencyList.Contains(nodes[i]))
                {
                    sndDegreeNode.AdjacencyList.Add(nodes[i]);
                    nodes[i].AdjacencyList.Add(sndDegreeNode);
                }

                sndDegreeNode.AdjacencyList.Remove(neighborNode);
            }

            nodes[i].AdjacencyList.Remove(neighborNode);
            neighborNode.AdjacencyList = default;
            nodes.Remove(neighborNode);
            i--;
        }
    }

    private static void SplitTrapezoidsAlongWidth(List<TrapezoidNode> nodes)
    {

        for(var i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];
            var trapezoid = node.Trapezoid;
            var topWidth = Math.Abs(trapezoid.XTR - trapezoid.XTL);
            var bottomWidth = Math.Abs(trapezoid.XBR - trapezoid.XBL);
            var maxWidth = Math.Max(topWidth, bottomWidth);

            var numSplits = (int)Math.Ceiling(maxWidth / MaxEdgeSize);
            if (numSplits <= 1)
            {
                continue;
            }

            var addedTrapezoids = new List<TrapezoidNode>(numSplits);
            for (var j = 0; j < numSplits; j++)
            {
                var lerpFactor = (float)j / numSplits;
                var nextLerpFactor = (float)(j + 1) / numSplits;

                var newXTL = Lerp(trapezoid.XTL, trapezoid.XTR, lerpFactor);
                var newXTR = Lerp(trapezoid.XTL, trapezoid.XTR, nextLerpFactor);
                var newXBL = Lerp(trapezoid.XBL, trapezoid.XBR, lerpFactor);
                var newXBR = Lerp(trapezoid.XBL, trapezoid.XBR, nextLerpFactor);

                var newTrapezoid = new Trapezoid
                {
                    Id = trapezoid.Id, // You might need to generate new IDs
                    PathingMapId = trapezoid.PathingMapId,
                    YT = trapezoid.YT,
                    YB = trapezoid.YB,
                    XTL = newXTL,
                    XTR = newXTR,
                    XBL = newXBL,
                    XBR = newXBR
                };

                if (j == 0)
                {
                    node.Trapezoid = newTrapezoid;
                }
                else
                {
                    var newNode = new TrapezoidNode
                    {
                        Trapezoid = newTrapezoid,
                        AdjacencyList = []
                    };

                    addedTrapezoids.Add(newNode);
                }
            }

            // Build new node adjacency
            for(var j = 0; j < addedTrapezoids.Count; j++)
            {
                var addedTrapezoid = addedTrapezoids[j];
                if (j <  addedTrapezoids.Count - 1)
                {
                    var rightNeighbor = addedTrapezoids[j + 1];
                    addedTrapezoid.AdjacencyList.Add(rightNeighbor);
                    rightNeighbor.AdjacencyList.Add(addedTrapezoid);
                }

                var addedTrapezoidPoints = MathUtils.GetTrapezoidPoints(addedTrapezoid.Trapezoid);
                for(var k = 0; k < node.AdjacencyList.Count; k++)
                {
                    var neighbor = node.AdjacencyList[k];
                    var neighborPoints = MathUtils.GetTrapezoidPoints(neighbor.Trapezoid);
                    if (TrapezoidsAdjacent(addedTrapezoidPoints, neighborPoints))
                    {
                        addedTrapezoid.AdjacencyList.Add(neighbor);
                        neighbor.AdjacencyList.Add(addedTrapezoid);
                    }
                }
            }

            // Reconstruct current node adjacency
            var currentNodePoints = MathUtils.GetTrapezoidPoints(node.Trapezoid);
            for (var j = 0; j < node.AdjacencyList.Count; j++)
            {
                var neighbor = node.AdjacencyList[j];
                var neighborPoints = MathUtils.GetTrapezoidPoints(neighbor.Trapezoid);
                if (!TrapezoidsAdjacent(currentNodePoints, neighborPoints))
                {
                    node.AdjacencyList.RemoveAt(j);
                    j--;
                }
            }

            //Add the right sided trapezoid to the adjacency list
            node.AdjacencyList.Add(addedTrapezoids[0]);

            nodes.AddRange(addedTrapezoids);
        }
    }

    private static Rect GetBoundingRectangle(Point[] points)
    {
        var minX = double.MaxValue;
        var maxX = double.MinValue;
        var minY = double.MaxValue;
        var maxY = double.MinValue;
        for (var i = 0; i < points.Length; i++)
        {
            var curPoint = points[i];
            if (curPoint.X < minX) minX = curPoint.X;
            if (curPoint.X > maxX) maxX = curPoint.X;
            if (curPoint.Y < minY) minY = curPoint.Y;
            if (curPoint.Y > maxY) maxY = curPoint.Y;
        }

        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }

    private static Rect MergeRectangles(List<Rect> rectangles)
    {
        double minX = double.MaxValue, minY = double.MaxValue;
        double maxX = double.MinValue, maxY = double.MinValue;

        foreach (var rect in rectangles)
        {
            minX = Math.Min(minX, rect.Left);
            minY = Math.Min(minY, rect.Top);
            maxX = Math.Max(maxX, rect.Right);
            maxY = Math.Max(maxY, rect.Bottom);
        }

        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }

    private static double EuclideanDistance((double X, double Y) point1, (double X, double Y) point2)
    {
        return Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2);
    }

    private static (double X, double Y) CalculateCentroid(Trapezoid trapezoid)
    {
        var x = (trapezoid.XTL + trapezoid.XTR + trapezoid.XBL + trapezoid.XBR) / 4;
        var y = (trapezoid.YT + trapezoid.YB) / 2;
        return (x, y);
    }

    private static bool TrapezoidsAdjacent(Point[] currentPoints, Point[] otherPoints)
    {
        var curBoundingRectangle = GetBoundingRectangle(currentPoints);
        var otherBoundingRectangle = GetBoundingRectangle(otherPoints);
        if (!curBoundingRectangle.IntersectsWith(otherBoundingRectangle))
        {
            return false;
        }

        for (var x = 0; x < currentPoints.Length; x++)
        {
            for (var y = 0; y < otherPoints.Length; y++)
            {
                if (MathUtils.LineSegmentsIntersect(currentPoints[x], currentPoints[(x + 1) % currentPoints.Length],
                    otherPoints[y], otherPoints[(y + 1) % otherPoints.Length], out _, epsilon: 0.1))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static Rect CalculateIntersection(Rect rectA, Rect rectB)
    {
        var left = Math.Max(rectA.Left, rectB.Left);
        var right = Math.Min(rectA.Right, rectB.Right);
        var top = Math.Max(rectA.Top, rectB.Top); // Changed to Max
        var bottom = Math.Min(rectA.Bottom, rectB.Bottom); // Changed to Min

        // If there is no overlap, return an empty rectangle
        if (left >= right || top >= bottom)
        {
            return new Rect(0, 0, 0, 0);
        }

        return new Rect(left, top, right - left, bottom - top);
    }

    private static Trapezoid MergeTrapezoids(Trapezoid t1, Trapezoid t2)
    {
        float newYT = Math.Max(t1.YT, t2.YT);
        float newYB = Math.Min(t1.YB, t2.YB);

        // Determine the new X-coordinates. This might need more complex logic
        // based on how the trapezoids are positioned relative to each other
        float newXTL = Math.Min(t1.XTL, t2.XTL);
        float newXTR = Math.Max(t1.XTR, t2.XTR);
        float newXBL = Math.Min(t1.XBL, t2.XBL);
        float newXBR = Math.Max(t1.XBR, t2.XBR);

        // Create a new trapezoid with these boundaries
        return new Trapezoid
        {
            Id = t1.Id, // or some new ID if needed
            PathingMapId = t1.PathingMapId, // assuming this remains the same
            YT = newYT,
            YB = newYB,
            XTL = newXTL,
            XTR = newXTR,
            XBL = newXBL,
            XBR = newXBR
        };
    }

    private static Point Lerp(Point a, Point b, float t)
    {
        return new Point
        {
            X = a.X + ((b.X - a.X) * t),
            Y = a.Y + ((b.Y - a.Y) * t)
        };
    }

    private static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    private static Point? LineSegmentIntersectsTrapezoid(Point p1, Point p2, Trapezoid trapezoid)
    {
        var trapezoidPoints = MathUtils.GetTrapezoidPoints(trapezoid);
        for (var i = 0; i < trapezoidPoints.Length; i++)
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

    private static Point GetClosestPointInTrapezoid(Point startingPoint, Trapezoid destination)
    {
        var trapezoidPoints = MathUtils.GetTrapezoidPoints(destination);

        var closestPoint = new Point();
        var closestDistance = double.MaxValue;
        for (var i = 0; i < trapezoidPoints.Length - 1; i++)
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
}
