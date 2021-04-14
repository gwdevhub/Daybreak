using Daybreak.Models;
using Microsoft.Win32;
using System;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls
{
    /// <summary>
    /// Interaction logic for GuildwarsPathTemplate.xaml
    /// </summary>
    public partial class GuildwarsPathTemplate : UserControl
    {
        public static readonly DependencyProperty PathProperty = DependencyPropertyExtensions.Register<GuildwarsPathTemplate, string>(nameof(Path));
        public static readonly DependencyProperty IsDefaultProperty = DependencyPropertyExtensions.Register<GuildwarsPathTemplate, bool>(nameof(IsDefault));

        public event EventHandler RemoveClicked;
        public event EventHandler DefaultClicked;

        public string Path
        {
            get => this.GetTypedValue<string>(PathProperty);
            set => this.SetValue(PathProperty, value);
        }
        public bool IsDefault
        {
            get => this.GetTypedValue<bool>(IsDefaultProperty);
            set => this.SetValue(IsDefaultProperty, value);
        }

        public GuildwarsPathTemplate()
        {
            this.InitializeComponent();
            this.DataContextChanged += GuildwarsPathTemplate_DataContextChanged;
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
            this.Path = sender.As<TextBox>()?.Text;
            this.DataContext.As<GuildwarsPath>().Path = this.Path;
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
                this.DataContext.As<GuildwarsPath>().Path = filePicker.FileName;
            }
        }
    }
}
