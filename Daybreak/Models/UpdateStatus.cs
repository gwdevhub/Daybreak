using System.ComponentModel;

namespace Daybreak.Models
{
    public sealed class UpdateStatus : INotifyPropertyChanged
    {
        public static readonly UpdateStep StartingStep = new("Starting");
        public static readonly UpdateStep CheckingLatestVersion = new("Checking latest version");
        public static UpdateStep Downloading(double progress) => new DownloadUpdateStep("Downloading", progress);
        public static readonly UpdateStep DownloadFinished = new("Download finished. Application will restart in order to apply the update.");

        private UpdateStep currentStep = StartingStep;

        public event PropertyChangedEventHandler PropertyChanged;

        public UpdateStep CurrentStep
        {
            get => this.currentStep;
            set
            {
                this.currentStep = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentStep)));
            }
        }

        public class UpdateStep
        {
            public string Name { get; }
            internal UpdateStep(string name)
            {
                this.Name = name;
            }
        }
        public class DownloadUpdateStep : UpdateStep
        {
            internal DownloadUpdateStep(string name, double progress) : base(name)
            {
                this.Progress = progress;
            }

            public double Progress { get; }
        }
    }
}
