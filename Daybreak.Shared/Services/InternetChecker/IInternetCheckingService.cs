﻿using Daybreak.Shared.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Shared.Services.InternetChecker;

public interface IInternetCheckingService
{
    Task<InternetConnectionState> CheckConnectionAvailable(CancellationToken cancellationToken);
}
