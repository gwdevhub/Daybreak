using Daybreak.Models.Guildwars;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media;

namespace Daybreak.Controls.Templates;

/// <summary>
/// Interaction logic for BagTemplate.xaml
/// </summary>
public partial class BagTemplate : UserControl
{
    private const int ItemsPerRow = 5;

    public event EventHandler<ItemBase>? ItemWikiClicked;
    public event EventHandler<ItemBase>? PriceHistoryClicked;
    public event EventHandler<IBagContent>? ItemClicked;

    [GenerateDependencyProperty]
    private string bagName = string.Empty;

    public BagTemplate()
    {
        this.InitializeComponent();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property == DataContextProperty)
        {
            if (this.DataContext is not Bag bag ||
                bag.Capacity > 100)
            {
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                this.Visibility = Visibility.Visible;
            }
        }
    }

    private void UserControl_DataContextChanged(object _, DependencyPropertyChangedEventArgs __)
    {
        if (this.DataContext is not Bag bag)
        {
            return;
        }

        if (!this.IsScreenVisible())
        {
            return;
        }

        if (bag.Capacity > 100)
        {
            return;
        }

        if (this.BagHolder.Children.Count != bag.Capacity)
        {
            this.SetupLayout(bag.Capacity);
        }

        var distinctItems = bag.Items.DistinctBy(i => i.Slot).ToList();
        for (var i = 0; i < this.BagHolder.Children.Count; i++)
        {
            if (this.BagHolder.Children[i] is not BagContentTemplate bagContentTemplate)
            {
                throw new InvalidOperationException($"Expected {this.BagName} to contain {nameof(BagContentTemplate)} at slot {i}");
            }

            var maybeItem = distinctItems.FirstOrDefault(item => (int)item.Slot == i);
            bagContentTemplate.DataContext = maybeItem ?? null;
        }
    }

    private void BagContentTemplate_ItemWikiClicked(object? _, ItemBase e)
    {
        this.ItemWikiClicked?.Invoke(this, e);
    }

    private void BagContentTemplate_PriceHistoryClicked(object? _, ItemBase e)
    {
        this.PriceHistoryClicked?.Invoke(this, e);
    }

    private void BagContentTemplate_ItemClicked(object? _, IBagContent e)
    {
        this.ItemClicked?.Invoke(this, e);
    }

    private void SetupLayout(int bagItemsCount)
    {
        this.BagHolder.RowDefinitions.Clear();
        this.BagHolder.ColumnDefinitions.Clear();
        foreach(BagContentTemplate child in this.BagHolder.Children)
        {
            child.ItemWikiClicked -= this.BagContentTemplate_ItemWikiClicked;
            child.PriceHistoryClicked -= this.BagContentTemplate_PriceHistoryClicked;
            child.ItemClicked -= this.BagContentTemplate_ItemClicked;
        }

        this.BagHolder.Children.Clear();
        var rows = Math.Ceiling((double)bagItemsCount / ItemsPerRow);
        for (var i = 0; i < ItemsPerRow; i++)
        {
            this.BagHolder.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }

        for (var i = 0; i < rows; i++)
        {
            this.BagHolder.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        }

        for (var i = 0; i < bagItemsCount; i++)
        {
            var currentIndex = this.BagHolder.Children.Count;
            var bagContentTemplate = new BagContentTemplate();
            Grid.SetColumn(bagContentTemplate, currentIndex % ItemsPerRow);
            Grid.SetRow(bagContentTemplate, currentIndex / ItemsPerRow);
            bagContentTemplate.VerticalAlignment = VerticalAlignment.Stretch;
            bagContentTemplate.HorizontalAlignment = HorizontalAlignment.Stretch;
            bagContentTemplate.ItemWikiClicked += this.BagContentTemplate_ItemWikiClicked;
            bagContentTemplate.PriceHistoryClicked += this.BagContentTemplate_PriceHistoryClicked;
            bagContentTemplate.ItemClicked += this.BagContentTemplate_ItemClicked;
            this.BagHolder.Children.Add(bagContentTemplate);
        }
    }

    private bool IsScreenVisible()
    {
        if (!this.IsVisible)
            return false;

        var container = VisualTreeHelper.GetParent(this) as FrameworkElement;
        if (container is null)
        {
            return false;
        }

        var bounds = this.TransformToAncestor(container).TransformBounds(new Rect(0, 0, this.RenderSize.Width, this.RenderSize.Height));
        var size = new Rect(0, 0, container.ActualWidth, container.ActualHeight);
        return size.IntersectsWith(bounds);
    }
}
