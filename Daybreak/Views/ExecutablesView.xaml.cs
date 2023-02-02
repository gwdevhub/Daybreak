using Daybreak.Configuration;
using Daybreak.Controls;
using Daybreak.Models;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Extensions;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for ExecutablesView.xaml
    /// </summary>
    public partial class ExecutablesView : UserControl
    {
        private readonly ILiveUpdateableOptions<ApplicationConfiguration> liveUpdateableOptions;
        public ObservableCollection<GuildwarsPath> Paths { get; } = new();

        public ExecutablesView(
            ILiveUpdateableOptions<ApplicationConfiguration> liveUpdateableOptions)
        {
            this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull(nameof(liveUpdateableOptions));
            this.InitializeComponent();
            this.GetPaths();
        }

        private void GetPaths()
        {
            this.Paths.AddRange(this.liveUpdateableOptions.Value.GuildwarsPaths);
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            var newPath = new GuildwarsPath();
            this.Paths.Add(newPath);
            if (this.Paths.Count == 1)
            {
                this.SetPathAsDefault(newPath);
            }
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            this.liveUpdateableOptions.Value.GuildwarsPaths = this.Paths.ToList();
            this.liveUpdateableOptions.UpdateOption();
        }

        private void GuildwarsPathTemplate_DefaultClicked(object sender, EventArgs e)
        {
            var path = sender.As<GuildwarsPathTemplate>()?.DataContext?.As<GuildwarsPath>();
            if (path is null)
            {
                return;
            }

            this.SetPathAsDefault(path);
        }

        private void GuildwarsPathTemplate_RemoveClicked(object sender, EventArgs e)
        {
            var path = sender.As<GuildwarsPathTemplate>()?.DataContext?.As<GuildwarsPath>();
            if (path is null)
            {
                return;
            }

            this.Paths.Remove(path);
            if (this.Paths.Count > 0 && path.Default is true)
            {
                this.SetPathAsDefault(this.Paths.First());
            }
        }

        private void SetPathAsDefault(GuildwarsPath gwPath)
        {
            foreach (var path in this.Paths)
            {
                path.Default = false;
            }

            gwPath.Default = true;
            var view = CollectionViewSource.GetDefaultView(this.Paths);
            view.Refresh();
        }
    }
}
