using Daybreak.Services.Metrics;
using System;
using System.Core.Extensions;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.Monitoring;

internal sealed class ProcessorUsageMonitor : IApplicationLifetimeService
{
    private const string ProcessorTime = "Processor Usage";
    private const string ProcessorTimeUnit = "% CPU";
    private const string ProcessorTimeDescription = "Percentage of CPU used by Daybreak";
    
    private readonly Histogram<double> processorTimeHistogram;
    private readonly Process currentProcess;
    private readonly int processorCount;
    private readonly CancellationTokenSource cancellationTokenSource = new();

    public ProcessorUsageMonitor(
        IMetricsService metricsService)
    {
        this.processorTimeHistogram = metricsService.ThrowIfNull().CreateHistogram<double>(ProcessorTime, ProcessorTimeUnit, ProcessorTimeDescription, Models.Metrics.AggregationTypes.NoAggregate);
        this.currentProcess = Process.GetCurrentProcess();
        this.processorCount = Environment.ProcessorCount;
    }

    public void OnClosing()
    {
        this.cancellationTokenSource.Cancel();
    }

    public void OnStartup()
    {
        _ = Task.Run(this.PeriodicallyCheckCPU, this.cancellationTokenSource.Token);
    }

    private async Task PeriodicallyCheckCPU()
    {
        while (!this.cancellationTokenSource.IsCancellationRequested)
        {
            var stopwatch = Stopwatch.StartNew();
            var startCpuUsage = this.currentProcess.TotalProcessorTime;
            await Task.Delay(1000, this.cancellationTokenSource.Token);

            var endCpuUsage = this.currentProcess.TotalProcessorTime;
            var elapsedTicks = stopwatch.ElapsedTicks;
            var usage = (double)(endCpuUsage - startCpuUsage).Ticks / (double)elapsedTicks / this.processorCount * 100d;

            this.processorTimeHistogram.Record(usage);
        }
    }
}
