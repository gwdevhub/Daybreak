using Daybreak.Shared.Models.FocusView;
using Daybreak.Shared.Models.Guildwars;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Controls.FocusViewComponents;
/// <summary>
/// Interaction logic for TitleInformationComponent.xaml
/// </summary>
public partial class TitleInformationComponent : UserControl
{
    public event EventHandler<string>? NavigateToClicked;

    [GenerateDependencyProperty]
    private string titleRankName = string.Empty;

    public TitleInformationComponent()
    {
        this.InitializeComponent();
    }

    private void Title_MouseLeftButtonDown(object _, MouseButtonEventArgs __)
    {
        if (this.DataContext is not TitleInformationComponentContext context ||
            context.Title is not Title title ||
            title.WikiUrl is not string url)
        {
            return;
        }

        this.NavigateToClicked?.Invoke(this, url);
    }

    private void UserControl_DataContextChanged(object _, DependencyPropertyChangedEventArgs __)
    {
        if (this.DataContext is not TitleInformationComponentContext context)
        {
            return;
        }

        if (context.Title is not null &&
                context.Title.Tiers!.Count > context.TierNumber - 1)
        {
            var rankIndex = (int)context.TierNumber! - 1;
            this.TitleRankName = $"{context.Title.Tiers![rankIndex]} ({context.TierNumber}/{Math.Min(context.Title.Tiers.Count, context.MaxTierNumber)})";
        }
        else if (context.IsPercentage &&
            context.Title is not null)
        {
            this.TitleRankName = $"{context.Title.Name} ({context.CurrentPoints / 10d}%)";
        }
        else
        {
            this.TitleRankName = string.Empty;
        }
    }
}
