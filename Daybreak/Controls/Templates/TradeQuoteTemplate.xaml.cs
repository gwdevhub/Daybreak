using Daybreak.Services.IconRetrieve;
using Daybreak.Services.Images;
using Daybreak.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Core.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media;

namespace Daybreak.Controls.Templates;
/// <summary>
/// Interaction logic for TradeQuoteTemplate.xaml
/// </summary>
public partial class TradeQuoteTemplate : UserControl
{
    private readonly IImageCache imageCache;
    private readonly IIconCache iconCache;

    [GenerateDependencyProperty]
    private ImageSource imageSource = default!;
    [GenerateDependencyProperty]
    private bool imageVisible;

    public TradeQuoteTemplate() :
        this(Global.GlobalServiceProvider.GetRequiredService<IImageCache>(),
            Global.GlobalServiceProvider.GetRequiredService<IIconCache>())
    {
    }

    public TradeQuoteTemplate(
        IImageCache imageCache,
        IIconCache iconCache)
    {
        this.imageCache = imageCache.ThrowIfNull();
        this.iconCache = iconCache.ThrowIfNull();
        this.InitializeComponent();
    }

    private async void UserControl_DataContextChanged(object _, DependencyPropertyChangedEventArgs __)
    {
        if (this.DataContext is not TraderQuoteModel traderQuote)
        {
            return;
        }

        var imageUri = await this.iconCache.GetIconUri(traderQuote.Item!);
        if (imageUri is null)
        {
            this.ImageVisible = false;
            return;
        }

        this.ImageVisible = true;
        this.ImageSource = await this.imageCache.GetImage(imageUri).ConfigureAwait(true);
    }
}
