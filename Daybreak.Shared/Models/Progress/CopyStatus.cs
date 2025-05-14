namespace Daybreak.Shared.Models.Progress;

public sealed class CopyStatus : ActionStatus
{
    public static readonly CopyStep InitializingCopy = new("Initializing copy");
    public static CopyProgressStep Copying(double progress) => new("Copying files", progress);
    public static readonly CopyStep CopyCancelled = new("Copy has been cancelled");
    public static readonly CopyStep CopyFinished = new("Copy has finished");
    public static readonly CopyStep CopyFailed = new("Copy has failed. Please check logs for details");

    public CopyStatus()
    {
        this.CurrentStep = InitializingCopy;
    }


    public class CopyStep : LoadStatus
    {
        internal CopyStep(string name) : base(name)
        {
        }
    }

    public sealed class CopyProgressStep : CopyStep
    {
        internal CopyProgressStep(string name, double progress) : base(name)
        {
            this.Progress = progress;
        }
    }
}
