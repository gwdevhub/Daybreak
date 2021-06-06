using System.Windows.Extensions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Extensions;

namespace Daybreak.Controls
{
    /// <summary>
    /// Interaction logic for LogTemplate.xaml
    /// </summary>
    public partial class LogMessageTemplate : UserControl
    {
        private bool expanded;

        [GenerateDependencyProperty]
        private string message;

        public LogMessageTemplate()
        {
            this.InitializeComponent();
        }

        private void TextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs eventArgs)
        {
            this.expanded = !this.expanded;
            if (this.expanded)
            {
                sender.As<TextBlock>().MaxHeight = double.MaxValue;
            }
            else
            {
                sender.As<TextBlock>().MaxHeight = 18;
            }
        }
    }
}
