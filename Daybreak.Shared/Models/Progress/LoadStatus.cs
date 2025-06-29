namespace Daybreak.Shared.Models.Progress;

public abstract class LoadStatus(string description)
{
    public string Description { get; set; } = description;
    public double Progress { get; set; }
}
