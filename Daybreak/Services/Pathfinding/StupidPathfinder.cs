using Daybreak.Configuration;
using Daybreak.Models.Guildwars;
using Daybreak.Services.Metrics;
using Daybreak.Services.Pathfinding.Models;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Daybreak.Services.Pathfinding;

/// <summary>
/// Pathfinder based on Euclidean distance with discrete space.
/// </summary>
public sealed class StupidPathfinder : IPathfinder
{
    private const string LatencyMetricName = "Pathfinding Latency";
    private const string LatencyMetricUnit = "Milliseconds";
    private const string LatencyMetricDescription = "Amount of milliseconds elapsed while running the pathfinding algorithm. P95 aggregation";
    private const double PathStep = 1;

    private readonly Histogram<long> latencyMetric;
    private readonly ILiveOptions<ApplicationConfiguration> liveOptions;
    private readonly ILogger<StupidPathfinder> logger;

    public StupidPathfinder(
        IMetricsService metricsService,
        ILiveOptions<ApplicationConfiguration> liveOptions,
        ILogger<StupidPathfinder> logger)
    {
        this.liveOptions = liveOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.latencyMetric = metricsService.ThrowIfNull().CreateHistogram<long>(LatencyMetricName, LatencyMetricUnit, LatencyMetricDescription, Daybreak.Models.Metrics.AggregationTypes.P95);
    }

    public Task<Result<PathfindingResponse, PathfindingFailure>> CalculatePath(PathingData map, Point startPoint, Point endPoint, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() =>
        {
            var scopedLogger = this.logger.CreateScopedLogger(nameof(this.CalculatePath), string.Empty);
            try
            {
                var sw = Stopwatch.StartNew();
                var result = this.CalculatePathInternal(map, startPoint, endPoint);
                var ms = sw.ElapsedMilliseconds;
                this.latencyMetric.Record(ms);
                return result;
            }
            catch(Exception e)
            {
                scopedLogger.LogError(e, "Encountered exception during pathfinding");
            }

            return new PathfindingFailure.UnexpectedFailure();
        }, cancellationToken);
    }

    private Result<PathfindingResponse, PathfindingFailure> CalculatePathInternal(PathingData map, Point startPoint, Point endPoint)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.CalculatePath), string.Empty);
        if (this.liveOptions.Value.ExperimentalFeatures.EnablePathfinding is false)
        {
            scopedLogger.LogInformation("Pathfinding is disabled");
            return new PathfindingFailure.PathfindingDisabled();
        }

        if (GetContainingTrapezoid(map, startPoint) is not Trapezoid startTrapezoid)
        {
            scopedLogger.LogInformation("Start point not in map. Getting closest start point in map");
            (var maybeClosestPoint, var maybeClosestTrapezoid) = GetClosestTrapezoidAndInnerPointToPoint(map.Trapezoids, startPoint);
            if (maybeClosestPoint is not Point newStartPoint ||
                maybeClosestTrapezoid is not Trapezoid newStartTrapezoid)
            {
                scopedLogger.LogError("Unable to find closest start point in map");
                return new PathfindingFailure.UnexpectedFailure();
            }

            startPoint = newStartPoint;
            startTrapezoid = newStartTrapezoid;
        }

        if (GetContainingTrapezoid(map, endPoint) is not Trapezoid endTrapezoid)
        {
            scopedLogger.LogInformation("End point not in map. Getting closest end point in map");
            (var maybeClosestPoint, var maybeClosestTrapezoid) = GetClosestTrapezoidAndInnerPointToPoint(map.Trapezoids, endPoint);
            if (maybeClosestPoint is not Point newEndPoint ||
                maybeClosestTrapezoid is not Trapezoid newEndTrapezoid)
            {
                scopedLogger.LogError("Unable to find closest end point in map");
                return new PathfindingFailure.UnexpectedFailure();
            }

            endPoint = newEndPoint;
            endTrapezoid = newEndTrapezoid;
        }

        if (GetTrapezoidPath(map, startTrapezoid, endTrapezoid) is not List<int> pathList)
        {
            scopedLogger.LogInformation("Unable to find an initial path");
            return new PathfindingFailure.NoPathFound();
        }

        var pathfinding = new List<PathSegment>();
        var currentPoint = startPoint;
        var currentDirection = endPoint - currentPoint;
        currentDirection.Normalize();
        for (var i = 0; i < pathList.Count - 1; i++)
        {
            var currentTrapezoid = map.Trapezoids[pathList[i]];
            var nextTrapezoid = map.Trapezoids[pathList[i + 1]];
            var currentTrajectoryEndPoint = new Point(currentDirection.X * 10e6, currentDirection.Y * 10e6);
            if (LineSegmentIntersectsTrapezoid(currentPoint, currentTrajectoryEndPoint, currentTrapezoid) is not Point intersectionPoint)
            {
                scopedLogger.LogInformation("Unable to find an optimal path");
                return new PathfindingFailure.NoPathFound();
            }

            var validPoint = false;
            intersectionPoint += currentDirection * PathStep;
            for (var j = i + 1; j < pathList.Count; j++)
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
        if (map.Trapezoids is null)
        {
            return default;
        }

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
        var found = false;
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

            foreach(var adjacentTrapezoidId in map.ComputedAdjacencyList[currentTrapezoid.Id])
            {
                var nextTrapezoid = map.Trapezoids[adjacentTrapezoidId];
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
        var trapezoidPoints = MathUtils.GetTrapezoidPoints(destination);

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
        var trapezoidPoints = MathUtils.GetTrapezoidPoints(trapezoid);
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

    private static (Point? ClosestPoint, Trapezoid? ClosestTrapezoid) GetClosestTrapezoidAndInnerPointToPoint(List<Trapezoid> trapezoids, Point point)
    {
        var distance = double.MaxValue;
        var closestPoint = (Point?) null;
        var closestTrapezoid = (Trapezoid?) null;
        foreach(var trapezoid in trapezoids)
        {
            var p = GetClosestPointInTrapezoid(point, trapezoid);
            var d = (p - point).LengthSquared;
            if (d < distance)
            {
                closestPoint = p;
                distance = d;
                closestTrapezoid = trapezoid;
            }
        }

        return (closestPoint, closestTrapezoid);
    }
}
