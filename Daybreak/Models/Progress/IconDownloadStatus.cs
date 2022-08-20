using System.ComponentModel;

namespace Daybreak.Models.Progress
{
    public sealed class IconDownloadStatus : INotifyPropertyChanged
    {
        public static readonly IconDownloadStep StartingStep = new StartingIconDownloadStep();
        public static readonly IconDownloadStep Finished = new FinishedIconDownloadStep();
        public static readonly IconDownloadStep BrowserNotSupported = new NotSupportedIconDownloadStep();
        public static IconDownloadStep Checking(string iconName, double progress) => new CheckingIconDownloadStep(iconName, progress);
        public static IconDownloadStep Downloading(string iconName, double progress) => new DownloadingIconDownloadStep(iconName, progress);
        public static IconDownloadStep Stopped(double progress) => new StoppedIconDownloadStep(progress);

        private IconDownloadStep currentStep = StartingStep;

        public event PropertyChangedEventHandler PropertyChanged;

        public IconDownloadStep CurrentStep
        {
            get => this.currentStep;
            set
            {
                this.currentStep = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CurrentStep)));
            }
        }

        public abstract class IconDownloadStep : LoadStatus
        {
            public IconDownloadStep(string name, double progress) : base(name)
            {
                this.Progress = progress;
            }
        }

        public class StoppedIconDownloadStep : IconDownloadStep
        {
            public StoppedIconDownloadStep(double progress) : base("Download stopped", progress)
            {
            }
        }

        public class DownloadingIconDownloadStep : IconDownloadStep
        {
            public DownloadingIconDownloadStep(string skillName, double progress) : base($"Downloading [{skillName}] icon", progress)
            {
            }
        }

        public class CheckingIconDownloadStep : IconDownloadStep
        {
            public CheckingIconDownloadStep(string skillName, double progress) : base($"Checking [{skillName}] icon", progress)
            {
            }
        }

        public class NotSupportedIconDownloadStep : IconDownloadStep
        {
            public NotSupportedIconDownloadStep() : base("Cannot download icons. The WebView2 browser is not supported", 0d)
            {
            }
        }

        public class FinishedIconDownloadStep : IconDownloadStep
        {
            public FinishedIconDownloadStep() : base("Download finished", 100d)
            {
            }
        }

        public class StartingIconDownloadStep : IconDownloadStep
        {
            public StartingIconDownloadStep() : base("Download starting", 0d)
            {
            }
        }
    }
}
