using Daybreak.Models.Guildwars;
using Daybreak.Services.TradeChat;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Core.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Controls.Templates;
/// <summary>
/// Interaction logic for BagItemMenu.xaml
/// </summary>
public partial class BagItemMenu : UserControl
{
    private readonly IItemHashService itemHashService;

    public event EventHandler? CloseClicked;
    public event EventHandler<ItemBase>? ItemWikiClicked;

    [GenerateDependencyProperty]
    private string modHash = string.Empty;

    public BagItemMenu()
        : this(Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IItemHashService>())
    {
    }

    private BagItemMenu(
        IItemHashService itemHashService)
    {
        this.itemHashService = itemHashService.ThrowIfNull();
        this.InitializeComponent();
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        this.CloseClicked?.Invoke(this, e);
    }

    private void UserControl_DataContextChanged(object _, DependencyPropertyChangedEventArgs e)
    {
        if (this.DataContext is not ItemBase item)
        {
            return;
        }

        this.ModHash = this.itemHashService.ComputeHash(item);
    }

    private void WikiTextBlock_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        if (this.DataContext is not ItemBase item)
        {
            return;
        }

        this.ItemWikiClicked?.Invoke(this, item);
    }
}
