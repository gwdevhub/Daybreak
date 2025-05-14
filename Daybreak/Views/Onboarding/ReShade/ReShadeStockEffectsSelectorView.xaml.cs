using Daybreak.Shared.Models.ReShade;
using Daybreak.Shared.Services.ReShade;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Extensions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Onboarding.ReShade;
/// <summary>
/// Interaction logic for ReShadeStockEffectsSelectorView.xaml
/// </summary>
public partial class ReShadeStockEffectsSelectorView : UserControl
{
    private readonly IReShadeService reShadeService;

    [GenerateDependencyProperty]
    private bool loading;

    public ObservableCollection<ShaderPackage> Packages { get; set; } = [];

    public ReShadeStockEffectsSelectorView(
        IReShadeService reShadeService)
    {
        this.reShadeService = reShadeService.ThrowIfNull();
        this.InitializeComponent();
    }

    private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.Packages.ClearAnd().AddRange(await this.reShadeService.GetStockPackages(CancellationToken.None));
    }

    private async void HighlightButton_Clicked(object sender, System.EventArgs e)
    {
        if (sender is not FrameworkElement element ||
            element.DataContext is not ShaderPackage package)
        {
            return;
        }

        this.Loading = true;
        await this.reShadeService.InstallPackage(package, CancellationToken.None);
        await this.Dispatcher.InvokeAsync(() => this.Loading = false);
    }
}
