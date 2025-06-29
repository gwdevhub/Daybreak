using Daybreak.Controls;
using Daybreak.Services.Graph.Models;
using System.Extensions;
using System.Windows.Controls;

namespace Daybreak.Services.Graph;

public interface IGraphClient
{
    Task<Result<User, Exception>> GetUserProfile<TViewType>()
        where TViewType : UserControl;
    Task<Result<bool, Exception>> PerformAuthorizationFlow(ChromiumBrowserWrapper chromiumBrowserWrapper, CancellationToken cancellationToken = default);
    Task<Result<bool, Exception>> LogOut();
    Task<Result<bool, Exception>> UploadBuilds();
    Task<Result<bool, Exception>> DownloadBuilds();
    Task<Result<bool, Exception>> UploadBuild(string buildName);
    Task<Result<bool, Exception>> DownloadBuild(string buildName);
    Task<Result<bool, Exception>> UploadSettings(string settings, CancellationToken cancellationToken);
    Task<Result<string, Exception>> DownloadSettings(CancellationToken cancellationToken);
    Task<Result<IEnumerable<BuildFile>, Exception>> RetrieveBuildsList();
    void ResetAuthorization();
}
