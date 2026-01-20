using Daybreak.Shared.Models.Metrics;
using Daybreak.Shared.Services.Metrics;
using Microsoft.Extensions.Hosting;
using System.Core.Extensions;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Daybreak.Services.Monitoring;

internal sealed class ProcessorUsageMonitor(
    IMetricsService metricsService) : IHostedService
{
    private const string ProcessorTime = "processor.usage";
    private const string ProcessorTimeUnit = "% CPU";
    private const string ProcessorTimeDescription = "Percentage of CPU used by Daybreak";
    
    private readonly Histogram<double> processorTimeHistogram = metricsService.ThrowIfNull().CreateHistogram<double>(ProcessorTime, ProcessorTimeUnit, ProcessorTimeDescription, AggregationTypes.NoAggregate);
    private readonly Process currentProcess = Process.GetCurrentProcess();
    private readonly int processorCount = Environment.ProcessorCount;

    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        await this.PeriodicallyCheckCPU(cancellationToken);
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task PeriodicallyCheckCPU(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var stopwatch = Stopwatch.StartNew();
            var startCpuUsage = this.currentProcess.TotalProcessorTime;
            await Task.Delay(1000, cancellationToken);

            var endCpuUsage = this.currentProcess.TotalProcessorTime;
            var elapsedTicks = stopwatch.ElapsedTicks;
            var usage = (double)(endCpuUsage - startCpuUsage).Ticks / (double)elapsedTicks / this.processorCount * 100d;

            this.processorTimeHistogram.Record(usage);
        }
    }
}
