using Daybreak.Controls;
using Daybreak.Services.Graph.Models;
using System;
using System.Collections.Generic;
using System.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Daybreak.Services.Graph;

public interface IGraphClient
{
    Task<Result<User, Exception>> GetUserProfile<TViewType>()
        where TViewType : UserControl;
    Task<Result<bool, Exception>> PerformAuthorizationFlow(ChromiumBrowserWrapper chromiumBrowserWrapper, CancellationToken cancellationToken = default);
    Task<Result<bool, Exception>> UploadBuilds();
    Task<Result<bool, Exception>> DownloadBuilds();
    Task<Result<List<BuildFile>, Exception>> RetrieveBuildsList();
    Task<Result<DateTime, Exception>> GetLastUpdateTime();
    void ResetAuthorization();
}
