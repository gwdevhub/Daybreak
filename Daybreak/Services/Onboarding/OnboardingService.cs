using Daybreak.Configuration.Options;
using Daybreak.Models.Onboarding;
using Daybreak.Services.Credentials;
using Daybreak.Services.ExecutableManagement;
using Daybreak.Services.LaunchConfigurations;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;

namespace Daybreak.Services.Onboarding;

internal sealed class OnboardingService : IOnboardingService
{
    private readonly ICredentialManager credentialManager;
    private readonly IGuildWarsExecutableManager guildWarsExecutableManager;
    private readonly ILaunchConfigurationService launchConfigurationService;
    private readonly ILiveOptions<LauncherOptions> options;
    private readonly ILogger<OnboardingService> logger;

    public OnboardingService(
        ICredentialManager credentialManager,
        IGuildWarsExecutableManager guildWarsExecutableManager,
        ILaunchConfigurationService launchConfigurationService,
        ILiveOptions<LauncherOptions> liveOptions,
        ILogger<OnboardingService> logger)
    {
        this.credentialManager = credentialManager.ThrowIfNull();
        this.guildWarsExecutableManager = guildWarsExecutableManager.ThrowIfNull();
        this.launchConfigurationService = launchConfigurationService.ThrowIfNull();
        this.options = liveOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public LauncherOnboardingStage CheckOnboardingStage()
    {
        this.logger.LogInformation("Verifying if user is completely onboarded");
        if (this.credentialManager.GetCredentialList().None())
        {
            this.logger.LogError("No credentials found. User needs to create at least one set of credentials");
            return LauncherOnboardingStage.NeedsCredentials;
        }

        if (this.guildWarsExecutableManager.GetExecutableList().None())
        {
            this.logger.LogError("No Guildwars executable found. User needs to set at least one Guildwars executable");
            return LauncherOnboardingStage.NeedsExecutable;
        }

        if (this.launchConfigurationService.GetLaunchConfigurations().None())
        {
            this.logger.LogError("No launch configuration found. User needs to set at least one launch configuration");
            return LauncherOnboardingStage.NeedsConfiguration;
        }

        this.logger.LogInformation("User is onboarded");
        return LauncherOnboardingStage.Complete;
    }
}
