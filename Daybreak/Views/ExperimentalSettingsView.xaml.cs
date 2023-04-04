﻿using Daybreak.Configuration;
using Daybreak.Services.Navigation;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for ExperimentalSettingsView.xaml
/// </summary>
public partial class ExperimentalSettingsView : UserControl
{
    private readonly IViewManager viewManager;
        
    [GenerateDependencyProperty]
    private bool launchAsCurrentUser;
    [GenerateDependencyProperty]
    private bool multiLaunch;
    [GenerateDependencyProperty]
    private bool dynamicBuildLoading;
    [GenerateDependencyProperty]
    private bool macrosEnabled;
    [GenerateDependencyProperty]
    public string gWToolboxLaunchDelay = string.Empty;
    [GenerateDependencyProperty]
    public bool downloadIcons;
    [GenerateDependencyProperty]
    public bool focusViewEnabled;
    [GenerateDependencyProperty]
    private double memoryReaderFrequency;
    [GenerateDependencyProperty]
    private bool pathfindingEnabled;

    private readonly ILiveUpdateableOptions<ApplicationConfiguration> liveUpdateableOptions;

    public ExperimentalSettingsView(
        IViewManager viewManager,
        ILiveUpdateableOptions<ApplicationConfiguration> liveUpdateableOptions)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
        this.InitializeComponent();
        this.LoadExperimentalSettings();
    }

    private void LoadExperimentalSettings()
    {
        var config = this.liveUpdateableOptions.Value;
        this.MultiLaunch = config.ExperimentalFeatures.MultiLaunchSupport;
        this.GWToolboxLaunchDelay = config.ExperimentalFeatures.ToolboxAutoLaunchDelay.ToString();
        this.DynamicBuildLoading = config.ExperimentalFeatures.DynamicBuildLoading;
        this.LaunchAsCurrentUser = config.ExperimentalFeatures.LaunchGuildwarsAsCurrentUser;
        this.MacrosEnabled = config.ExperimentalFeatures.CanInterceptKeys;
        this.DownloadIcons = config.ExperimentalFeatures.DownloadIcons;
        this.FocusViewEnabled = config.ExperimentalFeatures.FocusViewEnabled;
        this.MemoryReaderFrequency = config.ExperimentalFeatures.MemoryReaderFrequency;
        this.PathfindingEnabled = config.ExperimentalFeatures.EnablePathfinding;
    }

    private void SaveExperimentalSettings()
    {
        var config = this.liveUpdateableOptions.Value;
        config.ExperimentalFeatures.MultiLaunchSupport = this.MultiLaunch;
        config.ExperimentalFeatures.DynamicBuildLoading = this.DynamicBuildLoading;
        config.ExperimentalFeatures.LaunchGuildwarsAsCurrentUser = this.LaunchAsCurrentUser;
        config.ExperimentalFeatures.CanInterceptKeys = this.MacrosEnabled;
        config.ExperimentalFeatures.DownloadIcons = this.DownloadIcons;
        config.ExperimentalFeatures.FocusViewEnabled = this.FocusViewEnabled;
        config.ExperimentalFeatures.MemoryReaderFrequency = this.MemoryReaderFrequency;
        config.ExperimentalFeatures.EnablePathfinding = this.PathfindingEnabled;
        if (int.TryParse(this.GWToolboxLaunchDelay, out var gwToolboxLaunchDelay))
        {
            config.ExperimentalFeatures.ToolboxAutoLaunchDelay = gwToolboxLaunchDelay;
        }

        this.liveUpdateableOptions.UpdateOption();
        this.viewManager.ShowView<LauncherView>();
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        this.SaveExperimentalSettings();
    }

    private void TextBox_AllowNumbersOnly(object sender, TextCompositionEventArgs e)
    {
        e.Handled = e.Text.Select(c => char.IsDigit(c)).All(result => result is true) is false;
    }

    private void TextBox_DisallowPaste(object sender, CanExecuteRoutedEventArgs e)
    {
        if (e.Command == ApplicationCommands.ContextMenu || e.Command == ApplicationCommands.Paste)
        {
            e.CanExecute = false;
            e.Handled = true;
        }
    }

    private void MemoryReaderLatencyBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (this.MemoryReaderFrequency < 16)
        {
            this.MemoryReaderFrequency = 16;
        }
        else if (this.MemoryReaderFrequency > 1000)
        {
            this.MemoryReaderFrequency = 1000;
        }
    }
}
