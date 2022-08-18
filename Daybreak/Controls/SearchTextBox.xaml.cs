using System;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls
{
    /// <summary>
    /// Interaction logic for SearchTextBox.xaml
    /// </summary>
    public partial class SearchTextBox : UserControl
    {
        public event EventHandler<string> TextChanged;

        [GenerateDependencyProperty(InitialValue = true)]
        private bool placeholderVisibility;
        [GenerateDependencyProperty]
        private string searchText;

        public SearchTextBox()
        {
            this.InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = e.Source.As<TextBox>().Text;
            if (searchText.IsNullOrWhiteSpace())
            {
                this.PlaceholderVisibility = true;
            }
            else
            {
                this.PlaceholderVisibility = false;
            }

            this.SearchText = searchText;
            this.TextChanged?.Invoke(this, searchText);
        }
    }
}
