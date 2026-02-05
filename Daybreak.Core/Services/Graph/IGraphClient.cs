using Daybreak.Services.Graph.Models;
using Microsoft.AspNetCore.Components;

namespace Daybreak.Services.Graph;

public interface IGraphClient
{
    Task<User?> GetUserProfile<TViewType>()
        where TViewType : ComponentBase;
    Task<bool> PerformAuthorizationFlow(CancellationToken cancellationToken = default);
    Task<bool> LogOut();
    Task<bool> UploadBuilds();
    Task<bool> DownloadBuilds();
    Task<bool> UploadBuild(string buildName);
    Task<bool> DownloadBuild(string buildName);
    Task<bool> UploadSettings(string settings, CancellationToken cancellationToken);
    Task<string?> DownloadSettings(CancellationToken cancellationToken);
    Task<IEnumerable<BuildFile>?> RetrieveBuildsList();
    void ResetAuthorization();
}
