using Daybreak.Configuration.Options;
using Daybreak.Shared.Models.UMod;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.UMod;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Onboarding.UMod;
/// <summary>
/// Interaction logic for UModSwitchView.xaml
/// </summary>
public partial class UModMainView : UserControl
{
    private const string WikiLink = "https://code.google.com/archive/p/texmod/wikis/uMod.wiki";
    private const string ModsLink = "https://wiki.guildwars.com/wiki/Player-made_Modifications#Shared_player_content";

    private readonly IViewManager viewManager;
    private readonly IUModService uModService;
    private readonly ILiveOptions<UModOptions> liveOptions;
    private readonly Queue<Action> queuedSaveActions = new();

    [GenerateDependencyProperty]
    public bool uModEnabled;

    [GenerateDependencyProperty]
    public string currentVersion;

    public ObservableCollection<UModEntry> Mods { get; } = [];

    public UModMainView(
        IViewManager viewManager,
        IUModService uModService,
        ILiveOptions<UModOptions> liveOptions)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.uModService = uModService.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();

        this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        this.RefreshMods();
        this.UModEnabled = this.uModService.IsEnabled;
        this.CurrentVersion = this.uModService.Version.ToString();
    }

    private void SaveButton_Clicked(object _, EventArgs e)
    {
        this.uModService.SaveMods(this.Mods.ToList());
        this.uModService.IsEnabled = this.UModEnabled;
        while(this.queuedSaveActions.TryDequeue(out var action))
        {
            action();
        }

        this.viewManager.ShowView<LauncherView>();
    }

    private void HelpButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<UModBrowserView>(WikiLink);
    }

    private void BrowserButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<UModBrowserView>(ModsLink);
    }

    private void NavigateFileButton_Clicked(object sender, EventArgs e)
    {
        var filePicker = new OpenFileDialog
        {
            Filter = "Tpf or Zip Files (*.tpf;*.zip)|*.tpf;*.zip",
            Multiselect = true,
            RestoreDirectory = true,
            Title = "Please load a tpf or zip file"
        };
        if (filePicker.ShowDialog() is false)
        {
            return;
        }

        foreach(var name in filePicker.FileNames)
        {
            this.AddMod(name);
        }
    }

    private void BinButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not UserControl userControl ||
            userControl.DataContext is not UModEntry entry ||
            entry.PathToFile is not string pathToFile)
        {
            return;
        }

        this.Mods.Remove(entry);
        this.queuedSaveActions.Enqueue(() => this.uModService.RemoveMod(pathToFile));
    }

    private void AddMod(string fileName)
    {
        if (this.Mods.Any(m => m.PathToFile == Path.GetFullPath(fileName)))
        {
            return;
        }

        this.Mods.Add(
            new UModEntry
            {
                Name = Path.GetFileNameWithoutExtension(fileName),
                PathToFile = Path.GetFullPath(fileName),
                Enabled = this.liveOptions.Value.AutoEnableMods,
                Imported = true
            });

        this.queuedSaveActions.Enqueue(() => this.uModService.AddMod(fileName, imported: true));
    }

    private void RefreshMods()
    {
        this.Mods.ClearAnd().AddRange(this.uModService.GetMods());
    }

    private void UpButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not UserControl control ||
            control.DataContext is not UModEntry entry)
        {
            return;
        }
        
        var indexOfEntry = this.Mods.IndexOf(entry);
        // Ignore the mod that is first, since it cannot move up in the list
        if (indexOfEntry < 1 ||
            indexOfEntry > this.Mods.Count - 1)
        {
            return;
        }

        this.Mods.Remove(entry);
        this.Mods.Insert(indexOfEntry - 1, entry);
    }

    private void DownButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not UserControl control ||
            control.DataContext is not UModEntry entry)
        {
            return;
        }

        var indexOfEntry = this.Mods.IndexOf(entry);
        // Ignore the mod that is last, since it cannot move down in the list
        if (indexOfEntry < 0 ||
            indexOfEntry > this.Mods.Count - 2)
        {
            return;
        }

        this.Mods.Remove(entry);
        this.Mods.Insert(indexOfEntry + 1, entry);
    }
}
