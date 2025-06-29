using System.Core.Extensions;

namespace Daybreak.Services.Toolbox.Models;
internal abstract class DownloadLatestOperation
{
    public abstract string Message { get; }

    public sealed class Success(string pathToDll) : DownloadLatestOperation
    {
        public override string Message => "Retrieved latest version";
        public string PathToDll { get; } = pathToDll.ThrowIfNull();
    }

    public sealed class NoVersionFound : DownloadLatestOperation
    {
        public override string Message => "Http calls succeeded but no version was found";

        public NoVersionFound()
        {
        }
    }

    public sealed class NonSuccessStatusCode(int statusCode) : DownloadLatestOperation
    {
        public override string Message => "Received unsuccessful status code";
        public int StatusCode { get; } = statusCode;
    }

    public sealed class ExceptionEncountered(Exception exception) : DownloadLatestOperation
    {
        public override string Message => "Encountered exception";
        public Exception Exception { get; } = exception.ThrowIfNull();
    }
}
