using Daybreak.Models;
using Microsoft.Win32;
using System;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for GuildwarsPathTemplate.xaml
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
public partial class GuildwarsPathTemplate : UserControl
{
    public event EventHandler? RemoveClicked;

    [GenerateDependencyProperty]
    private string path = string.Empty;

    public GuildwarsPathTemplate()
    {
        this.InitializeComponent();
    }

    private void BinButton_Clicked(object sender, EventArgs e)
    {
        this.RemoveClicked?.Invoke(this, e);
    }

    private void FilePickerGlyph_Clicked(object sender, EventArgs e)
    {
        var filePicker = new OpenFileDialog()
        {
            CheckFileExists = true,
            CheckPathExists = true,
            DefaultExt = "exe",
            Multiselect = false
        };
        if (filePicker.ShowDialog() is true)
        {
            this.DataContext.As<ExecutablePath>()!.Path = filePicker.FileName;
        }
    }
}
