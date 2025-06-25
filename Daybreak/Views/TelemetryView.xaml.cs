using Daybreak.Configuration.Options;
using System.Configuration;
using System.Core.Extensions;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views;
/// <summary>
/// Interaction logic for TelemetryView.xaml
/// </summary>
public partial class TelemetryView : UserControl
{
    private const string DisclaimerTextDef =
@"Telemetry Disclaimer
Daybreak uses optional telemetry to help improve performance, stability, and feature development. When enabled, the telemetry system collects anonymous, non-personally identifiable information through the OpenTelemetry framework and securely transmits it to our monitoring service.

Collected data may include:
    • Application version
    • View transitions and usage patterns (e.g., which features are accessed)
    • HTTP request metadata (e.g., domain targets, response status codes)
    • Unhandled exceptions and diagnostic stack traces
    • Performance metrics (e.g., memory usage, startup time)

What we don’t collect:
    • Your Guild Wars credentials or chat logs
    • Your personal files or documents
    • Any identifying personal data
   
Telemetry is fully optional and can be turned off at any time in the settings. When disabled, no telemetry data is collected or transmitted.

For more information, you can view our source code or reach out to the maintainers.";

    private readonly ILiveUpdateableOptions<TelemetryOptions> liveUpdateableOptions;

    [GenerateDependencyProperty(InitialValue = DisclaimerTextDef)]
    private string disclaimerText = DisclaimerTextDef;

    [GenerateDependencyProperty]
    private bool telemetryEnabled;

    public TelemetryView(
        ILiveUpdateableOptions<TelemetryOptions> liveUpdateableOptions)
    {
        this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
        this.InitializeComponent();
        this.TelemetryEnabled = this.liveUpdateableOptions.Value.Enabled;
    }

    private void ToggleSwitch_Toggled(object sender, System.Windows.RoutedEventArgs e)
    {
        this.liveUpdateableOptions.Value.Enabled = this.TelemetryEnabled;
        this.liveUpdateableOptions.UpdateOption();
    }
}
