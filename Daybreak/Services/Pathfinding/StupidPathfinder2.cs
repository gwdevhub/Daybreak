using Daybreak.Models.Guildwars;
using Daybreak.Services.Pathfinding.Models;
using Daybreak.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Extensions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Daybreak.Services.Pathfinding;
internal sealed class StupidPathfinder2 : IPathfinder
{
    private const double PathStep = 1;
    private const double MinHeight = 50e1;
    private const double MaxEdgeSize = 1e6;

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
            map.NavMesh is not SuperOvercomplicatedNavmesh superOvercomplicatedNavmesh)
        {
            return new PathfindingFailure.UnexpectedFailure();
        }

        if (GetContainingTriangle(superOvercomplicatedNavmesh, startPoint) is not Triangle startTriangle)
        {
            return new PathfindingFailure.NoPathFound();
        }

        if (GetContainingTriangle(superOvercomplicatedNavmesh, endPoint) is not Triangle endTriangle)
        {
            return new PathfindingFailure.NoPathFound();
        }

        var pathList = GetTrianglePath2(superOvercomplicatedNavmesh, startTriangle, endTriangle);
        if (pathList is null)
        {
            return new PathfindingFailure.NoPathFound();
        }

        var pathfinding = new List<PathSegment>();
        var currentPoint = startPoint;
        var currentDirection = endPoint - currentPoint;
        currentDirection.Normalize();
        for (var i = 0; i < pathList.Count - 1; i++)
        {
            var currentTriangle = superOvercomplicatedNavmesh.Triangles[pathList[i]];
            var nextTriangle = superOvercomplicatedNavmesh.Triangles[pathList[i + 1]];
            var currentTrajectoryEndPoint = new Point(currentDirection.X * 10e6, currentDirection.Y * 10e6);
            if (LineSegmentIntersectsTriangle(currentPoint, currentTrajectoryEndPoint, currentTriangle) is not Point intersectionPoint)
            {
                return new PathfindingFailure.NoPathFound();
            }

            var validPoint = false;
            intersectionPoint += currentDirection * PathStep;
            for (var j = i + 1; j < pathList.Count; j++)
            {
                var subsequentTriangle = superOvercomplicatedNavmesh.Triangles[pathList[j]];
                if (PointInsideTriangle(subsequentTriangle, intersectionPoint))
                {
                    validPoint = true;
                    i = j - 1;
                    break;
                }
            }

            if (!validPoint)
            {
                var newCurrentPoint = GetClosestPointInTriangle(intersectionPoint, nextTriangle);
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
        //OptimizePath(superOvercomplicatedNavmesh, pathfinding);
        return new PathfindingResponse
        {
            Pathing = pathfinding
        };
    }

    private static SuperOvercomplicatedNavmesh? GenerateNavMeshInternal(List<Trapezoid> trapezoids, List<List<int>> computedAdjacencyList)
    {
        /*
         * Go over each triangle and populate its adjacency list with the neighbors of the trapezoid
         * they belonged to.
         * Since each trapezoid was split into 2 triangles, we can deduce the trapezoid they belonged to.
         * This way we can also deduce what are the triangles that the neighbor was split into as well.
         */
        var triangleList = new List<Triangle>();
        var triangleAdjacencyList = new List<List<int>>();
        for (var i = 0; i < trapezoids.Count; i++)
        {
            var trapezoid = trapezoids[i];
            var triangle1 = new Triangle(i * 2, new(trapezoid.XTL, trapezoid.YT), new(trapezoid.XBL, trapezoid.YB), new(trapezoid.XBR, trapezoid.YB));
            var triangle2 = new Triangle((i * 2) + 1, new(trapezoid.XBR, trapezoid.YB), new(trapezoid.XTL, trapezoid.YT), new(trapezoid.XTR, trapezoid.YT));
            triangleList.Add(triangle1);
            triangleList.Add(triangle2);
            triangleAdjacencyList.Add([]);
            triangleAdjacencyList.Add([]);
            triangleAdjacencyList[triangle1.Id].Add(triangle2.Id);
            triangleAdjacencyList[triangle2.Id].Add(triangle1.Id);
        }

        for(var i = 0; i < triangleList.Count; i++)
        {
            var triangle = triangleList[i];
            foreach(var neighboringId in computedAdjacencyList[i / 2])
            {
                var neighboringSubTriangle1Id = neighboringId * 2;
                var neighboringSubTriangle2Id = (neighboringId * 2) + 1;
                var neighboringSubTriangle1 = triangleList[neighboringSubTriangle1Id];
                var neighboringSubTriangle2 = triangleList[neighboringSubTriangle2Id];
                if (!triangleAdjacencyList[triangle.Id].Contains(neighboringSubTriangle1Id) &&
                    TrianglesAdjacent(triangle, neighboringSubTriangle1))
                {
                    triangleAdjacencyList[triangle.Id].Add(neighboringSubTriangle1Id);
                    triangleAdjacencyList[neighboringSubTriangle1Id].Add(triangle.Id);
                }

                if (!triangleAdjacencyList[triangle.Id].Contains(neighboringSubTriangle2Id) &&
                    TrianglesAdjacent(triangle, neighboringSubTriangle2))
                {
                    triangleAdjacencyList[triangle.Id].Add(neighboringSubTriangle2Id);
                    triangleAdjacencyList[neighboringSubTriangle2Id].Add(triangle.Id);
                }
            }
        }

        /*
         * Check each triangle for the length of its sides. If the length bigger than a limit, subdivide the triangle
         */
        var progressUpdate = 0;
        for (var i = 0; i < triangleList.Count; i++)
        {
            if (progressUpdate % 100 == 0)
            {
                var progress = ((double)i / triangleList.Count) * 10;
                var sb = new StringBuilder();
                for (var p = 0; p < 10; p++)
                {
                    if (p < progress)
                    {
                        sb.Append('#');
                    }
                    else
                    {
                        sb.Append('_');
                    }
                }

                Debug.WriteLine(sb.ToString());
            }

            progressUpdate++;

            var triangle = triangleList[i];
            var lines = new (Point, Point)[]
            {
                    (triangle.A, triangle.B),
                    (triangle.B, triangle.C),
                    (triangle.C, triangle.A),
            };

            // The following 3 ifs could probably be done more intelligently
            //if ((lines[0].Item1 - lines[0].Item2).LengthSquared > MaxEdgeSize)
            //{
            //    // Subdivide the triangle by splitting the long side in 2
            //    var d = new Point((triangle.A.X + triangle.B.X) / 2, (triangle.A.Y + triangle.B.Y) / 2);
            //    var subDividedTriangle1 = new Triangle(triangle.Id, triangle.A, d, triangle.C);
            //    var subDividedTriangle2 = new Triangle(triangleList.Count, triangle.B, d, triangle.C);
            //    AdjustAdjacency(subDividedTriangle1, subDividedTriangle2, triangleList, triangleAdjacencyList);

            //    // Retry the current triangle, until all sides are shortened enough
            //    i--;
            //    continue;
            //}
            if ((lines[1].Item1 - lines[1].Item2).LengthSquared > MaxEdgeSize)
            {
                // Subdivide the triangle by splitting the long side in 2
                var d = new Point((triangle.B.X + triangle.C.X) / 2, (triangle.B.Y + triangle.C.Y) / 2);
                var subDividedTriangle1 = new Triangle(triangle.Id, triangle.A, triangle.B, d);
                var subDividedTriangle2 = new Triangle(triangleList.Count, triangle.A, d, triangle.C);
                AdjustAdjacency(subDividedTriangle1, subDividedTriangle2, triangleList, triangleAdjacencyList);

                // Retry the current triangle, until all sides are shortened enough
                i--;
                continue;
            }
            //else if ((lines[2].Item1 - lines[2].Item2).LengthSquared > MaxEdgeSize)
            //{
            //    // Subdivide the triangle by splitting the long side in 2
            //    var d = new Point((triangle.C.X + triangle.A.X) / 2, (triangle.C.Y + triangle.A.Y) / 2);
            //    var subDividedTriangle1 = new Triangle(triangle.Id, triangle.C, d, triangle.B);
            //    var subDividedTriangle2 = new Triangle(triangleList.Count, triangle.A, d, triangle.B);
            //    AdjustAdjacency(subDividedTriangle1, subDividedTriangle2, triangleList, triangleAdjacencyList);

            //    // Retry the current triangle, until all sides are shortened enough
            //    i--;
            //    continue;
            //}
        }

        return new SuperOvercomplicatedNavmesh
        {
            AdjacencyList = triangleAdjacencyList,
            Triangles = triangleList
        };
    }

    private List<PathSegment> OptimizePath(SuperOvercomplicatedNavmesh navmesh, List<PathSegment> pathSegments)
    {
        var sw = Stopwatch.StartNew();
        /*
         * Optimize the final path by excluding redundant points.
         * Remove points to generate new paths, then walk these new paths, checking discrete points
         * that they are inside trapezoids.
         */
        var passes = 0;
        var nodesRemoved = 0;
        bool changed;
        do
        {
            changed = false;
            var increment = Math.Max(pathSegments.Count / 100, 1);
            for (var i = 0; i < pathSegments.Count - 1; i += increment)
            {
                var firstSegment = pathSegments[i];
                var secondSegment = pathSegments[i + 1];
                var newSegmentStart = firstSegment.StartPoint;
                var newSegmentEnd = secondSegment.EndPoint;
                var direction = newSegmentEnd - newSegmentStart;
                direction.Normalize();

                var valid = true;
                var newCurrentPoint = newSegmentStart;

                /*
                 * To improve performance, cache the current trapezoid. Very rarely the path crosses across multiple trapezoids. This way
                 */
                var maybeTriangle = GetContainingTriangle(navmesh, newCurrentPoint);
                while ((newSegmentEnd - newCurrentPoint).LengthSquared > 10000)
                {
                    newCurrentPoint += direction * 100;
                    if (maybeTriangle is Triangle triangle &&
                        !PointInsideTriangle(triangle, newCurrentPoint))
                    {
                        /*
                         * If the current point is outside of the trapezoid we were traversing, try to find the new trapezoid it traverses.
                         * First check the neighbors for the current trapezoid, then expand the search to all trapezoids
                         */
                        maybeTriangle = FindTrapezoidContainingPointFromNeighbors(navmesh, triangle, newCurrentPoint);
                        if (maybeTriangle is null)
                        {
                            maybeTriangle = GetContainingTriangle(navmesh, newCurrentPoint);
                        }
                    }

                    if (maybeTriangle is null)
                    {
                        valid = false;
                        break;
                    }
                }

                if (!valid)
                {
                    continue;
                }

                pathSegments.Remove(firstSegment);
                pathSegments.Remove(secondSegment);
                nodesRemoved++;
                pathSegments.Insert(i, new PathSegment { StartPoint = newSegmentStart, EndPoint = newSegmentEnd });
                i -= increment; // Stay on the same position to try and further optimize the current path segment
                changed = true;
            }

            passes++;
        } while (changed);

        return pathSegments;
    }

    private static Triangle? FindTrapezoidContainingPointFromNeighbors(SuperOvercomplicatedNavmesh navmesh, Triangle currentTriangle, Point currentPoint)
    {
        var neighbors = navmesh.AdjacencyList[currentTriangle.Id].Select(id => navmesh.Triangles[id]);
        foreach (var neighbor in neighbors)
        {
            if (PointInsideTriangle(neighbor, currentPoint))
            {
                return neighbor;
            }
        }

        return default;
    }

    private static Point GetClosestPointInTriangle(Point startingPoint, Triangle destination)
    {
        var trianglePoints = new Point[]
        {
            destination.A,
            destination.B,
            destination.C
        };

        var closestPoint = new Point();
        var closestDistance = double.MaxValue;
        for (var i = 0; i < trianglePoints.Length - 1; i++)
        {
            var closestPointToSegment = MathUtils.ClosestPointOnLineSegment(trianglePoints[i], trianglePoints[i + 1], startingPoint);
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

    private static List<int>? GetTrianglePath(SuperOvercomplicatedNavmesh navmesh, Triangle startTriangle, Triangle endTriangle)
    {
        var found = false;
        var visited = new int[navmesh.Triangles.Count];
        var visitationQueue = new Queue<Triangle>();
        visitationQueue.Enqueue(startTriangle);
        visited[startTriangle.Id] = (int)startTriangle.Id + 1;

        while (visitationQueue.TryDequeue(out var currentTrapezoid))
        {
            if (currentTrapezoid.Id == endTriangle.Id)
            {
                found = true;
                break;
            }

            foreach (var adjacentTrapezoidId in navmesh.AdjacencyList[currentTrapezoid.Id])
            {
                var nextTrapezoid = navmesh.Triangles[adjacentTrapezoidId];
                if (visited[adjacentTrapezoidId] > 0)
                {
                    continue;
                }

                visited[adjacentTrapezoidId] = currentTrapezoid.Id + 1;
                visitationQueue.Enqueue(nextTrapezoid);
            }
        }

        if (!found)
        {
            return default;
        }

        var backTrackingList = new List<int>();
        var currentTrapezoidId = endTriangle.Id;
        while (true)
        {
            backTrackingList.Add(currentTrapezoidId);
            if (currentTrapezoidId == startTriangle.Id)
            {
                backTrackingList.Reverse();
                return backTrackingList;
            }

            currentTrapezoidId = visited[currentTrapezoidId] - 1;
        }
    }

    private static List<int>? GetTrianglePath2(SuperOvercomplicatedNavmesh navmesh, Triangle startTriangle, Triangle endTriangle)
    {
        var visited = new Dictionary<int, (int Previous, double Distance)>();
        var priorityQueue = new PriorityQueue<Triangle, double>();
        var startCentroid = CalculateCentroid(startTriangle);
        var endCentroid = CalculateCentroid(endTriangle);
        var maxDistance = 10 * Distance(startCentroid, endCentroid); // 10 times straight-line distance

        priorityQueue.Enqueue(startTriangle, 0);
        visited[startTriangle.Id] = (-1, 0); // Mark as visited with initial distance

        while (priorityQueue.TryDequeue(out var currentTriangle, out var currentDistance))
        {
            if (currentDistance > maxDistance)
            {
                continue; // Skip paths that are too long
            }

            if (currentTriangle.Id == endTriangle.Id)
            {
                break; // Path found to the end triangle
            }

            foreach (var adjacentTriangleId in navmesh.AdjacencyList[currentTriangle.Id])
            {
                var nextTriangle = navmesh.Triangles[adjacentTriangleId];
                var nextCentroid = CalculateCentroid(nextTriangle);
                var distanceToEnd = Distance(nextCentroid, endCentroid);
                var totalDistance = currentDistance + 1 + distanceToEnd;

                if (!visited.TryGetValue(adjacentTriangleId, out var info) || totalDistance < info.Distance)
                {
                    visited[adjacentTriangleId] = (currentTriangle.Id, totalDistance);
                    priorityQueue.Enqueue(nextTriangle, totalDistance);
                }
            }
        }

        if (!visited.ContainsKey(endTriangle.Id))
        {
            return null; // No path found
        }

        // Backtrack to form the path
        var path = new List<int>();
        var currentId = endTriangle.Id;
        while (currentId != startTriangle.Id)
        {
            path.Add(currentId);
            currentId = visited[currentId].Previous;
        }
        path.Add(startTriangle.Id);
        path.Reverse();
        return path;
    }

    private static double Distance(Point p1, Point p2)
    {
        return (p1 - p2).LengthSquared;
    }

    private static Point? LineSegmentIntersectsTriangle(Point p1, Point p2, Triangle triangle)
    {
        var trianglePoints = new Point[]
        {
            triangle.A,
            triangle.B,
            triangle.C
        };
        for (var i = 0; i < trianglePoints.Length; i++)
        {
            var p3 = trianglePoints[i];
            var p4 = trianglePoints[(i + 1) % trianglePoints.Length];
            if (MathUtils.LineSegmentsIntersect(p1, p2, p3, p4, out var intersectionPoint, epsilon: 0.1))
            {
                return intersectionPoint;
            }
        }

        return default;
    }

    private static Triangle? GetContainingTriangle(SuperOvercomplicatedNavmesh navmesh, Point point)
    {
        foreach(var triangle in navmesh.Triangles)
        {
            if (PointInsideTriangle(triangle, point))
            {
                return triangle;
            }
        }

        return default;
    }

    private static Point CalculateCentroid(Triangle triangle)
    {
        var centroidX = (triangle.A.X + triangle.B.X + triangle.C.X) / 3;
        var centroidY = (triangle.A.Y + triangle.B.Y + triangle.C.Y) / 3;

        return new Point(centroidX, centroidY);
    }

    public static bool PointInsideTriangle(Triangle triangle, Point point)
    {
        var trapezoidPoints = new Point[]
        {
            triangle.A,
            triangle.B,
            triangle.C
        };

        // Check if the point is in any of the triangles of the quad. If true, then the point is in the quad.
        for (var i = 0; i < trapezoidPoints.Length; i++)
        {
            var a = trapezoidPoints[i];
            var b = trapezoidPoints[(i + 1) % trapezoidPoints.Length];
            var c = trapezoidPoints[(i + 2) % trapezoidPoints.Length];
            var v0 = c - a;
            var v1 = b - a;
            var v2 = point - a;

            var d00 = v0 * v0;
            var d01 = v0 * v1;
            var d02 = v0 * v2;
            var d11 = v1 * v1;
            var d12 = v1 * v2;

            var invDenom = 1 / ((d00 * d11) - (d01 * d01));
            var u = ((d11 * d02) - (d01 * d12)) * invDenom;
            var v = ((d00 * d12) - (d01 * d02)) * invDenom;

            if ((u > 0) && (v > 0) && (u + v < 1))
            {
                return true;
            }
        }

        return false;
    }

    private static void AdjustAdjacency(Triangle subDividedTriangle1, Triangle subDividedTriangle2, List<Triangle> triangleList, List<List<int>> triangleAdjacencyList)
    {
        // Replace the old triangle with the first of the new ones
        triangleList[subDividedTriangle1.Id] = subDividedTriangle1;

        // Create a new entry for the second new triangle
        triangleList.Add(subDividedTriangle2);
        triangleAdjacencyList.Add([]);

        // Connect the new triangles
        triangleAdjacencyList[subDividedTriangle1.Id].Add(subDividedTriangle2.Id);
        triangleAdjacencyList[subDividedTriangle2.Id].Add(subDividedTriangle1.Id);

        var toRemove = new List<int>();
        foreach (var oldNeighborId in triangleAdjacencyList[subDividedTriangle1.Id])
        {
            if (subDividedTriangle1.Id == oldNeighborId)
            {
                toRemove.Add(oldNeighborId);
                continue;
            }

            var oldNeighbor = triangleList[oldNeighborId];
            if (!TrianglesAdjacent(subDividedTriangle1, oldNeighbor))
            {
                // New triangle 1 is no longer adjacent to the old neighbor. Remove the existing adjacency
                // triangleAdjacencyList[subDividedTriangle1.Id].Remove(oldNeighborId); // Cannot remove directly since we can't change a collection during enumeration
                toRemove.Add(oldNeighbor.Id);
                triangleAdjacencyList[oldNeighborId].Remove(subDividedTriangle1.Id);
            }

            if (TrianglesAdjacent(subDividedTriangle2, oldNeighbor))
            {
                // New triangle 2 is adjacent to the old neighbor. Add the adjacency
                triangleAdjacencyList[subDividedTriangle2.Id].Add(oldNeighborId);
                triangleAdjacencyList[oldNeighborId].Add(subDividedTriangle2.Id);
            }
        }

        // Remove connections that are no longer valid
        foreach(var id in toRemove)
        {
            triangleAdjacencyList[subDividedTriangle1.Id].Remove(id);
        }
    }

    private static bool TrianglesAdjacent(Triangle triangle1, Triangle triangle2)
    {
        var lines1 = new (Point, Point)[3]
        {
            (triangle1.A, triangle1.B),
            (triangle1.B, triangle1.C),
            (triangle1.C, triangle1.A),
        };

        var lines2 = new (Point, Point)[3]
        {
            (triangle2.A, triangle2.B),
            (triangle2.B, triangle2.C),
            (triangle2.C, triangle2.A),
        };

        foreach(var line1 in lines1)
        {
            foreach(var line2 in lines2)
            {
                if (MathUtils.LineSegmentsIntersect(line1.Item1, line1.Item2, line2.Item1, line2.Item2, out _, epsilon: 0.1))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
