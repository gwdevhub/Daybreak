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
    public event EventHandler? DefaultClicked;

    [GenerateDependencyProperty]
    private string path = string.Empty;
    [GenerateDependencyProperty]
    private bool isDefault;

    public GuildwarsPathTemplate()
    {
        this.InitializeComponent();
        this.DataContextChanged += this.GuildwarsPathTemplate_DataContextChanged;
    }

    private void GuildwarsPathTemplate_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is GuildwarsPath guildwarsPath)
        {
            this.IsDefault = guildwarsPath.Default;
            this.Path = guildwarsPath.Path;
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        this.DataContext.As<GuildwarsPath>()!.Path = this.Path;
    }

    private void StarGlyph_Clicked(object sender, EventArgs e)
    {
        this.DefaultClicked?.Invoke(this, e);
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
            this.Path = filePicker.FileName;
            this.DataContext.As<GuildwarsPath>()!.Path = filePicker.FileName;
        }
    }
}
