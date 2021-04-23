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
    public partial class AttributeTemplate : UserControl
    {
        public readonly static DependencyProperty CanAddProperty =
            DependencyPropertyExtensions.Register<AttributeTemplate, bool>(nameof(CanAdd), new PropertyMetadata(false));
        public readonly static DependencyProperty CanSubtractProperty =
            DependencyPropertyExtensions.Register<AttributeTemplate, bool>(nameof(CanSubtract), new PropertyMetadata(false));

        public event EventHandler<AttributeEntry> HelpClicked;

        public bool CanAdd
        {
            get => this.GetTypedValue<bool>(CanAddProperty);
            private set => this.SetValue(CanAddProperty, value);
        }

        public bool CanSubtract
        {
            get => this.GetTypedValue<bool>(CanSubtractProperty);
            private set => this.SetValue(CanSubtractProperty, value);
        }

        public AttributeTemplate()
        {
            this.InitializeComponent();
            this.DataContextChanged += AttributeTemplate_DataContextChanged;
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
            }
        }

        private void AddButton_Clicked(object sender, System.EventArgs e)
        {
            if (this.DataContext.As<AttributeEntry>().Points < 12)
            {
                this.DataContext.As<AttributeEntry>().Points++;
                this.CanAdd = this.DataContext.As<AttributeEntry>().Points < 12;
                this.CanSubtract = true;
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
