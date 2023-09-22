using System;
using System.Core.Extensions;

namespace Daybreak.Services.Toolbox.Models;
internal abstract class DownloadLatestOperation
{
    public abstract string Message { get; }

    public sealed class Success : DownloadLatestOperation
    {
        public override string Message => "Retrieved latest version";
        public string PathToDll { get; }
        
        public Success(string pathToDll)
        {
            this.PathToDll = pathToDll.ThrowIfNull();
        }
    }

    public sealed class NoVersionFound : DownloadLatestOperation
    {
        public override string Message => "Http calls succeeded but no version was found";

        public NoVersionFound()
        {
        }
    }

    public sealed class NonSuccessStatusCode : DownloadLatestOperation
    {
        public override string Message => "Received unsuccessful status code";
        public int StatusCode { get; }

        public NonSuccessStatusCode(int statusCode)
        {
            this.StatusCode = statusCode;
        }
    }

    public sealed class ExceptionEncountered : DownloadLatestOperation
    {
        public override string Message => "Encountered exception";
        public Exception Exception { get; }

        public ExceptionEncountered(Exception exception)
        {
            this.Exception = exception.ThrowIfNull();
        }
    }
}
