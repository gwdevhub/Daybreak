using Daybreak.Configuration;
using Daybreak.Exceptions;
using Daybreak.Models;
using Daybreak.Services.Credentials;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Daybreak.Services.Onboarding;

public sealed class OnboardingService : IOnboardingService
{
    private readonly ICredentialManager credentialManager;
    private readonly ILiveOptions<ApplicationConfiguration> options;
    private readonly ILogger<OnboardingService> logger;

    public OnboardingService(
        ICredentialManager credentialManager,
        ILiveOptions<ApplicationConfiguration> liveOptions,
        ILogger<OnboardingService> logger)
    {
        this.credentialManager = credentialManager.ThrowIfNull();
        this.options = liveOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public async Task<OnboardingStage> CheckOnboardingStage()
    {
        this.logger.LogInformation("Verifying if user is completely onboarded");
        var maybeCredentials = await this.credentialManager.GetDefaultCredentials();
        var credentials = maybeCredentials.ExtractValue();
        if (credentials is null)
        {
            this.logger.LogError("No default credentials found. User needs to set default credentials");
            return OnboardingStage.NeedsCredentials;
        }

        var config = this.options.Value;
        var executable = config.GuildwarsPaths.Where(path => path.Default).FirstOrDefault();
        if (executable is null)
        {
            this.logger.LogError("No default Guildwars executable found. User needs to set default Guildwars executable");
            return OnboardingStage.NeedsExecutable;
        }

        if (File.Exists(executable.Path) is false)
        {
            this.logger.LogError("Default Guildwars executable does not exist. User needs to set default Guildwars executable");
            return OnboardingStage.NeedsExecutable;
        }

        this.logger.LogInformation("User is onboarded");
        return OnboardingStage.Complete;
    }
}
