using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Windows.Extensions.Services;
using System.Core.Extensions;
using System.Extensions;
using Daybreak.Shared.Services.Metrics;
using Daybreak.Shared.Models.Metrics;

namespace Daybreak.Services.Monitoring;

internal sealed class DiskUsageMonitor : IApplicationLifetimeService
{
    private const string WriteDiskUsage = "Write Disk Usage";
    private const string WriteDiskUsageUnit = "MBs/s";
    private const string WriteDiskUsageDescription = "MBs/s written by Daybreak";
    private const string ReadDiskUsage = "Read Disk Usage";
    private const string ReadDiskUsageUnit = "MBs/s";
    private const string ReadDiskUsageDescription = "MBs/s read by Daybreak";

    private readonly Histogram<double> writeDiskUsageHistogram;
    private readonly Histogram<double> readDiskUsageHistogram;
    private readonly CancellationTokenSource cancellationTokenSource = new();

    private PerformanceCounter? readPerformanceCounter;
    private PerformanceCounter? writePerformanceCounter;

    public DiskUsageMonitor(
        IMetricsService metricsService)
    {
        this.writeDiskUsageHistogram = metricsService.ThrowIfNull().CreateHistogram<double>(WriteDiskUsage, WriteDiskUsageUnit, WriteDiskUsageDescription, AggregationTypes.NoAggregate);
        this.readDiskUsageHistogram = metricsService.ThrowIfNull().CreateHistogram<double>(ReadDiskUsage, ReadDiskUsageUnit, ReadDiskUsageDescription, AggregationTypes.NoAggregate);
    }

    public void OnClosing()
    {
        this.cancellationTokenSource.Cancel();
    }

    public void OnStartup()
    {
        _ = Task.Run(this.StartupAndPeriodicallyReadDiskUsage, this.cancellationTokenSource.Token);
    }

    private async Task StartupAndPeriodicallyReadDiskUsage()
    {
        var processName = Process.GetCurrentProcess().ProcessName;
        this.readPerformanceCounter = new PerformanceCounter("Process", "IO Read Bytes/sec", processName);
        this.writePerformanceCounter = new PerformanceCounter("Process", "IO Write Bytes/sec", processName);
        while (!this.cancellationTokenSource.Token.IsCancellationRequested)
        {
            try
            {
                this.PeriodicallyCheckMemoryUsagePerfCounterBased();
                await Task.Delay(1000, this.cancellationTokenSource.Token);
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
