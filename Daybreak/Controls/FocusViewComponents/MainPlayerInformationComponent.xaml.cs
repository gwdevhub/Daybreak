using Daybreak.Launch;
using Daybreak.Models.Guildwars;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Navigation;
using Daybreak.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Core.Extensions;
using System.Windows.Controls;

namespace Daybreak.Controls.FocusViewComponents;
/// <summary>
/// Interaction logic for MainPlayerInformationComponent.xaml
/// </summary>
public partial class MainPlayerInformationComponent : UserControl
{
    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly IViewManager viewManager;

    public event EventHandler<string>? NavigateToClicked;

    public MainPlayerInformationComponent()
        : this(
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IBuildTemplateManager>(),
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IViewManager>())
    {
    }

    public MainPlayerInformationComponent(
        IBuildTemplateManager buildTemplateManager,
        IViewManager viewManager)
    {
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
    }

    private void MetaBuilds_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.DataContext is not MainPlayerInformation mainPlayer)
        {
            return;
        }

        if (mainPlayer.PrimaryProfession is not null &&
            mainPlayer.PrimaryProfession != Profession.None &&
            mainPlayer.PrimaryProfession.BuildsUrl is string url)
        {
            this.NavigateToClicked?.Invoke(this, url);
        }
    }

    private void PrimaryProfession_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.DataContext is not MainPlayerInformation mainPlayer)
        {
            return;
        }

        if (mainPlayer.PrimaryProfession is not null &&
            mainPlayer.PrimaryProfession != Profession.None &&
            mainPlayer.PrimaryProfession.WikiUrl is string url)
        {
            this.NavigateToClicked?.Invoke(this, url);
        }
    }

    private void SecondaryProfession_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.DataContext is not MainPlayerInformation mainPlayer)
        {
            return;
        }

        if (mainPlayer.SecondaryProfession is not null &&
            mainPlayer.SecondaryProfession != Profession.None &&
            mainPlayer.SecondaryProfession.WikiUrl is string url)
        {
            this.NavigateToClicked?.Invoke(this, url);
        }
    }

    private void EditBuild_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.DataContext is not MainPlayerInformation mainPlayer)
        {
            return;
        }

        if (mainPlayer.CurrentBuild is Build build)
        {
            var buildEntry = this.buildTemplateManager.CreateSingleBuild();
            buildEntry.Primary = build.Primary;
            buildEntry.Secondary = build.Secondary;
            buildEntry.Attributes = build.Attributes;
            buildEntry.Skills = build.Skills;
            this.viewManager.ShowView<BuildTemplateView>(buildEntry);
        }
    }
}
