using Daybreak.Configuration.Options;
using Daybreak.Models.Guildwars;
using Daybreak.Services.Metrics;
using Daybreak.Services.Pathfinding.Models;
using Microsoft.Extensions.Logging;
using SharpNav;
using SharpNav.Geometry;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Daybreak.Services.Pathfinding;
internal sealed class SharpNavPathfinder : IPathfinder
{
    public const double MaxSensitivity = 100d;
    public const double MinSensitivity = 1d;

    private const string PathfindingLatencyMetricName = "SharpNav Pathfinding Latency";
    private const string PathfindingLatencyMetricUnit = "Milliseconds";
    private const string PathfindingLatencyMetricDescription = "Amount of milliseconds elapsed while running the pathfinding algorithm. P95 aggregation";
    private const string MeshGenerationLatencyMetricName = "Pathfinding Mesh Generation Latency";
    private const string MeshGenerationLatencyMetricUnit = "Milliseconds";
    private const string MeshGenerationLatencyMetricDescription = "Amount of milliseconds elapsed while generating the pathfinding mesh. P95 aggregation";

    private readonly Histogram<double> meshLatencyMetric;
    private readonly Histogram<double> pathfindingLatencyMetric;
    private readonly ILiveOptions<PathfindingOptions> liveOptions;
    private readonly ILogger<SharpNavPathfinder> logger;

