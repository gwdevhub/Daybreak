using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Windows.Extensions.Services;
using System.Core.Extensions;
using System.Extensions;
using Daybreak.Shared.Services.Metrics;
using Daybreak.Shared.Models.Metrics;

namespace Daybreak.Services.Monitoring;

internal sealed class MemoryUsageMonitor(
    IMetricsService metricsService) : IApplicationLifetimeService
{
    private const string MemoryUsage = "Memory Usage";
    private const string MemoryUsageUnit = "MBs";
    private const string MemoryUsageDescription = "MBs used by Daybreak";

    private readonly Histogram<long> memoryUsageHistogram = metricsService.ThrowIfNull().CreateHistogram<long>(MemoryUsage, MemoryUsageUnit, MemoryUsageDescription, AggregationTypes.NoAggregate);
    private readonly Process currentProcess = Process.GetCurrentProcess();
    private readonly CancellationTokenSource cancellationTokenSource = new();

    private PerformanceCounter? memoryPerformanceCounter;

    public void OnClosing()
    {
        this.cancellationTokenSource.Cancel();
    }

    public void OnStartup()
    {
        _ = Task.Run(this.StartupAndPeriodicallyReadMemoryUsage, this.cancellationTokenSource.Token);
    }

    private async Task StartupAndPeriodicallyReadMemoryUsage()
    {
        var processName = this.currentProcess.ProcessName;
        try
        {
            this.memoryPerformanceCounter = new PerformanceCounter("Process", "Working Set - Private", processName);
            await this.PeriodicallyCheckMemoryUsagePerfCounterBased();
        }
        catch
        {
            await this.PeriodicallyCheckMemoryUsageProcessBased();
        }
    }

    private async Task PeriodicallyCheckMemoryUsagePerfCounterBased()
    {
        while (!this.cancellationTokenSource.IsCancellationRequested)
        {
            if (this.memoryPerformanceCounter is not null)
            {
                this.memoryUsageHistogram.Record(this.memoryPerformanceCounter.RawValue / 1024);
            }

            await Task.Delay(1000);
        }
    }

    private async Task PeriodicallyCheckMemoryUsageProcessBased()
    {
        while (!this.cancellationTokenSource.IsCancellationRequested)
        {
            this.currentProcess.Refresh();
            this.memoryUsageHistogram.Record(this.currentProcess.PrivateMemorySize64 / 1000000);
            await Task.Delay(1000);
        }
    }
}
