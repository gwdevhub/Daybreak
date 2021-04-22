using Daybreak.Models.Builds;
using System.Extensions;
using System.Windows.Controls;

namespace Daybreak.Controls
{
    /// <summary>
    /// Interaction logic for AttributeTemplate.xaml
    /// </summary>
    public partial class AttributeTemplate : UserControl
    {
        public AttributeTemplate()
        {
            InitializeComponent();
        }

        private void MinusButton_Clicked(object sender, System.EventArgs e)
        {
            if (this.DataContext.As<AttributeEntry>().Points > 0)
            {
                this.DataContext.As<AttributeEntry>().Points--;
            }
        }

        private void AddButton_Clicked(object sender, System.EventArgs e)
        {
            if (this.DataContext.As<AttributeEntry>().Points < 12)
            {
                this.DataContext.As<AttributeEntry>().Points++;
            }
        }
    }
}