    public SharpNavPathfinder(
        IMetricsService metricsService,
        ILiveOptions<PathfindingOptions> liveOptions,
        ILogger<SharpNavPathfinder> logger)
    {
        this.pathfindingLatencyMetric = metricsService.ThrowIfNull().CreateHistogram<double>(PathfindingLatencyMetricName, PathfindingLatencyMetricUnit, PathfindingLatencyMetricDescription, Daybreak.Models.Metrics.AggregationTypes.P95);
        this.meshLatencyMetric = metricsService.ThrowIfNull().CreateHistogram<double>(MeshGenerationLatencyMetricName, MeshGenerationLatencyMetricUnit, MeshGenerationLatencyMetricDescription, Daybreak.Models.Metrics.AggregationTypes.P95);
        this.liveOptions = liveOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public async Task<Result<PathfindingResponse, PathfindingFailure>> CalculatePath(PathingData map, Point startPoint, Point endPoint, CancellationToken cancellationToken)
    {
        return await new TaskFactory().StartNew(() => this.CalculatePathInternal(map, startPoint, endPoint), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    public Task<NavMesh?> GenerateNavMesh(List<Trapezoid> trapezoids, CancellationToken cancellationToken)
    {
        /*
         * High sensitivity loads meshes in 2 - 10s. Low sensitivity generates in ~100 ms. Low sensitivity ignores small objects on the mesh.
         * High sensitivity increases memory usage exponentially. On a large map, high sensitvity uses 200mbs or so of RAM, while low sensitivity uses < 10 mbs.
         */
        var highSensitivity = this.liveOptions.Value.HighSensitivity;
        var settings = NavMeshGenerationSettings.Default;
        settings.CellSize = highSensitivity ? 60 : 200;
        settings.CellHeight = highSensitivity ? 60 : 200;
        settings.ContourFlags = ContourBuildFlags.None;
        settings.SampleDistance = highSensitivity ? 15 : 100;
        return new TaskFactory().StartNew(() => this.GenerateNavMesh(ConvertTrapezoidsToTriangles(trapezoids), settings), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    private Result<PathfindingResponse, PathfindingFailure> CalculatePathInternal(PathingData pathingData, Point startPoint, Point endPoint)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.CalculatePath), string.Empty);
        if (pathingData is null ||
            pathingData.NavMesh is null)
        {
            scopedLogger.LogError("Null pathfinding map");
            return new PathfindingFailure.UnexpectedFailure();
        }

        var sw = Stopwatch.StartNew();
        var query = new NavMeshQuery(pathingData.NavMesh, 2048);
        var startVec = new Vector3((float)startPoint.X, 0, (float)startPoint.Y);
        var endVec = new Vector3((float)endPoint.X, 0, (float)endPoint.Y);
        var extents = Vector3.One;
        var startPolyRef = query.FindNearestPoly(ref startVec, ref extents, out var nearestStartRef, out var nearestStartPt);
        var endPolyRef = query.FindNearestPoly(ref  endVec, ref extents, out var nearestEndRef, out var nearestEndPt);
        var path = new List<int>(2048);
        query.FindPath(nearestStartRef, nearestEndRef, ref nearestStartPt, ref nearestEndPt, path);
        if (path.Count == 0)
        {
            scopedLogger.LogError("Unable to find path");
            return new PathfindingFailure.NoPathFound();
        }

        var straightPath = new Vector3[path.Count * 2];
        var straightPathFlags = new int[path.Count * 2];
        var straightPathRefs = new int[path.Count * 2];
        var straightPathCount = 0;
        query.FindStraightPath(nearestStartPt, nearestEndPt, [.. path], path.Count, straightPath, straightPathFlags, straightPathRefs, ref straightPathCount, path.Count * 2, 0);
        if (straightPathCount == 0)
        {
            scopedLogger.LogError("Unable to find straight path");
            return new PathfindingFailure.NoPathFound();
        }

        var pathSegments = new List<PathSegment>();
        for (var i = 1; i < straightPathCount; i++)
        {
            pathSegments.Add(new PathSegment
            {
                StartPoint = new Point(straightPath[i - 1].X, straightPath[i - 1].Z),
                EndPoint = new Point(straightPath[i].X, straightPath[i].Z)
            });
        }

        this.pathfindingLatencyMetric.Record(sw.ElapsedMilliseconds);
        return new PathfindingResponse
        {
            Pathing = pathSegments
        };
    }

    private NavMesh? GenerateNavMesh(IEnumerable<Triangle3> triangles, NavMeshGenerationSettings settings)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GenerateNavMesh), string.Empty);
        var sw = Stopwatch.StartNew();
        try
        {
            BBox3 bounds = triangles.GetBoundingBox(settings.CellSize);
            var heightfield = new Heightfield(bounds, settings);

            heightfield.RasterizeTriangles(triangles);

            var compactHeightfield = new CompactHeightfield(heightfield, settings);

            compactHeightfield.Erode(settings.VoxelAgentWidth);
            compactHeightfield.BuildDistanceField();
            compactHeightfield.BuildRegions(2, settings.MinRegionSize, settings.MergedRegionSize);

            var contourSet = new ContourSet(compactHeightfield, settings);

            var polyMesh = new PolyMesh(contourSet, settings);
            var polyMeshDetail = new PolyMeshDetail(polyMesh, compactHeightfield, settings);

            var buildData = new NavMeshBuilder(polyMesh, polyMeshDetail, new SharpNav.Pathfinding.OffMeshConnection[0], settings);

            var navMesh = new NavMesh(buildData);
            this.meshLatencyMetric.Record(sw.ElapsedMilliseconds);
            return navMesh;
        }
        catch(Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception");
            return default;
        }
    }

    private static IEnumerable<Triangle3> ConvertTrapezoidsToTriangles(IEnumerable<Trapezoid>? trapezoids)
    {
        foreach (var trap in trapezoids ?? Enumerable.Empty<Trapezoid>())
        {
            var v1 = new Vector3(trap.XTL, 0, trap.YT); // Top-left
            var v2 = new Vector3(trap.XTR, 0, trap.YT); // Top-right
            var v3 = new Vector3(trap.XBR, 0, trap.YB); // Bottom-right
            var v4 = new Vector3(trap.XBL, 0, trap.YB); // Bottom-left

            // Create two triangles from the trapezoid
            yield return new Triangle3(v1, v2, v3);
            yield return new Triangle3(v1, v3, v4);
        }
    }
}
