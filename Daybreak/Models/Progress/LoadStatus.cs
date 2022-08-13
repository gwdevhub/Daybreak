namespace Daybreak.Models.Progress
{
    public abstract class LoadStatus
    {
        public string Description { get; set; }
        public double Progress { get; set; }
        public LoadStatus(string description)
        {
            this.Description = description;
        }
    }
}
