using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Core.Extensions;
using System.Extensions;
using Daybreak.Shared.Services.Metrics;
using Daybreak.Shared.Models.Metrics;
using Microsoft.Extensions.Hosting;

namespace Daybreak.Services.Monitoring;

internal sealed class DiskUsageMonitor(
    IMetricsService metricsService) : IHostedService
{
    private const string WriteDiskUsage = "Write Disk Usage";
    private const string WriteDiskUsageUnit = "MBs/s";
    private const string WriteDiskUsageDescription = "MBs/s written by Daybreak";
    private const string ReadDiskUsage = "Read Disk Usage";
    private const string ReadDiskUsageUnit = "MBs/s";
    private const string ReadDiskUsageDescription = "MBs/s read by Daybreak";

    private readonly Histogram<double> writeDiskUsageHistogram = metricsService.ThrowIfNull().CreateHistogram<double>(WriteDiskUsage, WriteDiskUsageUnit, WriteDiskUsageDescription, AggregationTypes.NoAggregate);
    private readonly Histogram<double> readDiskUsageHistogram = metricsService.ThrowIfNull().CreateHistogram<double>(ReadDiskUsage, ReadDiskUsageUnit, ReadDiskUsageDescription, AggregationTypes.NoAggregate);

    private PerformanceCounter? readPerformanceCounter;
    private PerformanceCounter? writePerformanceCounter;

    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        await this.StartupAndPeriodicallyReadDiskUsage(cancellationToken);
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task StartupAndPeriodicallyReadDiskUsage(CancellationToken cancellationToken)
    {
        var processName = Process.GetCurrentProcess().ProcessName;
        this.readPerformanceCounter = new PerformanceCounter("Process", "IO Read Bytes/sec", processName);
        this.writePerformanceCounter = new PerformanceCounter("Process", "IO Write Bytes/sec", processName);
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                this.PeriodicallyCheckMemoryUsagePerfCounterBased();
                await Task.Delay(1000, cancellationToken);
            }
            catch
            {
            }
        }
    }

    private void PeriodicallyCheckMemoryUsagePerfCounterBased()
    {
        this.readDiskUsageHistogram.Record(this.readPerformanceCounter?.NextValue() / 1024 / 1024 ?? 0);
        this.writeDiskUsageHistogram.Record(this.writePerformanceCounter?.NextValue() / 1024 / 1024 ?? 0);
    }
}
