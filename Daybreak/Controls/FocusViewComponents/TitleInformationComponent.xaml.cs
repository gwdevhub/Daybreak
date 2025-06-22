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
    private int pointsInCurrentRank;
    [GenerateDependencyProperty]
    private int pointsForNextRank;
    [GenerateDependencyProperty]
    private bool titleActive;
    [GenerateDependencyProperty]
    private string titleText = string.Empty;

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

        this.TitleActive = context.Title is not null;

        if (context.Title is not null)
        {
            if (context.MaxTierNumber == context.TierNumber ||
                context.PointsForCurrentRank == context.PointsForNextRank)
            {
                this.PointsInCurrentRank = (int)context.CurrentPoints;
                this.PointsForNextRank = (int)context.CurrentPoints;
            }
            else if (context.IsPercentage is false)
            {
                this.PointsInCurrentRank = (int)((uint)context.CurrentPoints - (uint)context.PointsForCurrentRank);
                this.PointsForNextRank = (int)((uint)context.PointsForNextRank - (uint)context.PointsForCurrentRank);
            }
            else
            {

                this.PointsInCurrentRank = (int)(uint)context.CurrentPoints;
                this.PointsForNextRank = (int)(uint)context.PointsForNextRank;
            }
        }

        this.UpdateTitleText();
    }

    private void UpdateTitleText()
    {
        if (this.DataContext is not TitleInformationComponentContext context)
        {
            return;
        }

        if (context.IsPercentage is true)
        {
            this.TitleText = $"{(double?)context.CurrentPoints / 10d}% Rank Progress";
        }
        else
        {
            this.TitleText = $"{context.CurrentPoints}/{context.PointsForNextRank} Rank Progress";
        }
    }
}
