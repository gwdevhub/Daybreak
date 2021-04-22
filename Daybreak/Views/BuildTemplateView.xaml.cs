using Daybreak.Models.Builds;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.ViewManagement;
using System;
using System.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for BuildTemplatesView.xaml
    /// </summary>
    public partial class BuildTemplateView : UserControl
    {
        private const string DisallowedChars = "\r\n\\/.";

        public readonly static DependencyProperty CurrentBuildProperty =
            DependencyPropertyExtensions.Register<BuildTemplateView, BuildEntry>(nameof(CurrentBuild));
        public readonly static DependencyProperty SaveButtonEnabledProperty =
            DependencyPropertyExtensions.Register<BuildTemplateView, bool>(nameof(SaveButtonEnabled), new PropertyMetadata(false));

        private readonly IViewManager viewManager;
        private readonly IBuildTemplateManager buildTemplateManager;

        public bool SaveButtonEnabled
        {
            get => this.GetTypedValue<bool>(SaveButtonEnabledProperty);
            set => this.SetValue(SaveButtonEnabledProperty, value);
        }

        public BuildEntry CurrentBuild
        {
            get => this.GetTypedValue<BuildEntry>(CurrentBuildProperty);
            set => this.SetValue(CurrentBuildProperty, value);
        }

        public BuildTemplateView(
            IViewManager viewManager,
            IBuildTemplateManager buildTemplateManager)
        {
            this.buildTemplateManager = buildTemplateManager.ThrowIfNull(nameof(buildTemplateManager));
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.InitializeComponent();
            this.DataContextChanged += (sender, contextArgs) =>
            {
                if (contextArgs.NewValue is BuildEntry)
                {
                    this.CurrentBuild = contextArgs.NewValue.As<BuildEntry>();
                }
            };
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            this.viewManager.ShowView<BuildsListView>();
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            this.buildTemplateManager.SaveBuild(this.CurrentBuild);
            this.viewManager.ShowView<BuildsListView>();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (DisallowedChars.ToCharArray().Where(c => e.Text.Contains(c)).Any())
            {
                e.Handled = true;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(sender.As<TextBox>().Text))
            {
                this.SaveButtonEnabled = false;
            }
            else
            {
                this.SaveButtonEnabled = true;
            }
        }
    }
}
