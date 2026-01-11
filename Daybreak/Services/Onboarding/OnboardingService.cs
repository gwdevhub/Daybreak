using Daybreak.Shared.Models.Onboarding;
using Daybreak.Shared.Services.Credentials;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.LaunchConfigurations;
using Daybreak.Shared.Services.Onboarding;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;

namespace Daybreak.Services.Onboarding;

internal sealed class OnboardingService(
    ICredentialManager credentialManager,
    IGuildWarsExecutableManager guildWarsExecutableManager,
    ILaunchConfigurationService launchConfigurationService,
    ILogger<OnboardingService> logger) : IOnboardingService
{
    private readonly ICredentialManager credentialManager = credentialManager.ThrowIfNull();
    private readonly IGuildWarsExecutableManager guildWarsExecutableManager = guildWarsExecutableManager.ThrowIfNull();
    private readonly ILaunchConfigurationService launchConfigurationService = launchConfigurationService.ThrowIfNull();
    private readonly ILogger<OnboardingService> logger = logger.ThrowIfNull();

    public LauncherOnboardingStage CheckOnboardingStage()
    {
        this.logger.LogDebug("Verifying if user is completely onboarded");
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

        this.logger.LogDebug("User is onboarded");
        return LauncherOnboardingStage.Complete;
    }
}
