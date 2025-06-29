using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media.Imaging;

namespace Daybreak.Controls;
/// <summary>
/// Interaction logic for SnowfallOverlay.xaml
/// </summary>
public partial class SnowfallOverlay : UserControl
{
    private static readonly double[] Frequencies = [0.1, 0.025, 1, 0.25];
    private static readonly double[] Amplitudes = [1, 0.1, 0.1, 0.2];
    private static readonly double Divisor = Amplitudes.Sum();

    [GenerateDependencyProperty]
    private double flakeSize1;
    [GenerateDependencyProperty]
    private double flakeSize2;
    [GenerateDependencyProperty]
    private double flakeSize3;
    [GenerateDependencyProperty]
    private double flakeSize4;
    [GenerateDependencyProperty]
    private double flakeSize5;

    [GenerateDependencyProperty]
    private double time;

    [GenerateDependencyProperty]
    private double windStrength1;
    [GenerateDependencyProperty]
    private double windStrength2;
    [GenerateDependencyProperty]
    private double windStrength3;
    [GenerateDependencyProperty]
    private double windStrength4;
    [GenerateDependencyProperty]
    private double windStrength5;

    [GenerateDependencyProperty]
    private double baseWind1;
    [GenerateDependencyProperty]
    private double baseWind2;
    [GenerateDependencyProperty]
    private double baseWind3;
    [GenerateDependencyProperty]
    private double baseWind4;
    [GenerateDependencyProperty]
    private double baseWind5;

    private CancellationTokenSource? tokenSource;

    public SnowfallOverlay()
    {
        this.InitializeComponent();
        this.InitializeImages();
    }

    private void InitializeImages()
    {
        using var snowTexture1 = this.GetType().Assembly.GetManifestResourceStream("Daybreak.Resources.Snow1.png");
        using var snowTexture2 = this.GetType().Assembly.GetManifestResourceStream("Daybreak.Resources.Snow2.png");
        using var snowTexture3 = this.GetType().Assembly.GetManifestResourceStream("Daybreak.Resources.Snow3.png");
        using var snowTexture4 = this.GetType().Assembly.GetManifestResourceStream("Daybreak.Resources.Snow4.png");
        var bitmap1 = BitmapFactory.FromStream(snowTexture1);
        var bitmap2 = BitmapFactory.FromStream(snowTexture2);
        var bitmap3 = BitmapFactory.FromStream(snowTexture3);
        var bitmap4 = BitmapFactory.FromStream(snowTexture4);
        this.Host1.ImageSource = bitmap4;
        this.Host2.ImageSource = bitmap3;
        this.Host3.ImageSource = bitmap2;
        this.Host4.ImageSource = bitmap1;
        this.Host5.ImageSource = bitmap1;
    }

    private async void SimulateWind(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var time = this.Time;
            this.SnowfallTransform1.X += (this.BaseWind1 + (this.GetNoise(time) * this.WindStrength1)) / this.FlakeSize1;
            this.SnowfallTransform2.X += (this.BaseWind2 + (this.GetNoise(time - 0.02) * this.WindStrength2)) / this.FlakeSize2;
            this.SnowfallTransform3.X += (this.BaseWind3 + (this.GetNoise(time - 0.03) * this.WindStrength3)) / this.FlakeSize3;
            this.SnowfallTransform4.X += (this.BaseWind4 + (this.GetNoise(time - 0.05) * this.WindStrength4)) / this.FlakeSize4;
            this.SnowfallTransform5.X += (this.BaseWind5 + (this.GetNoise(time - 0.08) * this.WindStrength5)) / this.FlakeSize5;
            await Task.Delay(16, cancellationToken).ConfigureAwait(true);
        }
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        this.tokenSource?.Dispose();
        this.tokenSource = new CancellationTokenSource();
        this.SimulateWind(this.tokenSource.Token);
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        this.tokenSource?.Dispose();
        this.tokenSource = default;
    }

    private double GetNoise(double source)
    {
        var returnValue = 0d;
        for(var i = 0; i < Frequencies.Length; i++)
        {
            var f = Frequencies[i];
            var a = Amplitudes[i];
            returnValue += a * Math.Sin(f * source * Math.PI);
        }

        return returnValue / Divisor;
    }
}
