using Daybreak.Services.Metrics;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Extensions.Services;
using System.Core.Extensions;
using System.Extensions;

namespace Daybreak.Services.Monitoring;

public sealed class MemoryUsageMonitor : IApplicationLifetimeService
{
    private const string MemoryUsage = "Memory Usage";
    private const string MemoryUsageUnit = "MBs";
    private const string MemoryUsageDescription = "MBs used by the launcher";

    private readonly Histogram<long> memoryUsageHistogram;
    private readonly Process currentProcess;

    private PerformanceCounter? memoryPerformanceCounter;

    public MemoryUsageMonitor(
        IMetricsService metricsService)
    {
        this.memoryUsageHistogram = metricsService.ThrowIfNull().CreateHistogram<long>(MemoryUsage, MemoryUsageUnit, MemoryUsageDescription, Models.Metrics.AggregationTypes.NoAggregate);
        this.currentProcess = Process.GetCurrentProcess();
    }

    public void OnClosing()
    {
    }

    public void OnStartup()
    {
        _ = Task.Run(this.StartupAndPeriodicallyReadMemoryUsage);
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
        if (this.memoryPerformanceCounter is not null)
        {
            this.memoryUsageHistogram.Record(this.memoryPerformanceCounter.RawValue / 1024);
        }

        await Task.Delay(1000);
        _ = Task.Run(this.PeriodicallyCheckMemoryUsagePerfCounterBased);
    }

    private async Task PeriodicallyCheckMemoryUsageProcessBased()
    {
        this.currentProcess.Refresh();
        this.memoryUsageHistogram.Record(this.currentProcess.PrivateMemorySize64 / 1000000);
        await Task.Delay(1000);
        _ = Task.Run(this.PeriodicallyCheckMemoryUsageProcessBased);
    }
}
