using Daybreak.Models;
using Daybreak.Services.ExecutableManagement;
using Daybreak.Services.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Daybreak.Views.Launch;

/// <summary>
/// Interaction logic for ExecutablesView.xaml
/// </summary>
public partial class ExecutablesView : UserControl
{
    private readonly IViewManager viewManager;
    private readonly IGuildWarsExecutableManager guildWarsExecutableManager;
    public ObservableCollection<ExecutablePath> Paths { get; } = [];

    public ExecutablesView(
        IViewManager viewManager,
        IGuildWarsExecutableManager guildWarsExecutableManager)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.guildWarsExecutableManager = guildWarsExecutableManager.ThrowIfNull();
        this.InitializeComponent();
        this.GetPaths();
    }

    private void GetPaths()
    {
        this.Paths.ClearAnd().AddRange(this.guildWarsExecutableManager.GetExecutableList().Select(s => new ExecutablePath { Path = s }));
    }

    private void AddButton_Clicked(object sender, EventArgs e)
    {
        this.Paths.Add(new ExecutablePath());
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        var executableList = this.guildWarsExecutableManager.GetExecutableList().ToList();
        var executablesToAdd = this.Paths.ExceptBy(executableList, e => e.Path).ToList();
        var executablesToRemove = executableList.Except(this.Paths.Select(e => e.Path)).ToList();
        foreach(var executable in executablesToAdd)
        {
            this.guildWarsExecutableManager.AddExecutable(executable.Path);
        }

        foreach(var executable in executablesToRemove)
        {
            this.guildWarsExecutableManager.RemoveExecutable(executable);
        }

        this.viewManager.ShowView<LauncherView>();
    }

    private void GuildwarsPathTemplate_RemoveClicked(object sender, EventArgs e)
    {
        if (sender is not FrameworkElement frameworkElement ||
            frameworkElement.DataContext is not ExecutablePath path)
        {
            return;
        }

        this.Paths.Remove(path);
    }
}
