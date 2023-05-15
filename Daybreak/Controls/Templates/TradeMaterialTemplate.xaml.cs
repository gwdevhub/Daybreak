using Daybreak.Models.Trade;
using Daybreak.Services.IconRetrieve;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Core.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Controls.Templates;
/// <summary>
/// Interaction logic for TradeMaterialTemplate.xaml
/// </summary>
public partial class TradeMaterialTemplate : UserControl
{
    private readonly IIconCache iconCache;

    [GenerateDependencyProperty]
    private ImageSource imageSource = default!;
    [GenerateDependencyProperty]
    private bool imageVisible;

    public TradeMaterialTemplate() :
        this(Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IIconCache>())
    {
    }

    public TradeMaterialTemplate(
        IIconCache iconCache)
    {
        this.iconCache = iconCache.ThrowIfNull();
        this.InitializeComponent();
    }

    private async void UserControl_DataContextChanged(object _, DependencyPropertyChangedEventArgs __)
    {
        if (this.DataContext is not TraderQuote traderQuote)
        {
            return;
        }

        var imageUri = await this.iconCache.GetIconUri(traderQuote.Item);
        if (imageUri is null)
        {
            this.ImageVisible = false;
            return;
        }

        this.ImageVisible = true;
        this.ImageSource = new BitmapImage(imageUri);
    }
}
