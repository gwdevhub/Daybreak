using Daybreak.Models.Builds;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.ViewManagement;
using System;
using System.Collections.ObjectModel;
using System.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for BuildsListView.xaml
    /// </summary>
    public partial class BuildsListView : UserControl
    {
        private readonly IViewManager viewManager;
        private readonly IBuildTemplateManager buildTemplateManager;

        public ObservableCollection<BuildEntry> BuildEntries { get; } = new ObservableCollection<BuildEntry>();

        public BuildsListView(
            IViewManager viewManager,
            IBuildTemplateManager buildTemplateManager)
        {
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.buildTemplateManager = buildTemplateManager.ThrowIfNull(nameof(buildTemplateManager));
            this.InitializeComponent();
            this.LoadBuilds();
        }

        private void LoadBuilds()
        {
            this.BuildEntries.ClearAnd().AddRange(this.buildTemplateManager.GetBuilds());
        }

        private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.viewManager.ShowView<BuildTemplateView>(sender.As<ListView>().SelectedItem);
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            this.viewManager.ShowView<SettingsCategoryView>();
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            var build = this.buildTemplateManager.CreateBuild();
            this.viewManager.ShowView<BuildTemplateView>(build);
        }

        private void BuildEntryTemplate_RemoveClicked(object sender, BuildEntry e)
        {
            this.buildTemplateManager.RemoveBuild(e);
            this.LoadBuilds();
        }
    }
}
