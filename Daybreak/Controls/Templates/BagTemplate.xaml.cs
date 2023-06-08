using Daybreak.Models.Guildwars;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls.Templates;

/// <summary>
/// Interaction logic for BagTemplate.xaml
/// </summary>
public partial class BagTemplate : UserControl
{
    private const int ItemsPerRow = 5;

    [GenerateDependencyProperty]
    private string bagName = string.Empty;

    public BagTemplate()
    {
        this.InitializeComponent();
    }

    private void UserControl_DataContextChanged(object _, DependencyPropertyChangedEventArgs __)
    {
        this.Visibility = Visibility.Hidden;
        if (this.DataContext is not Bag bag)
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

        this.Visibility = Visibility.Visible;
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

    private void SetupLayout(int bagItemsCount)
    {
        this.BagHolder.RowDefinitions.Clear();
        this.BagHolder.ColumnDefinitions.Clear();
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
            this.BagHolder.Children.Add(bagContentTemplate);
        }
    }
}
