using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Core.Extensions;
using System.Extensions;
using Daybreak.Shared.Services.Metrics;
using Daybreak.Shared.Models.Metrics;
using Microsoft.Extensions.Hosting;

namespace Daybreak.Services.Monitoring;

internal sealed class MemoryUsageMonitor(
    IMetricsService metricsService) : IHostedService
{
    private const string MemoryUsage = "memory.usage";
    private const string MemoryUsageUnit = "MBs";
    private const string MemoryUsageDescription = "MBs used by Daybreak";

    private readonly Histogram<long> memoryUsageHistogram = metricsService.ThrowIfNull().CreateHistogram<long>(MemoryUsage, MemoryUsageUnit, MemoryUsageDescription, AggregationTypes.NoAggregate);
    private readonly Process currentProcess = Process.GetCurrentProcess();

    private PerformanceCounter? memoryPerformanceCounter;

    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        await this.StartupAndPeriodicallyReadMemoryUsage(cancellationToken);
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task StartupAndPeriodicallyReadMemoryUsage(CancellationToken cancellationToken)
    {
        var processName = this.currentProcess.ProcessName;
        try
        {
            this.memoryPerformanceCounter = new PerformanceCounter("Process", "Working Set - Private", processName);
            await this.PeriodicallyCheckMemoryUsagePerfCounterBased(cancellationToken);
        }
        catch
        {
            await this.PeriodicallyCheckMemoryUsageProcessBased(cancellationToken);
        }
    }

    private async Task PeriodicallyCheckMemoryUsagePerfCounterBased(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (this.memoryPerformanceCounter is not null)
            {
                this.memoryUsageHistogram.Record(this.memoryPerformanceCounter.RawValue / 1024);
            }

            await Task.Delay(1000, cancellationToken);
        }
    }

    private async Task PeriodicallyCheckMemoryUsageProcessBased(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            this.currentProcess.Refresh();
            this.memoryUsageHistogram.Record(this.currentProcess.PrivateMemorySize64 / 1000000);
            await Task.Delay(1000, cancellationToken);
        }
    }
}
