using Daybreak.Configuration.Options;
using Daybreak.Shared.Services.Options;
using Microsoft.Extensions.Options;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;

public sealed class TelemetryViewModel(
    IOptionsProvider optionsProvider,
    IOptionsMonitor<TelemetryOptions> options)
    : ViewModelBase<TelemetryViewModel, TelemetryView>
{
    public const string DisclaimerText =
@"Telemetry Disclaimer
Daybreak uses optional telemetry to help improve performance, stability, and feature development. When enabled, the telemetry system collects anonymous, non-personally identifiable information through the OpenTelemetry framework and securely transmits it to our monitoring service.

Collected data may include:
    • Application version
    • View transitions and usage patterns (e.g., which features are accessed)
    • HTTP request metadata (e.g., domain targets, response status codes)
    • Unhandled exceptions and diagnostic stack traces
    • Performance metrics (e.g., memory usage, startup time)

What we don’t collect:
    • Your Guild Wars credentials
    • Your personal files or documents
    • Any identifying personal data
   
Telemetry is fully optional and can be turned off at any time. When disabled, no telemetry data is collected or transmitted.

For more information, you can view our source code or reach out to the maintainers.";

    private readonly IOptionsProvider optionsProvider = optionsProvider;
    private readonly IOptionsMonitor<TelemetryOptions> options = options;

    public bool TelemetryEnabled { get; set; }

    public override ValueTask ParametersSet(TelemetryView view, CancellationToken cancellationToken)
    {
        this.TelemetryEnabled = this.options.CurrentValue.Enabled;
        return base.ParametersSet(view, cancellationToken);
    }

    public void EnableTelemetry()
    {
        this.TelemetryEnabled = true;
        this.TelemetryEnabledChanged();
    }

    public void DisableTelemetry()
    {
        this.TelemetryEnabled = false;
        this.TelemetryEnabledChanged();
    }

    private void TelemetryEnabledChanged()
    {
        var options = this.options.CurrentValue;
        options.Enabled = this.TelemetryEnabled;
        this.optionsProvider.SaveOption(options);
        this.RefreshView();
    }
}
