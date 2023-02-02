using Daybreak.Models.Builds;
using System;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls
{
    /// <summary>
    /// Interaction logic for AttributeTemplate.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
    public partial class AttributeTemplate : UserControl
    {
        public event EventHandler<AttributeEntry>? HelpClicked;
        public event EventHandler<AttributeEntry>? AttributeChanged;
        
        [GenerateDependencyProperty(InitialValue = false)]
        private bool canAdd;
        [GenerateDependencyProperty(InitialValue = false)]
        private bool canSubtract;

        public AttributeTemplate()
        {
            this.InitializeComponent();
            this.DataContextChanged += this.AttributeTemplate_DataContextChanged;
        }

        private void AttributeTemplate_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is AttributeEntry attributeEntry)
            {
                if (attributeEntry.Points > 0)
                {
                    this.CanSubtract = true;
                }

                if (attributeEntry.Points < 12)
                {
                    this.CanAdd = true;
                }
            }
        }

        private void MinusButton_Clicked(object sender, System.EventArgs e)
        {
            if (this.DataContext.As<AttributeEntry>().Points > 0)
            {
                this.DataContext.As<AttributeEntry>().Points--;
                this.CanSubtract = this.DataContext.As<AttributeEntry>().Points > 0;
                this.CanAdd = true;
                this.AttributeChanged?.Invoke(this, this.DataContext.As<AttributeEntry>());
            }
        }

        private void AddButton_Clicked(object sender, System.EventArgs e)
        {
            if (this.DataContext.As<AttributeEntry>().Points < 12)
            {
                this.DataContext.As<AttributeEntry>().Points++;
                this.CanAdd = this.DataContext.As<AttributeEntry>().Points < 12;
                this.CanSubtract = true;
                this.AttributeChanged?.Invoke(this, this.DataContext.As<AttributeEntry>());
            }
        }

        private void HelpButton_Clicked(object sender, System.EventArgs e)
        {
            if (this.DataContext is AttributeEntry attributeEntry)
            {
                this.HelpClicked?.Invoke(this, attributeEntry);
            }
        }
    }
}
